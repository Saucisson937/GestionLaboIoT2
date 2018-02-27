using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.RegularExpressions;
using RestSharp;
using Newtonsoft;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GestionLaboIot.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StudentMail : ContentPage
	{
		public StudentMail ()
		{
            if (Application.Current.Properties.ContainsKey("token"))
            {

                InitializeComponent();
                button_ValidMail.Clicked += Button_ValidMail_ClickedAsync;
                button_Retour.Clicked += Button_Retour_ClickedAsync;
            }
            else
            {
                Navigation.PushModalAsync(new Login());
            }
			
		}

		private async void Button_Retour_ClickedAsync(object sender, EventArgs e)
		{
			await Navigation.PopModalAsync();

		}

		private async void Button_ValidMail_ClickedAsync(object sender, EventArgs e)
		{
			if(!(String.IsNullOrEmpty(entry_Mail.Text)))//test si pas null ou pas vide
			{
				entry_Mail.Text = entry_Mail.Text.Replace(" ", "").ToLower();// enleve les espaces blancs et met en minuscule

                if(Regex.Match(entry_Mail.Text, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").Success) //test si format mail valide
				{
					if (entry_Mail.Text.EndsWith("ynov.com")) //test si domaine = ynov.com
					{
						await Navigation.PushModalAsync(new StudentChoice());
					}
					else
					{
						await DisplayAlert("Attention", "Veuillez indiquer une adresse mail ynov", "Ok");
						entry_Mail.Text = "";
					}
				}
				else
				{
					await DisplayAlert("Attention", "Format de mail invalide", "Ok");
					entry_Mail.Text = "";
				}				
			}
			else
			{
				await DisplayAlert("Attention", "Veuillez indiquer une adresse mail ynov", "Ok");
			}
			
			
		}
	}
}