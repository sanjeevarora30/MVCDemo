using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class EmployeeModel
    {
        [Display(Name = "EmpId")]
        public int EmpId { get; set; }

        [Required(ErrorMessage = "Employee Name is required.")]
        public string EmpName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "State is required.")]
        public string State { get; set; }

        [Required(ErrorMessage = "City is required.")]
        public string City { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "ZipCode")]
        [RegularExpression(@"^\d{6}(-\d{5})?$", ErrorMessage = "Invalid Zip")]
        public string ZipCode { get; set; }

        [Display(Name = "Deactive")]
        public bool Deactive { get; set; }

        [Display(Name = "CreatedOn")]
        public string CreatedOn { get; set; }

        [Display(Name = "ActiveStatus")]
        public string ActiveStatus { get; set; }
    }
}