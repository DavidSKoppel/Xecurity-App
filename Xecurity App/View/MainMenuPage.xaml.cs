using Plugin.LocalNotification;

namespace Xecurity_App.View;

public partial class MainMenuPage : ContentPage
{
	public MainMenuPage()
	{
		InitializeComponent();
        Task.Run( () =>
        {
#if ANDROID
            Android.Content.Intent intent = new Android.Content.Intent(Android.App.Application.Context, typeof(Platforms.Android.AlertForegroundService));
            Android.App.Application.Context.StartForegroundService(intent);
#endif
        });
    }

    private async void CreateUpdateChip_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new CreateLeasePage());
    }

    private async void ViewLivestream_Clicked(object sender, EventArgs e)
    {
        Uri uri = new Uri("http://192.168.1.116:8081");
        await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
    }

    private async void CheckHistory_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new CheckInHistoryPage());
    }
}