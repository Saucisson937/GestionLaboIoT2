using GestionLaboIot.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
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
	public partial class StudentChoice : ContentPage
	{
		ZXingScannerPage scanPage;
		public static Items item = new Items();
		public static Emprunt emprunt = new Emprunt();
		public static bool newEmprunt = false;
		public static bool isEmprunt = true;
		
		public StudentChoice()
		{
			InitializeComponent();
			button_emprunter.Clicked += Button_emprunter_ClickedAsync;
			button_rendre.Clicked += Button_rendre_ClickedAsync;
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

		private async void Button_rendre_ClickedAsync(object sender, EventArgs e)
		{
			isEmprunt = false;
			LaunchScanPage(isEmprunt);		
		}

		private async void Button_emprunter_ClickedAsync(object sender, EventArgs e)
		{
			isEmprunt = true;
			LaunchScanPage(isEmprunt);
		}

		public async void LaunchScanPage(bool isEmprunt)
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
						
						var client = new RestClient("http://51.254.117.45:3000/");
						var request = new RestRequest("items/{id}", Method.GET);

						request.AddHeader("x-access-token", Application.Current.Properties["token"].ToString());
						request.AddUrlSegment("id", result.Text);
						IRestResponse response = client.Execute(request);

						item = JsonConvert.DeserializeObject<Items>(response.Content);

						if (item._id != null)
						{
							Application.Current.Properties["itemId"] = item._id;
							var client_rendre = new RestClient("http://51.254.117.45:3000/");
							var request_rendre = new RestRequest("emprunts/byUserAndItem", Method.POST);

							request_rendre.AddHeader("x-acces-token", Application.Current.Properties["token"].ToString());
							request_rendre.AddParameter("user_mail", Application.Current.Properties["user_mail"].ToString());
							request_rendre.AddParameter("item", result.Text);

							IRestResponse response_rendre = client_rendre.Execute(request_rendre);
							emprunt = JsonConvert.DeserializeObject<Emprunt>(response_rendre.Content);

							if (isEmprunt && emprunt == null)
							{
								newEmprunt = true;
							} 
							else
							{

								newEmprunt = false;
							}

							if (emprunt != null || isEmprunt)
							{
								if(isEmprunt && item.Quantite == "0")
								{
									DisplayAlert("Attention", "Nous ne l'avons plus en stock", "OK");
								}
								else
								{
									Navigation.PushModalAsync(new RecapScan());
								}
								
							}
							else
							{
								DisplayAlert("Attention", "Action impossible", "OK");
							}

						}
						else
						{
							if (isEmprunt)
							{
								DisplayAlert("Attention", "Nous ne retrouvons pas cet item", "OK");
							}
							else
							{
								DisplayAlert("Attention", "Vous ne pouvez pas rendre puisque vous n'avez rien emprunté", "OK");
							}
						}
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
