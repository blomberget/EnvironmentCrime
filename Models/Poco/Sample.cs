﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Crime.Models
{
  public class Sample
  {
    public int SampleId { get; set; }
    public string SampleName { get; set; }

    public int ErrandId { get; set; }
  }
}