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
    bool isbusy;

    public LoginPage()
	{
		InitializeComponent();
	}

    private async void OnCounterClicked(object sender, EventArgs e)
    {
        try
        {
            _client = new HttpClient();
            var username = UserEntry.Text;
            var password = PasswordEntry.Text;

            var loginData = new PostUserLogin() { username = username, password = password };

            var json = JsonSerializer.Serialize(loginData);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var url = "http://192.168.1.122:7032/api/Auth/login";

            isbusy = true;
            var response = await _client.PostAsync(url, data);
            isbusy = false;

            string result = response.Content.ReadAsStringAsync().Result;

            if(response.IsSuccessStatusCode)
            {
                App.Current.MainPage = new NavigationPage(new MainMenuPage());
            } 
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Wrong password or username", "ok");
            }
        } catch (Exception ex)
        {

        }
    }
    private void isLoading() 
    {

    }
}

