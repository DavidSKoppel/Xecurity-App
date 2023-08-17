using Plugin.LocalNotification;

namespace Xecurity_App.View;

public partial class MainMenuPage : ContentPage
{
	public MainMenuPage()
	{
		InitializeComponent();
        Task.Run(async () =>
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

    private async void DisableChip_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new DisableNFCPage());
    }

    private async void ViewImage_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new PictureModePage());
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

    private void Button_Clicked(object sender, EventArgs e)
    {

    }
}