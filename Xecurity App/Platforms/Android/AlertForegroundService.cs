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
        List<DangerTemps> exempt23Danger26 = new List<DangerTemps>();
        List<DangerTemps> exemptTemperatures = new List<DangerTemps>();
        List<DangerTemps> exemptHumidity = new List<DangerTemps>();
        HttpClient _client;
        private string NOTIFICATION_CHANNEL_ID = "1000";
        private int NOTIFICATION_ID = 1; 
        private string NOTIFICATION_CHANNEL_NAME = "notification";

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            startForegroundService();
            return StartCommandResult.NotSticky;
        }

        private void startForegroundService()
        {
            var notifcationManager = GetSystemService(Context.NotificationService) as NotificationManager;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                //createNotificationChannel(notifcationManager);
            }

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
                    GetDangerousTemperaturesFromTheLast24Hours();
                    GetDangerousHumiditiesFromTheLast24Hours();
                    GetTemperaturesFromTheLastHour();
                    await Task.Delay(60000);
                }
            });
        }

        private void createNotificationChannel(NotificationManager notificationMnaManager)
        {
            var channel = new NotificationChannel(NOTIFICATION_CHANNEL_ID, NOTIFICATION_CHANNEL_NAME,
            NotificationImportance.Low);
            notificationMnaManager.CreateNotificationChannel(channel);
        }

        private async void GetDangerousHumiditiesFromTheLast24Hours()
        {
            _client = new HttpClient();

            var url = "http://192.168.1.122:7032/api/temperature/GetDangerousHumiditiesFromTheLast24Hours";

            var response = await _client.GetAsync(url);

            string result = response.Content.ReadAsStringAsync().Result;

            if (response.IsSuccessStatusCode)
            {
                List<DangerTemps> readings = JsonSerializer.Deserialize<List<DangerTemps>>(result);
                foreach (var reading in readings)
                {
                    if (!exemptHumidity.Where(i => i.Id == reading.Id).Any())
                    {
                        if (reading.Humidity > 55)
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
                        else if (reading.Humidity < 45)
                        {
                            var request = new NotificationRequest
                            {
                                NotificationId = 80085,
                                Title = "Advarsel",
                                Subtitle = "Serverrumnavn",
                                Description = "For lav luftfugtighed i serverrum målt klokken " + reading.DateUploaded.Hour,
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
                List<DangerTemps> readings = JsonSerializer.Deserialize<List<DangerTemps>>(result);
                for (int f = readings.Count - 1; f >= 0; f--)
                {
                    if (exemptTemperatures.Where(i => i.Id == readings[f].Id).Any())
                    {
                        readings.RemoveAt(f);
                    }
                }
                DangerTemps highTemp = readings.MaxBy(t => t.Temperature);
                DangerTemps lowTemp = readings.MinBy(t => t.Temperature);
                if (highTemp.Temperature >= lowTemp.Temperature + 1.5)
                {
                    var request = new NotificationRequest
                    {
                        NotificationId = 8008135,
                        Title = "Advarsel",
                        Subtitle = "Serverrumnavn",
                        Description = "Temperaturen svinger meget i serverrum",
                        BadgeNumber = 1,
                        CategoryType = NotificationCategoryType.Alarm
                    };
                    await LocalNotificationCenter.Current.Show(request);
                    exemptTemperatures.Add(highTemp);
                    exemptTemperatures.Add(lowTemp);
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
                List<DangerTemps> readings = JsonSerializer.Deserialize<List<DangerTemps>>(result);
                foreach (var reading in readings)
                {
                    if (!exempt23Danger26.Where(i => i.Id == reading.Id).Any())
                    {
                        if (reading.Temperature > 26)
                        {
                            var request = new NotificationRequest
                            {
                                NotificationId = 800,
                                Title = "Advarsel",
                                Subtitle = "Serverrumnavn",
                                Description = "For høje temperature i serverrum",
                                BadgeNumber = 1,
                                CategoryType = NotificationCategoryType.Alarm
                            };
                            await LocalNotificationCenter.Current.Show(request);
                            exempt23Danger26.Add(reading);
                        }
                        else if (reading.Temperature < 23)
                        {
                            var request = new NotificationRequest
                            {
                                NotificationId = 800800,
                                Title = "Advarsel",
                                Subtitle = "Serverrumnavn",
                                Description = "For lave temperature i serverrum målt klokken " + reading.DateUploaded.Hour,
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