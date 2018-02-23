using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GestionLaboIot.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StudentMail : ContentPage
	{
		public StudentMail ()
		{
			InitializeComponent ();
			button_ValidMail.Clicked += Button_ValidMail_ClickedAsync;
		}

		private async void Button_ValidMail_ClickedAsync(object sender, EventArgs e)
		{
			await Navigation.PushModalAsync(new StudentChoice());
		}
	}
}