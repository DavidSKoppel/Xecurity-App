using System.Text;
using System;
using System.Text.Json;
using System.Diagnostics;
using Xecurity_App.Model;

namespace Xecurity_App.View;

public partial class LoginPage : ContentPage
{
    HttpClient _client;
    JsonSerializerOptions _serializerOptions;

    public LoginPage()
	{
		InitializeComponent();
	}

    private async void OnCounterClicked(object sender, EventArgs e)
    {
        _client = new HttpClient();
        var username = UserEntry.Text;
        var password = PasswordEntry.Text;

        var soundData = new PostUserLogin() { username = username, password = password };

        var json = JsonSerializer.Serialize(soundData);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var url = "http://192.168.1.122:7032/api/Auth/login";

        //var response = await _client.PostAsync(url, data);

        //string result = response.Content.ReadAsStringAsync().Result;

        //if(response.IsSuccessStatusCode)
        //{
        //}
        App.Current.MainPage = new NavigationPage(new MainMenuPage());

        //await Application.Current.MainPage.DisplayAlert("Sent", result, "ok");

    } 
}

