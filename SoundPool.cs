using System;
using Android.App;
using Android.Graphics;
using Android.Media;
using Android.Widget;
using Android.Content.Res;
using System.Collections.Generic;

namespace Droid.Media {

	public class SoundPoolDemo {

		Activity _activity;


		Button c1;
		Button d1;
		Button e1;
		Button f1;
		Button g1;
		Button a1;
		Button b1;
		Button c2;
		SoundPool _soundPool;

		public SoundPoolDemo (Activity activity)
		{
			_activity = activity;

			_soundPool = new SoundPool(64, Stream.Music, 0);

			c1 = _activity.FindViewById<Button>(Resource.Id.sp_c);
			d1 = _activity.FindViewById<Button>(Resource.Id.sp_d);
			e1 = _activity.FindViewById<Button>(Resource.Id.sp_e);
			f1 = _activity.FindViewById<Button>(Resource.Id.sp_f);
			g1 = _activity.FindViewById<Button>(Resource.Id.sp_g);
			a1 = _activity.FindViewById<Button>(Resource.Id.sp_a);
			b1 = _activity.FindViewById<Button>(Resource.Id.sp_b);
			c2 = _activity.FindViewById<Button>(Resource.Id.sp_c2);

			Notes.Add("c1", _soundPool.Load(_activity, Resource.Raw.c1, 10));
			Notes.Add("d1", _soundPool.Load(_activity, Resource.Raw.d1, 10));
			Notes.Add("e1", _soundPool.Load(_activity, Resource.Raw.e1, 10));
			Notes.Add("f1", _soundPool.Load(_activity, Resource.Raw.f1, 10));
			Notes.Add("g1", _soundPool.Load(_activity, Resource.Raw.g1, 10));
			Notes.Add("a1", _soundPool.Load(_activity, Resource.Raw.a1, 10));
			Notes.Add("b1", _soundPool.Load(_activity, Resource.Raw.b1, 10));
			Notes.Add("c2", _soundPool.Load(_activity, Resource.Raw.c2, 10));

			c1.Click += (object sender, EventArgs e) => Play(Notes["c1"]);
			d1.Click += (object sender, EventArgs e) => Play(Notes["d1"]);
			e1.Click += (object sender, EventArgs e) => Play(Notes["e1"]);
			f1.Click += (object sender, EventArgs e) => Play(Notes["f1"]);
			g1.Click += (object sender, EventArgs e) => Play(Notes["g1"]);
			a1.Click += (object sender, EventArgs e) => Play(Notes["a1"]);
			b1.Click += (object sender, EventArgs e) => Play(Notes["b1"]);
			c2.Click += (object sender, EventArgs e) => Play(Notes["c2"]);
		}

		Dictionary<string, int> Notes = new Dictionary<string, int>();
		void Play(int id)
		{
			_soundPool.Play(id, 1, 1, 10, 0, 1);
		}
    }
}
