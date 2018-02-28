using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace GestionLaboIot.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RecapScan : ContentPage
	{
		ZXingScannerPage scanPage;
		public RecapScan ()
		{
			InitializeComponent ();
			button_Valid.Clicked += Button_Valid_ClickedAsync;
			button_minus.Clicked += Button_minus_Clicked;
			button_plus.Clicked += Button_plus_Clicked;
			button_Retour.Clicked += Button_Retour_ClickedAsync;
			button_LogOut.Clicked += Button_LogOut_ClickedAsync;
		}

		private async void Button_LogOut_ClickedAsync(object sender, EventArgs e)
		{
			var existingPages = Navigation.NavigationStack.ToList();
			foreach (var page in existingPages)
			{
				Navigation.RemovePage(page);
			}
			await Navigation.PushModalAsync(new Login());
		}

		private async void Button_Retour_ClickedAsync(object sender, EventArgs e)
		{
			await Navigation.PopModalAsync();
		}

		private void Button_plus_Clicked(object sender, EventArgs e)
		{
			//Faire +1 sur le label quantité (et donc -1 sur la quantité restante)
		}

		private void Button_minus_Clicked(object sender, EventArgs e)
		{
			//Faire -1 sur le label quantité (et donc +1 sur la quantité restante)
		}

		private async void Button_Valid_ClickedAsync(object sender, EventArgs e)
		{
			var nouveauScan = await DisplayAlert("Envoyé", "Données envoyées. Voulez-vous scanner un autre objet ?","Oui","Non");
			if (nouveauScan)
			{
				scanPage = new ZXingScannerPage();
				scanPage.OnScanResult += (result) => {
					scanPage.IsScanning = false;

					Device.BeginInvokeOnMainThread(() => {
						Navigation.PopModalAsync();
						Navigation.PushModalAsync(new RecapScan());
					});
				};
				await Navigation.PushModalAsync(scanPage);
			}
			else
			{
				await Navigation.PushModalAsync(new StudentMail());
			}
			
		}
	}
}