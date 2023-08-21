using System.Collections.ObjectModel;
using System.Text.Json;
using Xecurity_App.Model;

namespace Xecurity_App.View;

public partial class CreateLeasePage :ContentPage
{
    HttpClient _client;
	public ObservableCollection<KeyCard> KeyCards { get; set; }
	public CreateLeasePage()
    {
        KeyCards = new ObservableCollection<KeyCard>();
		InitializeComponent();

        KeyCards.Add(new KeyCard{ id = 1, active = true, expDate = DateTime.Now, password = "1234", userId = 1});
        Task.Run(async () =>
		{
            _client = new HttpClient();

            var url = "http://192.168.1.122:7032/api/keycard";

            var response = await _client.GetAsync(url);

            string result = response.Content.ReadAsStringAsync().Result;

            if (response.IsSuccessStatusCode)
            {
                List<KeyCard> nfc_chips = JsonSerializer.Deserialize<List<KeyCard>>(result);
                foreach (var chip in nfc_chips)
                {
                    KeyCards.Add(chip);
                }
            }
        });
        BindingContext = this;
    }

    private async void ListView_KeyCardSelected(object sender, SelectedItemChangedEventArgs e)
    {
        KeyCard keyCard = (KeyCard)e.SelectedItem;
        await Navigation.PushModalAsync(new ExtendKeyCardPage(keyCard));
    }
}