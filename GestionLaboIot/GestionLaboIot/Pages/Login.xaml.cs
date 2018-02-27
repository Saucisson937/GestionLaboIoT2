using GestionLaboIot.Pages;
using System;
using RestSharp;
using Newtonsoft.Json;
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

            login.Clicked += (sender, e) => {
                Authenticate(email.Text, password.Text);
            };
		}

        public class LoginToken
        {
            public bool success { get; set; }
            //public string message { get; set; }
            public string token { get; set; }
        }

        public void Authenticate(string email, string password)
        {
			if (!(String.IsNullOrEmpty(email) && String.IsNullOrEmpty(password)))
			{
				var client = new RestClient("http://localhost:3000/");
				var request = new RestRequest("users/authenticate", Method.POST);
				request.AddParameter("email", email);
				request.AddParameter("password", password);
				request.AddParameter("grant_type", "password");
				request.AddParameter("scope", "openid");

				IRestResponse response = client.Execute(request);
				Console.WriteLine(request);
				LoginToken loginToken = JsonConvert.DeserializeObject<LoginToken>(response.Content);

				if (loginToken.token != null)
				{
					Application.Current.Properties["success"] = loginToken.success;
					Application.Current.Properties["token"] = loginToken.token;
					Navigation.PushModalAsync(new StudentMail());
				}
				else
				{
					DisplayAlert("Erreur !", "Les informations saisies sont incorrects", "OK");
				};
			}
			else
			{
				DisplayAlert("Attention", "Veuillez remplir tous les champs", "OK");
			}
			Navigation.PushModalAsync(new StudentMail());
        }
	}
}