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

namespace Droid.Media
{
	public class AudioRecordDemo
	{
		Button _record;

		AudioRecord _audioRecord = null;

		string _filePath = "";
		byte[] _audioBuffer = null;
		bool _endRecording = false;
		bool _isRecording = false;

		Activity _activity;
		Activity Activity {
			set { 
				_activity = value;
			}
			get { 
				return _activity;
			}
		}

		public AudioRecordDemo (Activity activity)
		{
			Activity = activity;
			_record = Activity.FindViewById<Button> (Resource.Id.recordButton);

			_record.ViewDetachedFromWindow += (object sender, Android.Views.View.ViewDetachedFromWindowEventArgs e) => {
				this.Dispose ();
			};

			var dir = new Java.IO.File (Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/DroidMediaTutorials/");
			if (!dir.Exists ()) {
				dir.Mkdirs ();
			}
        	
			_filePath = dir.AbsolutePath + "/audioRecordDemo.pcm";
			_record.Click += _recordButton_Click;
		}

		void _recordButton_Click (object sender, EventArgs e)
		{

			if (!_isRecording) {
				_record.Text = "Recording...";
				StartAsync ();
			} else {
				_record.Text = "Stopped";
				Stop ();
				Activity.FindViewById<TextView>(Resource.Id.listenToIt).Visibility = Android.Views.ViewStates.Visible;
			}
		}

		async Task StartRecordAsync ()
		{

			_endRecording = false;
			_isRecording = true;

			_audioBuffer = new Byte[1024 * 10];
			_audioRecord = new AudioRecord (

                // Hardware source of recording.
				AudioSource.Mic,

                // Frequency
				44100,

                // Channels
				ChannelIn.Stereo,

                // Encoding
				Android.Media.Encoding.Pcm16bit,

                // Buffer size
				_audioBuffer.Length
			);

			_audioRecord.StartRecording ();

			// Off line this so that we do not block the UI thread.
			await ReadAudioAsync ();
		}

		Dictionary<int, int> pixels = new Dictionary<int, int>();

		int _average = 0;
		async Task ReadAudioAsync ()
		{
			using (var fileStream = new FileStream (_filePath, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite)) {
				while (true) {
					if (_endRecording) {
						_endRecording = false;
						break;
					}
					try {
						// Keep reading the buffer while there is audio input.
						int numBytes = await _audioRecord.ReadAsync (_audioBuffer, 0, _audioBuffer.Length);
						await fileStream.WriteAsync (_audioBuffer, 0, numBytes);

						// Do something with the audio input.
						var bufferAsAmplitudes = new short[_audioBuffer.Length * 2];
						Buffer.BlockCopy(_audioBuffer, 0, bufferAsAmplitudes, 0, _audioBuffer.Length);

						_average += ((int) Math.Abs(bufferAsAmplitudes.Where(x => true).Average(x => x))) > 10 ? 30 : -20;
						_average = _average < 0 ? 0 : _average;
						_average = _average > 255 ? 255 : _average;
						_record.SetBackgroundColor(Color.Argb(255, _average, _average, _average));

					} catch (Exception ex) {
						Console.Out.WriteLine (ex.Message);
						break;
					}
				}
				fileStream.Close ();
			}
			_audioRecord.Stop ();
			_audioRecord.Release ();
			_record.Text = "Recorded";
			_isRecording = false;

			// Finished.
		}

		public async Task StartAsync ()
		{
			await StartRecordAsync ();
		}

		public void Stop ()
		{
			_endRecording = true;
			Thread.Sleep (500); // Give it time to drop out.
		}


		// On exit
		void Dispose() {
			Stop();
			_record.Click -= _recordButton_Click;
		}
	}
}

