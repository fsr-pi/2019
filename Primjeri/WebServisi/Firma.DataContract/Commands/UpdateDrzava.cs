using System;
using System.Collections.Generic;
using System.Text;

namespace Firma.DataContract.Commands
{
  public class UpdateDrzava
  {
    public string OznDrzave { get; set; }

    public string NazDrzave { get; set; }

    public string Iso3drzave { get; set; }
    public int? SifDrzave { get; set; }
  }
}
