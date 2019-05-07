using System;
using System.Collections.Generic;
using System.Text;

namespace Firma.DataContract.Commands
{
  public class DeleteDrzava
  {
    public DeleteDrzava(string oznDrzave)
    {
      OznDrzave = oznDrzave;
    }
   
    public string OznDrzave { get; set; }
  }
}
