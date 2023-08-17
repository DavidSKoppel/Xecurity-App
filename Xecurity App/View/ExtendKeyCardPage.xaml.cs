using Xecurity_App.Model;

namespace Xecurity_App.View;

public partial class ExtendKeyCardPage : ContentPage
{
    KeyCard keyCard { get; set; }
	public ExtendKeyCardPage(KeyCard keyData)
	{
		InitializeComponent();
		keyCard = keyData;

		labelId.SetBinding(Label.TextProperty, new Binding("value", source: keyCard.id));
		labelPassword.SetBinding(Label.TextProperty, new Binding("value", source: keyCard.password));
		labelExpDate.SetBinding(Label.TextProperty, new Binding("value", source: keyCard.expDate));
		labelActive.SetBinding(Label.TextProperty, new Binding("value", source: keyCard.active));
    }
}