using GestionLaboIot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace GestionLaboIot.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StudentChoice : ContentPage
	{
		ZXingScannerPage scanPage;
		
		public StudentChoice()
		{
			InitializeComponent();
			button_emprunter.Clicked += Button_emprunter_ClickedAsync;
			button_rendre.Clicked += Button_rendre_ClickedAsync;
			button_Retour.Clicked += Button_Retour_ClickedAsync;
		}

		private async void Button_Retour_ClickedAsync(object sender, EventArgs e)
		{
			await Navigation.PopModalAsync();
		}

		private async void Button_rendre_ClickedAsync(object sender, EventArgs e)
		{
			try
			{
				var customOverlay = new StackLayout
				{
					HorizontalOptions = LayoutOptions.Start,
					VerticalOptions = LayoutOptions.Start
				};
				var retour = new Button
				{
					Image = "return64.png",
					BackgroundColor = Color.Transparent
				};
				var torch = new Button
				{
					Text = "Flash",
					BackgroundColor = Color.Transparent,
					FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
				};

				retour.Clicked += delegate
				{
					Navigation.PopModalAsync();
				};
				torch.Clicked += delegate {
					scanPage.ToggleTorch();
				};

				customOverlay.Children.Add(retour);
				customOverlay.Children.Add(torch);

				scanPage = new ZXingScannerPage(customOverlay: customOverlay);

				scanPage.OnScanResult += (result) =>
				{
					scanPage.IsScanning = false;

					Device.BeginInvokeOnMainThread(() =>
					{
						Navigation.PopModalAsync();
						//DisplayAlert("Scanned Barcode", result.Text, "OK");
						Navigation.PushModalAsync(new RecapScan());
					});
				};
				await Navigation.PushModalAsync(scanPage);
			}
			catch (Exception ex)
			{
				await DisplayAlert("Erreur", ex.ToString(), "OK");
			}			
		}

		private async void Button_emprunter_ClickedAsync(object sender, EventArgs e)
		{
			try
			{
				var customOverlay = new StackLayout
				{
					HorizontalOptions = LayoutOptions.Start,
					VerticalOptions = LayoutOptions.Start
				};
				var retour = new Button
				{
					Image = "return64.png",
					BackgroundColor = Color.Transparent
				};
				var torch = new Button
				{
					Text = "Flash",
					BackgroundColor = Color.Transparent,
					FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
				};

				retour.Clicked += delegate
				{
					Navigation.PopModalAsync();
				};
				torch.Clicked += delegate {
					scanPage.ToggleTorch();
				};

				customOverlay.Children.Add(retour);
				customOverlay.Children.Add(torch);

				scanPage = new ZXingScannerPage(customOverlay: customOverlay);

				scanPage.OnScanResult += (result) =>
				{
					scanPage.IsScanning = false;

					Device.BeginInvokeOnMainThread(() =>
					{
						Navigation.PopModalAsync();
						//DisplayAlert("Scanned Barcode", result.Text, "OK");
						Navigation.PushModalAsync(new RecapScan());
					});
				};
				await Navigation.PushModalAsync(scanPage);
			}
			catch (Exception ex)
			{
				await DisplayAlert("Erreur", ex.ToString(), "OK");
			}
		}
	}
}