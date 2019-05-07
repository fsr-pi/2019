using System.ComponentModel.DataAnnotations;

namespace Firma.WebApi.Models
{
  /// <summary>
  /// Data transfer object (DTO) za državu. Koristi se kao ulazno/izlazni model za web api s državama
  /// </summary>
  public class Drzava
  {

    /// <summary>
    /// Oznaka države
    /// </summary>
    [Required(ErrorMessage = "Oznaka države je obvezno polje")]
    public string OznDrzave { get; set; }

    /// <summary>
    /// Naziv države
    /// </summary>
    [Required(ErrorMessage = "Naziv države je obvezno polje")]
    public string NazDrzave { get; set; }

    /// <summary>
    /// ISO 3 oznaka države
    /// </summary>
    [MaxLength(3, ErrorMessage = "ISO3 oznaka može sadržavati maksimalno 3 slova")]
    public string Iso3drzave { get; set; }

    /// <summary>
    /// Šifra države
    /// </summary>
    public int? SifDrzave { get; set; }

    public override string ToString()
    {
      return $"{OznDrzave} - {NazDrzave} - {Iso3drzave}/{SifDrzave}";
    }
  }
}
