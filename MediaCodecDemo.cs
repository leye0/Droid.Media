using System;
using Android.App;
using Android.Widget;

namespace Droid.Media
{
	public class MediaCodecDemo
	{
		public MediaCodecDemo (Activity activity)
		{
			var videoView = activity.FindViewById<GlVideoView>(Resource.Id.myVideoView);
			var movie = Android.OS.Environment.ExternalStorageDirectory + "/videoview-demo.mp4";

			videoView.SetSource (movie);

			activity.FindViewById<Button>(Resource.Id.playMediaCodec).Click += (sender, e) => 
			{
				videoView.Play();
			};
		}
	}
}

