using System;
using Android.Content;
using Android.Media;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using Android.Widget;
using Android.App;
using System.Linq;
using Android.Graphics;
using System.Collections.Generic;
using Android.Media.Audiofx;

namespace Droid.Media
{
	public class AudioTrackDemo
	{
		Button _playButton;
		Button _playButton2;

		string _filePath = "";

		Activity _activity;
		Activity Activity {
			set { 
				_activity = value;
			}
			get { 
				return _activity;
			}
		}

		bool hasFx = false;
		public AudioTrackDemo (Activity activity, bool withEffect = false)
		{
			Activity = activity;
			hasFx = withEffect;

			_playButton = Activity.FindViewById<Button> (Resource.Id.playButton);
			_playButton.ViewDetachedFromWindow += (object sender, Android.Views.View.ViewDetachedFromWindowEventArgs e) => {
				this.Dispose ();
			};
		
			_playButton2 = Activity.FindViewById<Button> (Resource.Id.playButton2);


			var dir = new Java.IO.File (Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/DroidMediaTutorials/");
			_filePath = dir.AbsolutePath + "/audioRecordDemo.pcm";

			_playButton.Click += _playButtonClick;
			if (withEffect) {
				_playButton2.Click += _playButton2Click;
			}
		}

		void _playButtonClick (object sender, EventArgs e)
		{
			var bytes = File.ReadAllBytes(_filePath);
			Task.Run(() => {
				var audioTrack = new AudioTrack(Android.Media.Stream.Music, 44100, ChannelOut.Stereo, Encoding.Default, 1024 * 20, AudioTrackMode.Stream);
				if (hasFx) {
					SetFX(audioTrack);
				}
				audioTrack.Play();
				audioTrack.Write(bytes, 0, bytes.Length);
			});
		}

		void _playButton2Click (object sender, EventArgs e)
		{
			var bytes = File.ReadAllBytes (_filePath);
			var bytes2x = new List<byte> ();
			var bytes4x = new List<byte> ();
			for (int i = 0; i < bytes.Length; i += 8) {
				bytes2x.Add (bytes [i]);
				bytes2x.Add (bytes [i + 1]);
				bytes2x.Add (bytes [i + 2]);
				bytes2x.Add (bytes [i + 3]);
				bytes4x.Add (bytes [i]);
				bytes4x.Add (bytes [i + 1]);
			}

			var audioTrack = new AudioTrack(Android.Media.Stream.Music, 44100, ChannelOut.Stereo, Encoding.Default, 1024 * 20, AudioTrackMode.Stream);
				if (hasFx) {
					SetFX(audioTrack);
				}
			Task.Run(() => {
				audioTrack.Play();
				audioTrack.Write(bytes, 0, bytes.Length);
				audioTrack.Write(bytes2x.ToArray(), 0, bytes2x.ToArray().Length);
				audioTrack.Write(bytes4x.ToArray(), 0, bytes4x.ToArray().Length);
			});
		}

		void SetFX(AudioTrack audioTrack) {
			var reverb = new EnvironmentalReverb(0, audioTrack.AudioSessionId);
        	reverb.SetEnabled(true);
		}

		// On exit
		void Dispose ()
		{
			_playButton.Click -= _playButtonClick;
			if (_playButton2 != null) {
				_playButton2.Click -= _playButtonClick;
			}
		}
	}
}

