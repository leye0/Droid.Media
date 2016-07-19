using System;
using Android.App;
using Android.Graphics;
using Android.Media;
using Android.Widget;
using Android.Content.Res;
using System.Collections.Generic;

namespace Droid.Media {

	public class Ring {

		Activity _activity;


		Button playRingtone;
		Button playAlarm;
		Button playNotification;
		Button shutup;

		public Ring (Activity activity)
		{
			_activity = activity;



			shutup = _activity.FindViewById<Button>(Resource.Id.shutup);
			shutup.Click += (object sender, EventArgs e) => 
			{
				foreach (var ringtone in ringtones)
				{
					ringtone.Stop();
				}
				ringtones.Clear();
			};

			playNotification = _activity.FindViewById<Button>(Resource.Id.playNotification);
			playNotification.Click += (object sender, EventArgs e) => {
				var notification = RingtoneManager.GetDefaultUri(RingtoneType.Notification);
				Ringtone r = RingtoneManager.GetRingtone(_activity, notification);
				ringtones.Add(r);
				r.Play();
			};

			playAlarm = _activity.FindViewById<Button>(Resource.Id.playAlarm);
			playAlarm.Click += (object sender, EventArgs e) => {
				var notification = RingtoneManager.GetDefaultUri(RingtoneType.Alarm);
				Ringtone r = RingtoneManager.GetRingtone(_activity, notification);
				ringtones.Add(r);
				r.Play();
			};

			playRingtone = _activity.FindViewById<Button>(Resource.Id.playRingtone);
			playRingtone.Click += (object sender, EventArgs e) => {
				var notification = RingtoneManager.GetDefaultUri(RingtoneType.Ringtone);
				Ringtone r = RingtoneManager.GetRingtone(_activity, notification);
				ringtones.Add(r);
				r.Play();
			};
		}

		List<Ringtone> ringtones = new List<Ringtone>();
    }
}
