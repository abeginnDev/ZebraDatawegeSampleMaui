using Microsoft.Maui.Controls;
using scannerwerWithIntent.model;

namespace scannerwerWithIntent;

public partial class MainPage : ContentPage
{
    Helpermodel helpermodel = Helpermodel.GetInstance();
    public MainPage()
	{
		InitializeComponent();

        this.BindingContext = helpermodel;
        ScannerText.Text = helpermodel.ScannedText;
    }

    private void OnCounterClicked(object sender, EventArgs e)
	{
        ScannerText.Text = helpermodel.ScannedText;
	}
}

