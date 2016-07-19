using System;
using Android.App;
using Android.Graphics;
using Android.Media;
using Android.Widget;
using Android.Content.Res;
using System.Collections.Generic;

namespace Droid.Media {

	public class ToneGeneratorDemo {

		Activity _activity;


		Button tone1;
		Button tone2;
		Button tone3;
		Button tone4;

		public ToneGeneratorDemo (Activity activity)
		{
			_activity = activity;



			tone1 = _activity.FindViewById<Button>(Resource.Id.tone1);
			tone2 = _activity.FindViewById<Button>(Resource.Id.tone2);
			tone3 = _activity.FindViewById<Button>(Resource.Id.tone3);
			tone4 = _activity.FindViewById<Button>(Resource.Id.tone4);

			var toneGenerator = new ToneGenerator(Stream.Music, Volume.Max);
			var toning1 = false;
			var toning2 = false;
			var toning3 = false;
			var toning4 = false;
			tone1.Click += (object sender, EventArgs e) => 
			{
				if (!toning1) {
					toneGenerator.StartTone(Tone.Dtmf0);
					toning1 = true;
				} else {
					toneGenerator.StopTone();
					toning1 = false;
				}
			};
			tone2.Click += (object sender, EventArgs e) => 
			{
				if (!toning2) {
					toneGenerator.StartTone(Tone.Dtmf1);
					toning2 = true;
				} else {
					toneGenerator.StopTone();
					toning2 = false;
				}
			};
			tone3.Click += (object sender, EventArgs e) => 
			{
				if (!toning3) {
					toneGenerator.StartTone(Tone.Dtmf2);
					toning3 = true;
				} else {
					toneGenerator.StopTone();
					toning3 = false;
				}
			};
			tone4.Click += (object sender, EventArgs e) => 
			{
				if (!toning4) {
					toneGenerator.StartTone(Tone.Dtmf3);
					toning4 = true;
				} else {
					toneGenerator.StopTone();
					toning4 = false;
				}
			};
		}
    }
}
