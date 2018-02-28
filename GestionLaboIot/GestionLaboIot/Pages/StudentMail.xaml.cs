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
                button_ValidMail.IsVisible = false;
                entry_Mail.TextChanged += EditorChanged;
                button_ValidMail.Clicked += Button_ValidMail_ClickedAsync;
				button_LogOut.Clicked += Button_LogOut_ClickedAsync;
            }
            else
            {
                Navigation.PushModalAsync(new Login());
            }
			
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

        private void EditorChanged(object sender, EventArgs e){

            String email = entry_Mail.Text.Trim();
            if(!(String.IsNullOrEmpty(email)) // si pas null ou pas vide
               && Regex.Match(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").Success // si c'est au format mail
               && email.EndsWith("ynov.com")) // si c'est une adresse ynov
            {
                button_ValidMail.IsVisible = true;
                              
            }
            else
            {
                button_ValidMail.IsVisible = false;
            }           
        }

		private async void Button_Retour_ClickedAsync(object sender, EventArgs e)
		{
            Application.Current.Properties["userEmail"] = entry_Mail.Text;
			await Navigation.PopModalAsync();
		}

		private async void Button_ValidMail_ClickedAsync(object sender, EventArgs e)
		{
            await Navigation.PushModalAsync(new StudentChoice());
		}
	}
}