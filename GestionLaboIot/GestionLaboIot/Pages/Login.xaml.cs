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


            //var email = new Entry
            //{
            //    Placeholder = "Email"
            //};

            //var password = new Entry
            //{
            //    Placeholder = "Mot de passe",
            //    IsPassword = true
            //};

            //var login = new Button
            //{
            //    Text = "Connexion"
            //};


            login.Clicked += (sender, e) => {
                Authenticate(email.Text, password.Text);
            };

            //Content = new StackLayout
            //{
            //    Padding = 30,
            //    Spacing = 10,
            //    Children = { email, password, login }
            //};

		}

        public class LoginToken
        {
            public bool success { get; set; }
            //public string message { get; set; }
            public string token { get; set; }
        }

        public void Authenticate(string email, string password)
        {
            var client = new RestClient("http://localhost:3000/");
            var request = new RestRequest("users/authenticate", Method.POST);
            request.AddParameter("email", email);
            request.AddParameter("password", password);
            request.AddParameter("grant_type", "password");
            request.AddParameter("scope", "openid");


            IRestResponse response = client.Execute(request);
            Console.WriteLine(request);
            LoginToken token = JsonConvert.DeserializeObject<LoginToken>(response.Content);

            if (token.token != null)
            {
                Application.Current.Properties["success"] = token.success;
                Application.Current.Properties["token"] = token.token;
                Navigation.PushModalAsync(new StudentMail());
            }
            else
            {
                DisplayAlert("Erreur !", "Les informations saisies sont incorrects", "OK");
            };
        }



	}
}