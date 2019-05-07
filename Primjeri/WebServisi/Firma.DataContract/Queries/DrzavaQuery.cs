﻿using CommandQueryCore;
using Firma.DataContract.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Firma.DataContract.Queries
{
  public class DrzavaQuery : IQuery<IEnumerable<DrzavaDto>>
  {
    public string SearchText { get; set; }
    public int From { get; set; }
    public int Count { get; set; }
    public SortInfo Sort { get; set; }   
  }
  
}
