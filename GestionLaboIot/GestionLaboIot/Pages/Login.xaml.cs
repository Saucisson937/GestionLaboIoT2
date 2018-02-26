using GestionLaboIot.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.RegularExpressions;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GestionLaboIot
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Login : ContentPage
	{
		public Login ()
		{
			InitializeComponent ();
			button_Valid.Clicked += Button_Valid_ClickedAsync;
		}

		private async void Button_Valid_ClickedAsync(object sender, EventArgs e)
		{
			if(!(String.IsNullOrEmpty(entry_Login.Text) || String.IsNullOrEmpty(entry_password.Text)))
			{
				await Navigation.PushModalAsync(new StudentMail());
			}
			else
			{
				await DisplayAlert("Attention", "Veuillez entrez vos identifiants de connexion","Ok");
			}
			
		}
	}
}