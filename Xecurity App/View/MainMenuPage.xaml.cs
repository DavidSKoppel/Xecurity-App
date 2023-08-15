using Plugin.LocalNotification;

namespace Xecurity_App.View;

public partial class MainMenuPage : ContentPage
{
	public MainMenuPage()
	{
		InitializeComponent();
        Task.Run(async () =>
        {
            var request = new NotificationRequest
            {
                NotificationId = 8008,
                Title = "Alert",
                Subtitle = "Attention",
                Description = "Danger in server room",
                BadgeNumber = 42,
                CategoryType = NotificationCategoryType.Alarm,
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = DateTime.Now.AddSeconds(5),
                }
            };
            await LocalNotificationCenter.Current.Show(request);
        });
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var request = new NotificationRequest
        {
            NotificationId = 1000,
            Title = "Subscribe for me",
            Subtitle = "Hello Friends",
            Description = "Stay Tuned",
            BadgeNumber = 42,
            Schedule = new NotificationRequestSchedule
            {
                NotifyTime = DateTime.Now.AddSeconds(5),
                NotifyRepeatInterval = TimeSpan.FromDays(1)
            }
        };
        await LocalNotificationCenter.Current.Show(request);
    }
}