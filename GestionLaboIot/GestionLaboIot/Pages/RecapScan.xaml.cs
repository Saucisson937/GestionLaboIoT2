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
				}
				else
				{
					stepper.Maximum = Convert.ToInt32(StudentChoice.emprunt.quantite);
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
			var nouveauScan = await DisplayAlert("Envoyé", "Données envoyées. Voulez-vous scanner un autre objet ?","Oui","Non");


			//CODE ENVOIE DONNEE


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
		//public class Emprunt
		//{
		//	public string user_mail { get; set; }
		//	public string item { get; set; }
		//	public string dateStart { get; set; }
		//	public string dateEnd { get; set; }
		//	public string etat { get; set; }
		//	public string quantite { get; set; }
		//}

	}
}
