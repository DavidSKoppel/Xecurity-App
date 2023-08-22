using System.Text.Json;
using System.Text;
using Xecurity_App.Model;

namespace Xecurity_App.View;

public partial class ExtendKeyCardPage : ContentPage
{
    private HttpClient _client;

    KeyCard keyCard { get; set; }
	public ExtendKeyCardPage(KeyCard keyData)
	{
		InitializeComponent();
        keyCard = keyData;
        labelPassword.Text = keyCard.password.ToString();
        datepicker.Date = keyCard.expDate;
		switchModule.IsToggled = keyCard.active;
    }

    private async void Put_Button(object sender, EventArgs e)
    {
        _client = new HttpClient();
        var chipid = keyCard.id;

        var putData = new KeyCard() { active = switchModule.IsToggled, password = keyCard.password, id = chipid, expDate = datepicker.Date, userId = keyCard.userId};

        var json = JsonSerializer.Serialize(putData);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var url = "http://192.168.1.122:7032/api/KeyCard/" + keyCard.id;

        var response = await _client.PutAsync(url, data);

        string result = response.Content.ReadAsStringAsync().Result;

        if (response.IsSuccessStatusCode)
        {
            await Navigation.PopModalAsync();

        }
    }
}