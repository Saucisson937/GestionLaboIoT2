using System;
using System.Collections.Generic;
using System.Text;

namespace GestionLaboIot.Models
{
    public class Emprunt
    {
		public string _id { get; set; }
		public string user_email { get; set; }
		public string item { get; set; }
		public DateTime date_start { get; set; }
		public DateTime date_end { get; set; }
		public string etat { get; set; }
		public string quantite { get; set; }
	}
}
