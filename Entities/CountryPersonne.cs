using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesEntites
{
    public  class CountryPersonne
    {
        public   Person? Person { get; set; }
      public   Country? Country { get; set; }

     public  Dictionary<string, string>? CritereDeRecherche { get; set; }
    }
}
