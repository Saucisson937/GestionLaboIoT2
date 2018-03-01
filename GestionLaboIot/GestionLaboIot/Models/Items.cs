using System;
using System.Collections.Generic;
using System.Text;

namespace GestionLaboIot.Models
{
    public class Items
    {
		public string _id { get; set; }
		public string Nom { get; set; }
		
		public Categorie Categorie { get; set; }
		public SousCategorie SousCategorie { get; set; }
		public string Quantite { get; set; }
		public DateTime Creation { get; set; }
		public DateTime Update { get; set; }

	}

	public class Categorie
	{
		public string _id { get; set; }
		public string  Nom { get; set; }
	}
	public class SousCategorie
	{
		public string _id { get; set; }
		public string Nom { get; set; }
	}
}
