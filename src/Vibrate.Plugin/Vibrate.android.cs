using System;

using Plugin.Vibrate.Abstractions;
using Android.OS;
using Android.Content;
using Android.App;

[assembly: UsesPermission(Android.Manifest.Permission.Vibrate)]
namespace Plugin.Vibrate
{
    /// <summary>
    /// Vibration Implentation on Android 
    /// </summary>
    public class Vibrate : IVibrate
    {
		/// <summary>
		/// Gets if device can vibrate
		/// </summary>
		public bool CanVibrate
		{
			get
			{
				if ((int)Build.VERSION.SdkInt >= 11)
				{

					using (var v = (Vibrator)Android.App.Application.Context.GetSystemService(Context.VibratorService))
						return v.HasVibrator;
				}

				return true;
			}
		}

		/// <summary>
		/// Vibrate the phone for specified amount of time
		/// </summary>
		/// <param name="vibrateSpan">Time span to vibrate. 500ms is default if null</param>
		public void Vibration(TimeSpan? vibrateSpan = null)
        {
            var milliseconds = vibrateSpan.HasValue ? vibrateSpan.Value.TotalMilliseconds : 500;
            using (var v = (Vibrator)Android.App.Application.Context.GetSystemService(Context.VibratorService))
            {
                if ((int)Build.VERSION.SdkInt >= 11)
                {
#if __ANDROID_11__
                    if (!v.HasVibrator)
                    {
                        Console.WriteLine("Android device does not have vibrator.");
                        return;
                    }
#endif
                }

				if (milliseconds < 0)
					milliseconds = 0;

                try
                {
                    v.Vibrate((int)milliseconds);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to vibrate Android device, ensure VIBRATE permission is set.");
                }
            }

        }
    }
}