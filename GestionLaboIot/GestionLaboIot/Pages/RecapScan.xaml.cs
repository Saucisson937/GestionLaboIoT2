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
	public partial class RecapScan : ContentPage
	{
		public RecapScan ()
		{
			InitializeComponent ();
			button_Valid.Clicked += Button_Valid_Clicked;
			button_minus.Clicked += Button_minus_Clicked;
			button_plus.Clicked += Button_plus_Clicked;
		}

		private void Button_plus_Clicked(object sender, EventArgs e)
		{
			throw new NotImplementedException();
		}

		private void Button_minus_Clicked(object sender, EventArgs e)
		{
			throw new NotImplementedException();
		}

		private void Button_Valid_Clicked1(object sender, EventArgs e)
		{
			throw new NotImplementedException();
		}

		private void Button_Valid_Clicked(object sender, EventArgs e)
		{
			throw new NotImplementedException();
		}
	}
}