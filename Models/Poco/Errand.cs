using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Crime.Models
{
  public class Errand
  {
    public int ErrandId { get; set; }
    public string RefNumber { get; set; }

    [Display(Name = "Var har brottet skett någonstans?")]
    [Required(ErrorMessage = "Vänligen fyll i plats.")]
    public string Place { get; set; }

    [Display(Name = "Vilken typ av brott?")]
    [Required(ErrorMessage = "Vänligen fyll i vilken typ av brott som begåtts.")]
    public string TypeOfCrime { get; set; }

    [Required(ErrorMessage = "Vänligen fyll i observeringsdatumet.")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy - MM - dd}")]
    [Display(Name = "När skedde brottet? (YYYY-MM-DD)")]
    [UIHint("Date")]
    public DateTime DateOfObservation { get; set; }

    [Display(Name = "Beskriv din observation \n(ex. namn på misstänkt person):")]
    public string Observation { get; set; }

    public string InvestigatorInfo { get; set; }
    public string InvestigatorAction { get; set; }

    [Display(Name = "Ditt namn (för- och efternamn):")]
    [Required(ErrorMessage = "Vänligen fyll i ditt namn.")]
    public string InformerName { get; set; }

    [Display(Name = "Din telefon:")]
    [RegularExpression(pattern: @"^[0]{1}[0-9]{1,3}-[0-9]{5,9}$", ErrorMessage = "Formatet för mobilnummer ska vara 0xxx-xxxxxx")]
    [Required(ErrorMessage = "Vänligen fyll i ditt telefonnummer.")]
    public string InformerPhone { get; set; }

    public string StatusId { get; set; }

    public string DepartmentId { get; set; }
    public string EmployeeId { get; set; }

    public ICollection<Sample> Samples { get; set; }
    public ICollection<Picture> Pictures { get; set; }

  }
}
