using System;
using System.Collections.Generic;
using System.Text;

namespace GestionLaboIot.Models
{
    class Items
    {
		public int Id { get; set; }
		public string Nom { get; set; }
		public string Categorie { get; set; }
		public string SousCategorie { get; set; }
		public string Quantite { get; set; }
		public string QRCode { get; set; }
	}
}
