using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Speech.Tts;
using Android.Views;
using Android.Widget;
using Android.Text;
using System.IO;
using System.Threading.Tasks;
using Android.Webkit;
using Java.Net;

namespace Droid.Media
{
	[Activity (Label = "Droid.Media", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		private WebView _sourceBrowser;
		private ScrollView _sourceBrowserContainer;
		private Button _sourceButton;
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			this.RequestWindowFeature (Android.Views.WindowFeatures.NoTitle);
			this.Window.SetFlags (Android.Views.WindowManagerFlags.Fullscreen, Android.Views.WindowManagerFlags.Fullscreen);
			SetContentView (Resource.Layout.Main);
			InitSideList ();
			InitAnimations ();
			InitNavigation ();
			_sourceButton = this.FindViewById<Button> (Resource.Id.sourceButton);
			_sourceBrowser = this.FindViewById<WebView> (Resource.Id.sourceBrowser);
			_sourceBrowser.VerticalScrollBarEnabled = true;
			_sourceBrowser.HorizontalScrollBarEnabled = true;
			_sourceBrowserContainer = this.FindViewById<ScrollView> (Resource.Id.sourceBrowserContainer);

			_sourceButton.Click += (object sender, EventArgs e) => {
				_sourceBrowserContainer.Visibility = _sourceBrowserContainer.Visibility == ViewStates.Gone ? ViewStates.Visible : ViewStates.Gone;
				NavigateToSource ();
			};

			var _gestureDetector = new GestureDetector (this, new GestureListener ());

			_gestureDetector.DoubleTap += (object sender, GestureDetector.DoubleTapEventArgs e) => {
				_sourceBrowserContainer.Visibility = ViewStates.Gone;
			};

			
			_sourceBrowser.Touch += (object sender, View.TouchEventArgs e) => {
				_gestureDetector.OnTouchEvent (e.Event);
				e.Handled = false;
			};
		}

		void InitSideList ()
		{
			var namespacesView = FindViewById<LinearLayout> (Resource.Id.namespaces);
			namespacesView.RemoveAllViews();

			var strs = new string [] {

				"AudioRecord",
				"AudioFX",
				"AudioManager",
				"AudioTrack",

				"FaceDetector",
				"TextureView",
				"MediaActionSound",
				"ExifInterface",

				"JetPlayer",

				"MediaRecorder",
				"ThumbnailUtils",

				"Ringtone",
				"RingtoneManager",

				"ToneGenerator",
				"SoundPool"
			};

			for (var i = 0; i < strs.Length; i++) {
				var ns = strs [i];
				namespacesView.AddView (MakeText(ns, Color.DarkGray, 9));
			}
		}

		void InitNavigation() {

			var back = FindViewById<Button>(Resource.Id.back);
			var next = FindViewById<Button>(Resource.Id.next);

			back.Click += (s,e) => {
				Navigate(--currentPage);
			};

			next.Click += (s,e) => {
				Navigate(++currentPage);
			};

			Navigate(currentPage);
		}

		int lastPage = 20;
		int currentPage = 20;

		void Navigate (int page)
		{
			InitSideList();

			var layoutId = 0;
			Tuto.RemoveAllViews ();

			switch (currentPage = page = page < 0 ? lastPage : page) {
			case 0:
				layoutId = Resource.Layout._0000intro;
				sourceCodeUrl = "";
				break;
			case 1:
				layoutId = Resource.Layout._0010intro;
				sourceCodeUrl = "";
				break;
			case 2:
				layoutId = Resource.Layout._0020intro;
				sourceCodeUrl = "";
				break;
			case 3:
				layoutId = Resource.Layout._0030intro;
				sourceCodeUrl = "";
				break;
			case 4:
				layoutId = Resource.Layout._0040intro;
				sourceCodeUrl = "";
				break;
			case 5:
				layoutId = Resource.Layout._0050intro;
				sourceCodeUrl = "";
				break;
			case 6:
				layoutId = Resource.Layout._0060audio1;
				sourceCodeUrl = "";
				break;
			case 7:
				layoutId = Resource.Layout._0070audio2;
				sourceCodeUrl = "";
				break;
			case 8:
				layoutId = Resource.Layout._0080audio3;
				sourceCodeUrl = "";
				break;
			case 9:
				layoutId = Resource.Layout._0090audio4;
				sourceCodeUrl = "";
				break;
			case 10:
				layoutId = Resource.Layout._0100audio5;
				sourceCodeUrl = "";
				break;
			case 11:
				layoutId = Resource.Layout._0110audio6;
				sourceCodeUrl = "";
				break;
			case 12:
				layoutId = Resource.Layout._0120audio7;
				sourceCodeUrl = "";
				break;
			case 13:
				layoutId = Resource.Layout._0130camera1;
				sourceCodeUrl = "";
				break;
			case 14:
				layoutId = Resource.Layout._0140camera2;
				sourceCodeUrl = "";
				break;
			case 15:
				layoutId = Resource.Layout._0150camera3;
				sourceCodeUrl = "";
				break;
			case 16:
				layoutId = Resource.Layout._0160jetplayer;
				sourceCodeUrl = "";
				break;
			case 17:
				layoutId = Resource.Layout._0170video;
				sourceCodeUrl = "https://github.com/leye0/Droid.Media/blob/master/MediaRecorder.cs";
				break;
			case 18:
				layoutId = Resource.Layout._0180ringtone;
				sourceCodeUrl = "https://github.com/leye0/Droid.Media/blob/master/Ring.cs";
				break;
			case 19:
				layoutId = Resource.Layout._0190tonegenerator;
				sourceCodeUrl = "https://github.com/leye0/Droid.Media/blob/master/ToneGenerator.cs";
				break;
			case 20:
				layoutId = Resource.Layout._0200soundpool;
				sourceCodeUrl = "https://github.com/leye0/Droid.Media/blob/master/SoundPool.cs";
				break;
			case 21:
				layoutId = Resource.Layout._0210drum;
				sourceCodeUrl = "https://github.com/leye0/Droid.Media/blob/master/Drum.cs";
				break;
			default:
				currentPage = 0;
				sourceCodeUrl = "";
				layoutId = Resource.Layout._0000intro;
				break;
			}


			FindViewById<Button> (Resource.Id.sourceButton).Visibility = (sourceCodeUrl == "") ? ViewStates.Invisible : ViewStates.Visible; 
			FindViewById<TextView> (Resource.Id.page).Text = (currentPage + 1).ToString () + "/" + (lastPage + 1).ToString ();
			var inflater = LayoutInflater.From (this);
			inflater.Inflate (layoutId, Tuto);
			CustomStuff();
		}

		void Hili (string[] names)
		{
			InitSideList ();
			var namespacesView = FindViewById<LinearLayout> (Resource.Id.namespaces);
			for (var i = 0; i < namespacesView.ChildCount; i++) {
				var namespaceTextView = namespacesView.GetChildAt (i) as TextView;
				if (namespacesView != null) {
					if (names.Contains (namespaceTextView.Text)) {
						namespaceTextView.SetTextColor(Color.Argb(0xFF, 0xA4, 0xC6, 0x39));
					}
				}
			}

			names.ToList().ForEach(name => {
				View[] views = new View[0];
				namespacesView.FindViewsWithText(views, name, FindViewsWith.Text);

			});
		}

		AudioRecordDemo _audioRecordDemo;
		private string sourceCodeUrl = "";
		async Task CustomStuff ()
		{
			// Page before starting the examples
			if (currentPage == 5) {
				Animate (Resource.Id.robotAnimation, RobotAnimation);
			}

			// Pages with AudioRecord
			if (currentPage >= 6 && currentPage <= 12) {
				Hili (new [] { "AudioRecord", "AudioFX", "AudioManager", "AudioTrack" });
			}

			if (currentPage >= 13 && currentPage <= 15) {
				Hili (new [] { "FaceDetector", "TextureView", "MediaActionSound", "ExifInterface" });
			}

			if (currentPage >= 16 && currentPage <= 16) {
				Hili (new [] { "JetPlayer" });
			}

			if (currentPage >= 17 && currentPage <= 17) {
				Hili (new [] { "MediaRecorder", "ThumbnailUtils" });
			}

			if (currentPage >= 18 && currentPage <= 18) {
				Hili (new [] { "Ringtone", "RingtoneManager" });
			}

			if (currentPage >= 19 && currentPage <= 19) {
				Hili (new [] { "ToneGenerator" });
			}

			if (currentPage >= 19 && currentPage <= 19) {
				Hili (new [] { "SoundPool" });
			}

			// Page where we record a sound
			if (currentPage == 7) {
				var audioRecord = new AudioRecordDemo(this);
			}

			// Page where we play the sound
			if (currentPage == 8) {
				var audioTrack = new AudioTrackDemo(this);
			}

			// Page where we play the sound with FX
			if (currentPage == 12) {
				var audioTrack = new AudioTrackDemo(this, true);
			}

			// Page where we capture the cam and face
			if (currentPage == 14) {
				var camera = new CameraAndFaceDetection(this);
			}

			if (currentPage == 15) {
				var exif = new Exif(this);
			}

			if (currentPage == 16) {
				var jetPlayer = new JetPlayerDemo(this);
			}

			if (currentPage == 17) {
				var videoView = new VideoViewDemo(this);
			}

			if (currentPage == 18) {
				var ring = new Ring(this);
			}

			if (currentPage == 19) {
				var toneGenerator = new ToneGeneratorDemo(this);
			}

			if (currentPage == 20) {
				var soundPoolDemo = new SoundPoolDemo(this);
			}

			if (currentPage == 21) {
				var drumDemo = new Drum(this);
			}
		}

		Dictionary<string, TextView> TextToNS = new Dictionary<string, TextView>();

		ScrollView _tuto;
		ScrollView Tuto {
			get { 
				return _tuto ?? (_tuto = FindViewById<ScrollView>(Resource.Id.tuto));
			}
		}

		TextView MakeText (string text, Color color, int size)
		{
			var textView = TextToNS [text] = new TextView (this)
			{
				TextSize = size,
				Text = text,
				LayoutParameters =  new LinearLayout.LayoutParams (LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent)
			};
			textView.SetTextColor(color);
			return textView;
		}

		void InitAnimations ()
		{
			RobotAnimation = new List<KeyValuePair<int, int>> () {
				new KeyValuePair<int, int> (Robot, 250),
				new KeyValuePair<int, int> (RobotTalk, 100),
				new KeyValuePair<int, int> (Robot, 450),
				new KeyValuePair<int, int> (RobotTalk, 150),
				new KeyValuePair<int, int> (Robot, 250),
				new KeyValuePair<int, int> (RobotTalk, 150),
				new KeyValuePair<int, int> (Robot, 500)
			};
		}

		void NavigateToSource ()
		{
			_sourceBrowser.LoadUrl (sourceCodeUrl);
		}

		int Robot{ get{ return Resource.Drawable.robot; }}
		int RobotTalk{ get{ return Resource.Drawable.robottalk; }}

		List<KeyValuePair<int, int>> RobotAnimation = new List<KeyValuePair<int, int>>();

		void Animate (int imageViewId, List<KeyValuePair<int, int>> animation)
		{
			var staticAnimation = new AnimationDrawable();
			animation.ForEach(image => staticAnimation.AddFrame(this.Resources.GetDrawable(image.Key), image.Value));
			FindViewById(imageViewId).SetBackgroundDrawable(staticAnimation );
			staticAnimation.Start();
		}

		private class GestureListener : GestureDetector.SimpleOnGestureListener
		{
		    public override bool OnDown(MotionEvent e)
		    {
		        return true;
		    }

		    public override bool OnDoubleTap(MotionEvent e)
		    {
		        return true;
		    }
		}
	}

}
