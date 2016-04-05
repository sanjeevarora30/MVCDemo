using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class StateModel
    {
        public int id { get; set; }

        public string StateName { get; set; }
    }
}