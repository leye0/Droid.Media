using System;
using Android.App;
using Android.Graphics;
using Android.Media;
using Android.Widget;
using Android.Content.Res;

namespace Droid.Media {

	public class JetPlayerDemo {

		Activity _activity;
	    Button _playJetContent;
		Button _playMidiSong;

		public JetPlayerDemo(Activity activity) {

			_activity = activity;
			var jetPlayer = Android.Media.JetPlayer.GetJetPlayer();
			jetPlayer.LoadJetFile(_activity.Resources.OpenRawResourceFd(Resource.Raw.level1));
			byte segmentId = 0;
			jetPlayer.QueueJetSegment(5, -1, 1, -1, 0, (sbyte)segmentId++);
			_playJetContent = _activity.FindViewById<Button>(Resource.Id.playJetContent);
			_playJetContent.Click += (object sender, EventArgs e) => 
			{
				jetPlayer.Play();
			};

			var mediaPlayer = new MediaPlayer();
			_playMidiSong = _activity.FindViewById<Button>(Resource.Id.playMidiSong);
			_playMidiSong.Click += (object sender, EventArgs e) => 
			{
				if (mediaPlayer.IsPlaying) {
					mediaPlayer.Stop();
					mediaPlayer.Dispose();
					mediaPlayer = new MediaPlayer();
				} else {
					mediaPlayer = MediaPlayer.Create(activity, Resource.Raw.smb1);
					mediaPlayer.Start();
				}

			};
		}
    }
}
