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
using GestionLaboIot.Models;

namespace GestionLaboIot.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RecapScan : ContentPage
	{
		ZXingScannerPage scanPage;
		Emprunt emprunt = new Emprunt();
		public RecapScan ()
		{

            if (Application.Current.Properties.ContainsKey("token"))
            {
                InitializeComponent();				
				String token = Application.Current.Properties["token"].ToString();
                //String itemId = "5a968bb13478ba1b8a3e66a0";
                button_Valid.Clicked += Button_Valid_ClickedAsync;
                button_Retour.Clicked += Button_Retour_ClickedAsync;
                stepper.ValueChanged += OnStepperValueChanged;				
				button_LogOut.Clicked += Button_LogOut_ClickedAsync;
				label_nomObject.Text = StudentChoice.item.Nom;
				label_categObject.Text = StudentChoice.item.Categorie.Nom;
				label_sousCateg.Text = StudentChoice.item.SousCategorie.Nom;
				if (Application.Current.Properties["isEmprunt"].ToString() == "true")
				{
					stepper.Maximum = Convert.ToInt32(StudentChoice.item.Quantite);
					if (!StudentChoice.newEmprunt)
					{
						DisplayAlert("Attention", "Vous en avez déjà emprunter" + StudentChoice.emprunt.quantite , "OK"); 
						stepper.Minimum = Convert.ToInt32(StudentChoice.emprunt.quantite);
					}
				}
				else
				{
					stepper.Maximum = Convert.ToInt32(StudentChoice.emprunt.quantite);
					stepper.Minimum = 1;
					picker_etatObject.SelectedItem = StudentChoice.emprunt.etat;
				}
				//emprunt.item = Application.Current.Properties["token"].ToString();	
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

		private async void Button_Retour_ClickedAsync(object sender, EventArgs e)
		{
			await Navigation.PopModalAsync();
		}
		private async void Button_Valid_ClickedAsync(object sender, EventArgs e)
		{
			bool nouveauScan = false;
			bool flag = false;

			if(StudentChoice.newEmprunt)
			{
				var client = new RestClient("http://51.254.117.45:3000");
				var request = new RestRequest("emprunts/create", Method.PUT);

				request.AddHeader("x-access-token", Application.Current.Properties["token"].ToString());
				request.AddParameter("item", Application.Current.Properties["itemId"].ToString());
				request.AddParameter("user_mail", Application.Current.Properties["user_mail"].ToString());
				request.AddParameter("etat", picker_etatObject.SelectedItem.ToString());
				request.AddParameter("quantite", label_numberObjectSelect.Text);
				IRestResponse response = client.Execute(request);
				emprunt = JsonConvert.DeserializeObject<Emprunt>(response.Content);

				if(emprunt._id != null && emprunt._id.Length > 1)
				{
					flag = true;					
				}
			}
			else
			{
				var client = new RestClient("http://51.254.117.45:3000");
				var request = new RestRequest("emprunts/{id}", Method.POST);

				request.AddHeader("x-access-token", Application.Current.Properties["token"].ToString());
				request.AddUrlSegment("id", StudentChoice.emprunt._id);
				request.AddParameter("isEmprunt", Application.Current.Properties["isEmprunt"].ToString());
				request.AddParameter("etat", picker_etatObject.SelectedItem.ToString());
				request.AddParameter("quantite", label_numberObjectSelect.Text);
				IRestResponse response = client.Execute(request);
				
				if (response.IsSuccessful)
				{
					flag = true;
				}
			}

			if (flag)
			{
				nouveauScan = await DisplayAlert("Envoyé", "Données envoyées. Voulez-vous scanner un autre objet ?", "Oui", "Non");
				if (nouveauScan)
				{
					//scanPage = new ZXingScannerPage();
					//scanPage.OnScanResult += (result) => {
					//	scanPage.IsScanning = false;

					//	Device.BeginInvokeOnMainThread(() => {
					//		Navigation.PopModalAsync();
					//		Navigation.PushModalAsync(new RecapScan());
					//	});
					//};
					//await Navigation.PushModalAsync(scanPage);
					await Navigation.PushModalAsync(new StudentChoice());
				}
				else
				{
					await Navigation.PushModalAsync(new StudentMail());
				}
			}
			else
			{
				await DisplayAlert("Attention", "Une erreur est survenue lors de l'envoie des données", "OK");
			}			
		}
	}
}
