using System;
using Android.App;
using Android.Graphics;
using Android.Media;
using Android.Widget;
using Android.Content.Res;
using System.Collections.Generic;
using System.Linq;
using Android.Views;

namespace Droid.Media {

	public class Drum {

		Activity _activity;

		SoundPool _soundPool;

		ImageView _drum;

		public Drum (Activity activity)
		{
			_activity = activity;
			_soundPool = new SoundPool(32, Stream.Music, 0);
			_drum = _activity.FindViewById<ImageView>(Resource.Id.drum);
			Notes.Add("snare", _soundPool.Load(_activity, Resource.Raw.drum_snare_slam, 10));
			Notes.Add("snare_rim", _soundPool.Load(_activity, Resource.Raw.drum_snare_rim, 10));
			Notes.Add("tum1", _soundPool.Load(_activity, Resource.Raw.drum_tom_hi, 10));
			Notes.Add("tum2", _soundPool.Load(_activity, Resource.Raw.drum_tom_med, 10));
			Notes.Add("tum3", _soundPool.Load(_activity, Resource.Raw.drum_tom_low, 10));
			Notes.Add("bd_flat", _soundPool.Load(_activity, Resource.Raw.drum_kick_flat, 10));
			Notes.Add("bd_gated", _soundPool.Load(_activity, Resource.Raw.drum_kick_gated, 10));
			Notes.Add("bd_low", _soundPool.Load(_activity, Resource.Raw.drum_kick_low, 10));
			Notes.Add("hihat", _soundPool.Load(_activity, Resource.Raw.drum_hihat, 10));
			Notes.Add("cymb", _soundPool.Load(_activity, Resource.Raw.drum_crash, 10));
			Notes.Add("ride", _soundPool.Load(_activity, Resource.Raw.drum_ride, 10));

			drumPieces.Add(new DrumPiece("snare", snare));
			drumPieces.Add(new DrumPiece("tum1", tum1));
			drumPieces.Add(new DrumPiece("tum2", tum2));
			drumPieces.Add(new DrumPiece("tum3", floor));
			drumPieces.Add(new DrumPiece("bd_flat", bd));
			drumPieces.Add(new DrumPiece("hihat", hihat));
			drumPieces.Add(new DrumPiece("cymb", cymb));
			drumPieces.Add(new DrumPiece("ride", ride));

			// Oups, no multitouch implemented yet. :(
			_drum.Touch += (object sender, View.TouchEventArgs e) => {
				var x = (int) e.Event.GetX();
				var y = (int) e.Event.GetY();

				var tappedPiece = drumPieces.ToList().FirstOrDefault(piece => piece.Rec.Contains(x, y));
				if (tappedPiece != null) {
					Play(Notes.FirstOrDefault(note => note.Key == tappedPiece.Note).Value);
				}
				e.Handled = false;
			};
		}

		public List<DrumPiece> drumPieces = new List<DrumPiece>();

		public class DrumPiece
		{
			public DrumPiece(string note, Rect rec) {
				Note = note;
				Rec = rec;
			}

			public string Note {get;set;}
			public Rect Rec {get;set;}
		}

		Rect snare = new Rect(86, 202, 173, 250);
		Rect tum1 = new Rect(146, 137, 223, 191);
		Rect tum2 = new Rect(223, 137, 307, 191);
		Rect floor = new Rect(259, 213, 360, 275);
		Rect bd = new Rect(170, 270, 278, 426);
		Rect hihat = new Rect(10, 95, 108, 140);
		Rect cymb = new Rect(64, 28, 180, 80);
		Rect ride = new Rect(280, 65, 380, 118);

		Dictionary<string, int> Notes = new Dictionary<string, int>();

		void Play(int id)
		{
			_soundPool.Play(id, 1, 1, 10, 0, 1);
		}
    }
}
