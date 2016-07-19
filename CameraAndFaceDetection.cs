using Android.Views;
using Android.Content;
using Android.Hardware;
using System;
using Android.App;
using Android.Widget;
using Android.Gms.Vision;
using Android.Media;
using Bitmap = Android.Graphics.Bitmap;
using Java.Interop;
using System.Threading.Tasks;
using System.Linq;

namespace Droid.Media {

	public class CameraAndFaceDetection: Java.Lang.Object, TextureView.ISurfaceTextureListener  {

		Camera _camera;
		TextureView _textureView;
	    Activity _activity;
	    FaceDetector _detector;
		FaceDetector.Face[] _faces;
		ImageView _faceResult;

		public CameraAndFaceDetection(Activity activity) {
			_activity = activity;
			_textureView = _activity.FindViewById<TextureView>(Resource.Id.camera);
			_textureView.SurfaceTextureListener = this;
			_faces = new FaceDetector.Face[4];
			_faceResult = _activity.FindViewById<ImageView>(Resource.Id.faceResult);
			_faceResult.Click += (object sender, EventArgs e) => 
			{
				var bitmap = (_faceResult.Drawable as Android.Graphics.Drawables.BitmapDrawable).Bitmap;
				var fos = new System.IO.FileStream(Android.OS.Environment.ExternalStorageDirectory + "/camera-face.jpg", System.IO.FileMode.Create);
				bitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, fos);
                fos.Close();
				new MediaActionSound().Play(MediaActionSoundType.ShutterClick);
			};

			new MediaActionSound().Play(MediaActionSoundType.ShutterClick);
		}

		void Init ()
		{
		}

		private void ReleaseCamera() {
		    
		    if (_camera != null) {
		        _camera.Release();
		        _camera = null;
		    }
		}

		public static Camera CameraInstance {
			get {
				Camera c = null;
				try {
					c = Camera.Open (); // attempt to get a Camera instance
				} catch (Exception e) {
					// Camera is not available (in use or does not exist)
				}
				return c; // returns null if camera is unavailable
			}
		}

		public void OnSurfaceTextureAvailable (Android.Graphics.SurfaceTexture surface, int width, int height)
		{
			ReleaseCamera ();
			_camera = CameraInstance;

			_textureView.LayoutParameters =
		              new FrameLayout.LayoutParams (width, height);

			try {
				_detector = new FaceDetector (_textureView.Width, _textureView.Height, 4);
				var parameters = _camera.GetParameters ();
			        
				var display = this._activity.GetSystemService (Context.WindowService).JavaCast<IWindowManager> ().DefaultDisplay;

				if (display.Rotation == SurfaceOrientation.Rotation0) {
					parameters.SetPreviewSize (height, width);  
					_camera.SetDisplayOrientation (90);
				}

				if (display.Rotation == SurfaceOrientation.Rotation90) {
					parameters.SetPreviewSize (width, height);                           
				}

				if (display.Rotation == SurfaceOrientation.Rotation180) {
					parameters.SetPreviewSize (height, width);
				}

				if (display.Rotation == SurfaceOrientation.Rotation270) {
					parameters.SetPreviewSize (width, height);
					_camera.SetDisplayOrientation (180);
				}

				_camera.SetPreviewTexture (surface);
				_camera.StartPreview ();

				Init ();

			} catch (Java.IO.IOException ex) {
				Console.WriteLine (ex.Message);
			}
		}

		public void OnSurfaceTextureSizeChanged (Android.Graphics.SurfaceTexture surface, int w, int h)
		{
		}

		public void OnSurfaceTextureUpdated (Android.Graphics.SurfaceTexture surface)
		{
			GetFace();
		}

		Random _rnd = new Random();
		private bool _isLocked;
		private async Task GetFace ()
		{
			if (!_isLocked) {
				_isLocked = true;
				var bitmap = _textureView.Bitmap.Copy (Bitmap.Config.Rgb565, true);
				var result = await _detector.FindFacesAsync (bitmap, _faces);
				if (result > 0) {
					var point = new Android.Graphics.PointF ();
					_faces [0].GetMidPoint (point);
					var eyeDistance = _faces [0].EyesDistance ();
					var canvas = new Android.Graphics.Canvas (bitmap);
					var x = (int)point.X;
					var y = (int)point.Y;
					var p = new Android.Graphics.Paint ();
					p.Color = Android.Graphics.Color.Black;
					canvas.DrawRect (x - eyeDistance * 2f, y - eyeDistance * 0.6f, x + eyeDistance * 2f, y - eyeDistance * 0.8f, p);
					canvas.DrawRect (x - eyeDistance, y - eyeDistance * 0.8f, x + eyeDistance, y - eyeDistance * 1.4f, p);
					for (var i = (eyeDistance / 2) + 1; i > 1; i--) {
						p.SetARGB(255, _rnd.Next(256), _rnd.Next(256), _rnd.Next(256));
						canvas.DrawCircle (x - (eyeDistance / 2), y, i, p);
						canvas.DrawCircle (x + (eyeDistance / 2), y, i, p);
					}

					_faceResult.SetImageBitmap(bitmap);
				}

				GC.Collect(); // There is no memory management in the GL layer when obtaining the pixel array. Need to force GC.
				Console.WriteLine (result);
				_isLocked = false;
			}
		}

		public bool OnSurfaceTextureDestroyed (Android.Graphics.SurfaceTexture surface)
		{
		       _camera.StopPreview ();
		       _camera.Release ();
		       _detector.Dispose();
		       return true;
		}
    }
}
