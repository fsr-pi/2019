using System.Runtime.Serialization;

namespace Firma.Mvc.Controllers.AutoComplete
{
  [DataContract]
  public class Artikl
  {
    [DataMember(Name = "label")]
    public string Label { get; set; }
    [DataMember(Name = "id")]
    public int Id { get; set; }
    [DataMember(Name = "cijena")]
    public decimal Cijena { get; set; }
    public Artikl() { }
    public Artikl(int id, string label, decimal cijena)
    {
      Id = id;
      Label = label;
      Cijena = cijena;
    }
  }


}
