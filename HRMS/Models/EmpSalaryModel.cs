using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class EmpSalaryModel
    {
        [Display(Name = "SalaryId")]
        public int id { get; set; }

        [Display(Name = "EmployeeId")]
        public int EmpId { get; set; }

        [Required(ErrorMessage = "Salary is required.")]
        [RegularExpression("^[1-9][0-9]*$", ErrorMessage = "Salary is not valid")]
        public int Salary { get; set; }

        [Required(ErrorMessage = "Month is required.")]
        public int SalaryMonth { get; set; }

        [Required(ErrorMessage = "Year is required.")]
        public int SalaryYear { get; set; }
    }
}