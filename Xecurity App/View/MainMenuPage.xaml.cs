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

    private void Button_Clicked(object sender, EventArgs e)
    {
#if ANDROID
        Android.Content.Intent intent = new Android.Content.Intent(Android.App.Application.Context, typeof(Platforms.Android.AlertForegroundService));
        Android.App.Application.Context.StartForegroundService(intent);
#endif
    }

    private void Extend_Lease_Button(object sender, EventArgs e)
    {
        App.Current.MainPage = new NavigationPage(new ExtendLeasePage());
    }
}