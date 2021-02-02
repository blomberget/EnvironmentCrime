﻿using System;
using System.Threading.Tasks;
using Crime.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Crime.Models
{
  public class MyErrand
  {
    public DateTime DateOfObservation { get; set; }
    public int ErrandId { get; set; }
    public string RefNumber { get; set; }
    public string TypeOfCrime { get; set; }
    public string StatusName { get; set; }
    public string DepartmentName { get; set; }
    public string EmployeeName { get; set; }
  }
}