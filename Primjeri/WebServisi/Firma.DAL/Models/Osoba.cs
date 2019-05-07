using System;
using System.Collections.Generic;

namespace Firma.DAL.Models
{
    public partial class Osoba
    {
        public int IdOsobe { get; set; }
        public string PrezimeOsobe { get; set; }
        public string ImeOsobe { get; set; }

        public Partner IdOsobeNavigation { get; set; }
    }
}
