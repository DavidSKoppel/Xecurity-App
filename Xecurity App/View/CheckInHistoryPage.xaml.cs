using System.Collections.ObjectModel;
using System.Text.Json;
using Xecurity_App.Model;

namespace Xecurity_App.View;

public partial class CheckInHistoryPage : ContentPage
{
    HttpClient _client;
    public List<KeyCardHistoryDto> KeyCards { get; set; }
    public ObservableCollection<KeyCardHistoryObservable> KeyCardHistories { get; set; }
    public CheckInHistoryPage()
    {
        KeyCards = new List<KeyCardHistoryDto>();
        KeyCardHistories = new ObservableCollection<KeyCardHistoryObservable>();

        InitializeComponent();

        ImageSourceConverter converter = new ImageSourceConverter();
        KeyCardHistories.Add(new KeyCardHistoryObservable
        {
            addressName = "Sømarken",
            image = new Uri("https://www.cnet.com/a/img/resize/0052392ffa339a707dcc4156ca0d9c1a7ef1abd5/hub/2021/11/03/3c2a7d79-770e-4cfa-9847-66b3901fb5d7/c09.jpg?auto=webp&fit=crop&height=900&width=1200", UriKind.Absolute),
            dateUploaded = DateTime.Now,
            id = 1,
            keyCardId = 11,
            locationName = "Ollerup",
            serverRoomName = "Xervices",
            status = "Denied",
            user = "Doge"
        });
        KeyCardHistories.Add(new KeyCardHistoryObservable
        {
            addressName = "Sømarken",
            image = new Uri("https://www.cnet.com/a/img/resize/0052392ffa339a707dcc4156ca0d9c1a7ef1abd5/hub/2021/11/03/3c2a7d79-770e-4cfa-9847-66b3901fb5d7/c09.jpg?auto=webp&fit=crop&height=900&width=1200", UriKind.Absolute),
            dateUploaded = DateTime.Now,
            id = 2,
            keyCardId = 11,
            locationName = "Ollerup",
            serverRoomName = "Xervices",
            status = "Success",
            user = "Doge"
        });
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
                    ImageSourceConverter converter = new ImageSourceConverter();
                    var HttpImage = (UriImageSource)converter.ConvertFromString("http://192.168.1.122:5500" + chip.image);
                    KeyCardHistories.Add( new KeyCardHistoryObservable 
                    {
                        addressName = chip.addressName,
                        image = HttpImage.Uri,
                        dateUploaded = chip.dateUploaded,
                        id = chip.id,
                        keyCardId = chip.keycardId,
                        locationName = chip.locationName,
                        serverRoomName = chip.serverRoomName,
                        status = chip.status,
                        user = chip.user
                    });
                }
            }
        });

        BindingContext = this;
    }

    private async void ListView_KeyCardSelected(object sender, SelectedItemChangedEventArgs e)
    {
        KeyCardHistoryObservable keyCard = (KeyCardHistoryObservable)e.SelectedItem;
        Uri uri = new Uri(keyCard.image.AbsoluteUri);
        await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);

    }
}