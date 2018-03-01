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

            // on vide le token quand on est sur la page de login
            Application.Current.Properties.Remove("token");

            login.Clicked += (sender, e) => {
                Console.WriteLine(email.Text);
                Console.WriteLine(password.Text);
                Authenticate(email.Text, password.Text);
            };
		}

        public class LoginToken
        {
            public string token { get; set; }
        }

        public void Authenticate(string email, string password)
        {

            if (!String.IsNullOrEmpty(email.Trim()) && !String.IsNullOrEmpty(password) &&
                Regex.Match(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").Success )
			{
				var client = new RestClient("http://10.92.1.230:3000/");
				var request = new RestRequest("authenticate", Method.POST);
				request.AddParameter("email", email);
				request.AddParameter("password", password);
				request.AddParameter("grant_type", "password");
				request.AddParameter("scope", "openid");

				IRestResponse response = client.Execute(request);
				LoginToken loginToken = JsonConvert.DeserializeObject<LoginToken>(response.Content);


				if (loginToken.token != null)
				{
					Application.Current.Properties["token"] = loginToken.token;
                    Navigation.PushModalAsync(new StudentMail());
				} else {
					DisplayAlert("Authenfication échouée !", "Les informations saisies sont incorrects", "Fermer");
				}
			} else {
				DisplayAlert("Attention", "Veuillez saisir correctement les champs", "Fermer");
			}

        	}
	}
}
