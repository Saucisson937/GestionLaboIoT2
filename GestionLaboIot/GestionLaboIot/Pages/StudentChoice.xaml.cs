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
			LaunchScanPage();		
		}

		private async void Button_emprunter_ClickedAsync(object sender, EventArgs e)
		{
			 LaunchScanPage();
		}

		public async void LaunchScanPage()
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

						request.AddHeader("x-acces-token", Application.Current.Properties["token"].ToString());
						//request.AddParameter("id", result.Text);
						//request.AddQueryParameter("id", result.Text);

						//request.AddUrlSegment("id", result.Text);
						request.AddUrlSegment("id", "5a968bb13478ba1b8a3e66a0");

						//	request.AddParameter("password", password);
						//	request.AddParameter("grant_type", "password");
						//	request.AddParameter("scope", "openid");


						IRestResponse response = client.Execute(request);

						item = JsonConvert.DeserializeObject<Items>(response.Content);

						if (item._id != null && item._id.Length > 1)
						{
							Application.Current.Properties["itemId"] = item._id;
							Navigation.PushModalAsync(new RecapScan());
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