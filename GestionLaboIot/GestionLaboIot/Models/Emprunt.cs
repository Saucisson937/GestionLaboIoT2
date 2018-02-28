using System;
using System.Collections.Generic;
using System.Text;

namespace GestionLaboIot.Models
{
    public class Emprunt
    {
		public string Email { get; set; }
		public string Item { get; set; }
		public string Categorie { get; set; }
		public string SousCategorie { get; set; }
		public DateTime Date { get; set; }
		public int Quantite { get; set; }
		public string Etat { get; set; }
	}
}
