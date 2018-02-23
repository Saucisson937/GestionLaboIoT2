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
		}

		private async void Button_rendre_ClickedAsync(object sender, EventArgs e)
		{
			try
			{
				scanPage = new ZXingScannerPage();
				scanPage.OnScanResult += (result) => {
					scanPage.IsScanning = false;

					Device.BeginInvokeOnMainThread(() => {
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
			scanPage = new ZXingScannerPage();
			scanPage.OnScanResult += (result) => {
				scanPage.IsScanning = false;

				Device.BeginInvokeOnMainThread(() => {
					Navigation.PopModalAsync();
					Navigation.PushModalAsync(new RecapScan());
					//DisplayAlert("Scanned Barcode", result.Text, "OK");
				});
			};

			await Navigation.PushModalAsync(scanPage);
		}
	}
}