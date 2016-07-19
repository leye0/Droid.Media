using System;
using Android.App;
using Android.Graphics;
using Android.Media;
using Android.Widget;

namespace Droid.Media {

	public class Exif {

		Activity _activity;
	    ImageView _exifImage;
		EditText _latitude;
		EditText _longitude;
		Button _setExifTags;

		public Exif(Activity activity) {
			var path = Android.OS.Environment.ExternalStorageDirectory + "/camera-face.jpg";
			_activity = activity;
			_exifImage = _activity.FindViewById<ImageView>(Resource.Id.exifImage);

			var bitmap = BitmapFactory.DecodeFile(path);
			_exifImage.SetImageBitmap(bitmap);

			_latitude = _activity.FindViewById<EditText>(Resource.Id.latitude);
			_longitude = _activity.FindViewById<EditText>(Resource.Id.longitude);
			_setExifTags = _activity.FindViewById<Button>(Resource.Id.setExifTags);
			_setExifTags.Click += (object sender, EventArgs e) => 
			{
				var exif = new ExifInterface(path);
				exif.SetAttribute(ExifInterface.TagGpsLatitude, _latitude.Text);
				exif.SetAttribute(ExifInterface.TagGpsLongitude, _longitude.Text);
				exif.SaveAttributes();
				_setExifTags.Text = "Lat: " + exif.GetAttribute(ExifInterface.TagGpsLatitude).ToString() 
					+ ", Lng: " + exif.GetAttribute(ExifInterface.TagGpsLongitude).ToString();
			};
		}
    }
}
