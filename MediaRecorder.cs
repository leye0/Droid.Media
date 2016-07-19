using System;
using Android.App;
using Android.Graphics;
using Android.Media;
using Android.Widget;
using Android.Content.Res;

namespace Droid.Media {

	public class VideoViewDemo {

		Activity _activity;
		VideoView video;
		MediaRecorder recorder;
		Button _toggleVideo;

		public VideoViewDemo (Activity activity)
		{
			_activity = activity;
			var path = Android.OS.Environment.ExternalStorageDirectory + "/videoview-demo.mp4";
			video = _activity.FindViewById<VideoView> (Resource.Id.videoView);

			_toggleVideo = _activity.FindViewById<Button>(Resource.Id.toggleVideo);
			_toggleVideo.Visibility = Android.Views.ViewStates.Visible;
			_toggleVideo.Text = "Record";

			var isRecording = false;

			_toggleVideo.Click += (object sender, EventArgs e) => {
				if (!isRecording) {
					isRecording = true;
					_toggleVideo.Text = "Stop";
					new MediaActionSound ().Play (MediaActionSoundType.ShutterClick);
					video.StopPlayback ();
					recorder = new MediaRecorder ();
					recorder.SetVideoSource (VideoSource.Camera);
					recorder.SetAudioSource (AudioSource.Mic);
					recorder.SetOutputFormat (OutputFormat.Default);
					recorder.SetVideoEncoder (VideoEncoder.Default);
					recorder.SetAudioEncoder (AudioEncoder.Default);
					recorder.SetOutputFile (path);
					recorder.SetPreviewDisplay (video.Holder.Surface);
					recorder.Prepare ();
					recorder.Start ();
				} else {
					if (recorder != null) {
						new MediaActionSound ().Play (MediaActionSoundType.ShutterClick);
						recorder.Stop ();
						recorder.Release ();
						var uri = Android.Net.Uri.Parse (path);
						_toggleVideo.Visibility = Android.Views.ViewStates.Gone;
						video.SetVideoURI (uri);
						video.Start ();
						ShowThumbnailUtils(uri);
					}
				}
			};
		}

		void ShowThumbnailUtils(Android.Net.Uri uri) {
			var tFull = ThumbnailUtils.CreateVideoThumbnail (uri.Path, Android.Provider.ThumbnailKind.FullScreenKind);
			var tMicro = ThumbnailUtils.CreateVideoThumbnail (uri.Path, Android.Provider.ThumbnailKind.MicroKind);
			var tnViewFull = _activity.FindViewById<ImageView>(Resource.Id.thumbnailUtilsViewFull);
			var tnViewMicro= _activity.FindViewById<ImageView>(Resource.Id.thumbnailUtilsViewMicro);
			tnViewFull.SetImageBitmap(tFull);
			tnViewMicro.SetImageBitmap(tMicro);
		}
    }
}
