using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Crime.Models
{
  public class Sequence
  {
    public int Id { get; set; }
    public int CurrentValue { get; set; }
  }
}
