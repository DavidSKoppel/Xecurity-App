using System.Collections.ObjectModel;
using System.Text.Json;
using Xecurity_App.Model;

namespace Xecurity_App.View;

public partial class CheckInHistoryPage : ContentPage
{
    HttpClient _client;
    public ObservableCollection<KeyCardHistoryDto> KeyCardHistories { get; set; }
    public CheckInHistoryPage()
    {
        KeyCardHistories = new ObservableCollection<KeyCardHistoryDto>();
        InitializeComponent();
        Task.Run(async () =>
        {
            _client = new HttpClient();

            var url = "http://192.168.1.122:7032/api/keycardhistory";

            var response = await _client.GetAsync(url);

            string result = response.Content.ReadAsStringAsync().Result;

            if (response.IsSuccessStatusCode)
            {
                List<KeyCardHistoryDto> nfc_chips = JsonSerializer.Deserialize<List<KeyCardHistoryDto>>(result);
                foreach (var chip in nfc_chips)
                {
                    chip.image = "http://192.168.1.122:5500" + chip.image;
                    KeyCardHistories.Add(chip);
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