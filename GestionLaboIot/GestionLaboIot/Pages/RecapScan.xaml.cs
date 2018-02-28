using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;
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

            if (Application.Current.Properties.ContainsKey("token"))
            {
                InitializeComponent();
                String token = Application.Current.Properties["token"].ToString();
                String itemId = "5a968bb13478ba1b8a3e66a0";
                GetItem(token, itemId);
                button_Valid.Clicked += Button_Valid_ClickedAsync;
                button_Retour.Clicked += Button_Retour_ClickedAsync;
                stepper.ValueChanged += OnStepperValueChanged;
            }
            else
            {
                Navigation.PushModalAsync(new Login());
            }
		}

        private void OnStepperValueChanged(object sender, ValueChangedEventArgs e)
        {
            label_numberObjectSelect.Text =  e.NewValue.ToString();
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

        public class Item
        {
            public string nom { get; set; }
            public string categorie { get; set; }
            public string sousCategorie { get; set; }
            public string quantite { get; set; }
        }

        public class Emprunt
        {
            public string user_mail { get; set; }
            public string item { get; set; }
            public string dateStart { get; set; }
            public string dateEnd { get; set; }
            public string etat { get; set; }
            public string quantite { get; set; }
        }

        public void GetItem(String token, String itemId){
            var client = new RestClient("http://51.254.117.45:3000/");
            var request = new RestRequest("items/{id}", Method.GET);
            request.AddUrlSegment("id", itemId);
            request.AddParameter("x-access-token", token);
            request.AddParameter("scope", "openid");

            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            Item item = JsonConvert.DeserializeObject<Item>(response.Content);

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