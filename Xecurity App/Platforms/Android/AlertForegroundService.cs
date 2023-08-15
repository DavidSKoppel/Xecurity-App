using Android.App;
using Android.Content;
using Android.OS;
using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xecurity_App.Model;

namespace Xecurity_App.Platforms.Android
{
    [Service]
    public class AlertForegroundService : Service
    {
        List<Temperature> exempt23Danger26 = new List<Temperature>();
        List<Temperature> exemptTemperatures24 = new List<Temperature>();
        List<Temperature> exemptHumidity = new List<Temperature>();
        HttpClient _client;
        private string NOTIFICATION_CHANNEL_ID = "1000";
        private int NOTIFICATION_ID = 1;

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            startForegroundService();
            return StartCommandResult.NotSticky;
        }

        private void startForegroundService()
        {
            var notification = new AndroidX.Core.App.NotificationCompat.Builder(this, NOTIFICATION_CHANNEL_ID);
            notification.SetAutoCancel(false);
            notification.SetOngoing(true);
            notification.SetSmallIcon(Resource.Mipmap.appicon);
            notification.SetContentTitle("ForegroundService");
            notification.SetContentText("Foreground Service is running");
            StartForeground(NOTIFICATION_ID, notification.Build());

            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(60000);
                    GetDangerousTemperaturesFromTheLast24Hours();
                    GetDangerousHumiditiesFromTheLast24Hours();
                    GetTemperaturesFromTheLastHour();
                }
            });
        }

        private async void GetDangerousHumiditiesFromTheLast24Hours()
        {
            _client = new HttpClient();

            var url = "http://192.168.1.122:7032/api/temperature/GetDangerousHumiditiesFromTheLast24Hours";

            var response = await _client.GetAsync(url);

            string result = response.Content.ReadAsStringAsync().Result;

            if (response.IsSuccessStatusCode)
            {
                List<Temperature> readings = JsonSerializer.Deserialize<List<Temperature>>(result);
                foreach (var reading in readings)
                {
                    if (!exemptHumidity.Where(i => i.id == reading.id).Any())
                    {
                        if (reading.humidity > 55)
                        {
                            var request = new NotificationRequest
                            {
                                NotificationId = 8008,
                                Title = "Advarsel",
                                Subtitle = "Serverrumnavn",
                                Description = "For høj luftfugtighed i serverrum",
                                BadgeNumber = 1,
                                CategoryType = NotificationCategoryType.Alarm
                            };
                            await LocalNotificationCenter.Current.Show(request);
                            exemptHumidity.Add(reading);
                        }
                        else if (reading.humidity < 45)
                        {
                            var request = new NotificationRequest
                            {
                                NotificationId = 8008,
                                Title = "Advarsel",
                                Subtitle = "Serverrumnavn",
                                Description = "For lav luftfugtighed i serverrum målt klokken " + reading.dateUploaded.Hour,
                                BadgeNumber = 1,
                                CategoryType = NotificationCategoryType.Alarm
                            };
                            await LocalNotificationCenter.Current.Show(request);
                            exemptHumidity.Add(reading);
                        }
                    }
                }
            }
        }
        private async void GetTemperaturesFromTheLastHour()
        {
            _client = new HttpClient();

            var url = "http://192.168.1.122:7032/api/temperature/GetTemperaturesFromTheLastHour";

            var response = await _client.GetAsync(url);

            string result = response.Content.ReadAsStringAsync().Result;

            if (response.IsSuccessStatusCode)
            {
                List<Temperature> readings = JsonSerializer.Deserialize<List<Temperature>>(result);
                foreach ( var reading in readings )
                {
                    if (exempt23Danger26.Where(i => i.id == reading.id).Any())
                        readings.Remove(reading);
                }
                float min = readings.Min().temperature;
                float max = readings.Max().temperature;
                if (max >= min + 1.5)
                {
                    var request = new NotificationRequest
                    {
                        NotificationId = 8008,
                        Title = "Advarsel",
                        Subtitle = "Serverrumnavn",
                        Description = "Temperaturen svinger meget i serverrum",
                        BadgeNumber = 1,
                        CategoryType = NotificationCategoryType.Alarm
                    };
                    await LocalNotificationCenter.Current.Show(request);
                    exemptTemperatures24.Add(readings.Min());
                    exemptTemperatures24.Add(readings.Max());
                }
            }
        }

        private async void GetDangerousTemperaturesFromTheLast24Hours()
        {
            _client = new HttpClient();

            var url = "http://192.168.1.122:7032/api/temperature/GetDangerousTemperaturesFromTheLast24Hours";

            var response = await _client.GetAsync(url);

            string result = response.Content.ReadAsStringAsync().Result;

            if (response.IsSuccessStatusCode)
            {
                List<Temperature> readings = JsonSerializer.Deserialize<List<Temperature>>(result);
                foreach (var reading in readings)
                {
                    if (!exempt23Danger26.Where(i => i.id == reading.id).Any())
                    {
                        if (reading.temperature > 26)
                        {
                            var request = new NotificationRequest
                            {
                                NotificationId = 8008,
                                Title = "Advarsel",
                                Subtitle = "Serverrumnavn",
                                Description = "For høje temperature i serverrum",
                                BadgeNumber = 1,
                                CategoryType = NotificationCategoryType.Alarm
                            };
                            await LocalNotificationCenter.Current.Show(request);
                            exempt23Danger26.Add(reading);
                        }
                        else if (reading.temperature < 23)
                        {
                            var request = new NotificationRequest
                            {
                                NotificationId = 8008,
                                Title = "Advarsel",
                                Subtitle = "Serverrumnavn",
                                Description = "For lave temperature i serverrum målt klokken " + reading.dateUploaded.Hour,
                                BadgeNumber = 1,
                                CategoryType = NotificationCategoryType.Alarm
                            };
                            await LocalNotificationCenter.Current.Show(request);
                            exempt23Danger26.Add(reading);
                        }
                    }
                }
            }
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }


    }
}