using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRMS.Repository;
using HRMS.Models;

namespace HRMS.Controllers
{
    public class IndexController : Controller
    {
        //
        // GET: /Index/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAllEmpDetails()
        {
            EmpRepository empRo = new EmpRepository();
            ModelState.Clear();

            bool SearchCriteria = false;
            string SearchEmployee = Request.Form["txtEmployee"];
            string Status = Request.Form["cmbStatus"];

            if (!string.IsNullOrEmpty(SearchEmployee))
            {
                if (SearchEmployee.Trim() != "")
                {
                    SearchCriteria = true;
                }
            }

            if (!string.IsNullOrEmpty(Status))
            {
                SearchCriteria = true;
            }
            else
            {
                Status = "0";
                SearchCriteria = true;
            }

            ViewBag.Title = "HRMS - Employee List";

            if (SearchCriteria)
            {
                return View(empRo.GetSearchedEmployees(SearchEmployee, Status));
            }
            else
            {
                return View(empRo.GetAllEmployees());
            }
        }

        public ActionResult AddEmployee()
        {
            StateRepository sr = new StateRepository();
            var sel = new SelectList(sr.GetAllStates(), "id", "StateName");
            ViewBag.Title = "HRMS - Add Employee";
            ViewBag.StateList = sel.ToList();
            return View();
        }

        [HttpPost]
        [ActionName("AddEmployee")]
        public ActionResult AddEmployee1()
        {
            EmployeeModel Emp = new EmployeeModel();
            Emp.EmpId = 0;
            Emp.EmpName = Request.Form["EmpName"];
            Emp.Email = Request.Form["Email"];
            Emp.State = Request.Form["State"];
            Emp.City = Request.Form["City"];
            Emp.ZipCode = Request.Form["ZipCode"];
            Emp.Address = Request.Form["Address"];
            Emp.Deactive = false;

            try
            {
                bool DuplicateEmail = false;
                if (ModelState.IsValid)
                {
                    EmpRepository EmpRepo = new EmpRepository();
                    if (EmpRepo.CheckDuplicateEmailId(Emp.EmpId.ToString(), Emp.Email.ToString()))
                    {
                        DuplicateEmail = true;
                    }
                    else
                    {
                        if (EmpRepo.EmployeeAddEdit(Emp, "insert"))
                        {
                            return RedirectToAction("GetAllEmpDetails");
                        }
                    }
                }

                if (DuplicateEmail)
                {
                    ViewBag.ErrorMessage = "Duplicate email!";
                }
                else
                {
                    ViewBag.ErrorMessage = "System error. Please try again!";
                }
                ViewBag.Message = "";

                StateRepository sr = new StateRepository();
                var sel = new SelectList(sr.GetAllStates(), "id", "StateName");
                ViewBag.Title = "HRMS - Add Employee";
                ViewBag.StateList = sel.ToList();
            }
            catch
            {
                ViewBag.ErrorMessage = "System error. Please try again!";
                ViewBag.Message = "";

                StateRepository sr = new StateRepository();
                var sel = new SelectList(sr.GetAllStates(), "id", "StateName");
                ViewBag.Title = "HRMS - Add Employee";
                ViewBag.StateList = sel.ToList();
            }

            return View(Emp);
        }

        public ActionResult EditEmployee(string id)
        {
            EmpRepository empRo = new EmpRepository();
            ModelState.Clear();

            StateRepository sr = new StateRepository();
            var sel = new SelectList(sr.GetAllStates(), "id", "StateName");
            ViewBag.Title = "HRMS - Edit Employee";
            ViewBag.StateList = sel.ToList();

            return View(empRo.GetSingleEmployeeOnId(id));
        }

        [HttpPost]
        [ActionName("EditEmployee")]
        public ActionResult EditEmployee1()
        {
            EmployeeModel Emp = new EmployeeModel();
            Emp.EmpId = Convert.ToInt32(Request.Form["EmpId"]);
            Emp.EmpName = Request.Form["EmpName"];
            Emp.Email = Request.Form["Email"];
            Emp.State = Request.Form["State"];
            Emp.City = Request.Form["City"];
            Emp.ZipCode = Request.Form["ZipCode"];
            Emp.Address = Request.Form["Address"];
            if (Request.Form["Deactive"].Contains("true"))
            {
                Emp.Deactive = true;
            }
            else
            {
                Emp.Deactive = false;
            }

            try
            {
                bool DuplicateEmail = false;
                if (ModelState.IsValid)
                {
                    EmpRepository EmpRepo = new EmpRepository();
                    if (EmpRepo.CheckDuplicateEmailId(Emp.EmpId.ToString(), Emp.Email.ToString()))
                    {
                        DuplicateEmail = true;
                    }
                    else
                    {
                        if (EmpRepo.EmployeeAddEdit(Emp, "update"))
                        {
                            return RedirectToAction("GetAllEmpDetails");
                        }
                    }

                    Emp = EmpRepo.GetSingleEmployeeOnId(Emp.EmpId.ToString());
                }

                if (DuplicateEmail)
                {
                    ViewBag.ErrorMessage = "Duplicate email!";
                }
                else
                {
                    ViewBag.ErrorMessage = "System error. Please try again!";
                }
                ViewBag.Message = "";

                StateRepository sr = new StateRepository();
                var sel = new SelectList(sr.GetAllStates(), "id", "StateName");
                ViewBag.Title = "HRMS";
                ViewBag.StateList = sel.ToList();
            }
            catch
            {
                ViewBag.ErrorMessage = "System error. Please try again!";
                ViewBag.Message = "";

                StateRepository sr = new StateRepository();
                var sel = new SelectList(sr.GetAllStates(), "id", "StateName");
                ViewBag.Title = "HRMS";
                ViewBag.StateList = sel.ToList();
            }

            return View(Emp);
        }

        //[HttpPost]
        public ActionResult EmployeeSalaryList(string id)
        {
            if (id == null)
            {
                id = Request.Form["EmpId"];
            }
            string SalaryYear = "";

            if (Request.Form["SalaryYear"] != null)
            {
                SalaryYear = Request.Form["SalaryYear"];
            }
            string SalaryMonth = "";

            if (Request.Form["SalaryMonth"] != null)
            {
                SalaryMonth = Request.Form["SalaryMonth"];
            }

            EmpRepository empRo = new EmpRepository();
            ModelState.Clear();

            ViewData["EmpId"] = id;

            ViewBag.Title = "HRMS - Employee Salary List";
            EmpRepository empRep = new EmpRepository();
            EmployeeModel emp = empRep.GetSingleEmployeeOnId(id);

            @ViewBag.EmpName = emp.EmpName + " (#" + emp.EmpId.ToString() + ")";

            return View(empRo.GetEmpSalaryList(id, SalaryYear, SalaryMonth));
        }

        public ActionResult AddEmployeeSalary(string EmpId)
        {
            ViewBag.Title = "HRMS - Add Employee Salary";
            EmpRepository empRep = new EmpRepository();
            EmployeeModel emp = empRep.GetSingleEmployeeOnId(EmpId);

            @ViewBag.EmpName = emp.EmpName + " (#" + emp.EmpId.ToString() + ")";
            return View();
        }

        public ActionResult EditEmployeeSalary(string id)
        {
            ViewBag.Title = "HRMS - Edit Employee Salary";
            EmpRepository empRo = new EmpRepository();
            ModelState.Clear();

            EmpRepository empRep = new EmpRepository();
            EmpSalaryModel empSal = empRo.GetSingleEmployeeSalaryOnId(id);
            string empid = Convert.ToString(empSal.EmpId);

            EmployeeModel emp = empRep.GetSingleEmployeeOnId(empid);

            @ViewBag.EmpName = emp.EmpName + " (#" + emp.EmpId.ToString() + ")";
            @ViewBag.EmpId = emp.EmpId.ToString();
            @ViewBag.SalMonth = empSal.SalaryMonth;

            return View(empRo.GetSingleEmployeeSalaryOnId(id));
        }

        [HttpPost]
        [ActionName("AddEmployeeSalary")]
        public ActionResult AddEmployeeSalary1()
        {
            EmpSalaryModel EmpSal = new EmpSalaryModel();
            EmpSal.EmpId = Convert.ToInt32(Request.Form["EmpId"]);
            EmpSal.Salary = Convert.ToInt32(Request.Form["Salary"]);
            EmpSal.SalaryYear = Convert.ToInt32(Request.Form["SalaryYear"]);
            EmpSal.SalaryMonth = Convert.ToInt32(Request.Form["SalaryMonth"]);

            try
            {
                bool DuplicateSalary = false;
                if (ModelState.IsValid)
                {
                    EmpRepository EmpRepo = new EmpRepository();
                    if (EmpRepo.CheckDuplicateEmpSalary(EmpSal.id.ToString(),EmpSal.EmpId.ToString(),EmpSal.SalaryYear.ToString(),EmpSal.SalaryMonth.ToString()))
                    {
                        DuplicateSalary = true;
                    }
                    else
                    {
                        if (EmpRepo.EmployeeSalaryAddEdit(EmpSal, "insert"))
                        {
                            return RedirectToAction("EmployeeSalaryList", new { id = EmpSal.EmpId });
                        }
                    }
                }

                if (DuplicateSalary)
                {
                    ViewBag.ErrorMessage = "Salary is already entered for " + MonthName(EmpSal.SalaryMonth) + ", " + EmpSal.SalaryYear + " for this employee!";
                }
                else
                {
                    ViewBag.ErrorMessage = "System error. Please try again!";
                }
                ViewBag.Message = "";
            }
            catch
            {
                ViewBag.ErrorMessage = "System error. Please try again!";
                ViewBag.Message = "";
            }

            ViewBag.Title = "HRMS - Add Employee Salary";
            EmpRepository empRep = new EmpRepository();
            EmployeeModel emp = empRep.GetSingleEmployeeOnId(EmpSal.EmpId.ToString());

            @ViewBag.EmpName = emp.EmpName + " (#" + emp.EmpId.ToString() + ")";
            return View();
        }

        [HttpPost]
        [ActionName("EditEmployeeSalary")]
        public ActionResult EditEmployeeSalary1()
        {
            EmpSalaryModel EmpSal = new EmpSalaryModel();
            EmpSal.id = Convert.ToInt32(Request.Form["id"]);
            EmpSal.EmpId = Convert.ToInt32(Request.Form["EmpId"]);
            EmpSal.Salary = Convert.ToInt32(Request.Form["Salary"]);
            EmpSal.SalaryYear = Convert.ToInt32(Request.Form["SalaryYear"]);
            EmpSal.SalaryMonth = Convert.ToInt32(Request.Form["SalaryMonth"]);

            try
            {
                bool DuplicateSalary = false;
                if (ModelState.IsValid)
                {
                    EmpRepository EmpRepo = new EmpRepository();
                    if (EmpRepo.CheckDuplicateEmpSalary(EmpSal.id.ToString(), EmpSal.EmpId.ToString(), EmpSal.SalaryYear.ToString(), EmpSal.SalaryMonth.ToString()))
                    {
                        DuplicateSalary = true;
                    }
                    else
                    {
                        if (EmpRepo.EmployeeSalaryAddEdit(EmpSal, "update"))
                        {
                            return RedirectToAction("EmployeeSalaryList", new { id = EmpSal.EmpId });
                        }
                    }
                }

                if (DuplicateSalary)
                {
                    ViewBag.ErrorMessage = "Salary is already entered for " + MonthName(EmpSal.SalaryMonth) + ", " + EmpSal.SalaryYear + " for this employee!";
                }
                else
                {
                    ViewBag.ErrorMessage = "System error. Please try again!";
                }
                ViewBag.Message = "";
            }
            catch
            {
                ViewBag.ErrorMessage = "System error. Please try again!";
                ViewBag.Message = "";
            }

            ViewBag.Title = "HRMS - Edit Employee Salary";
            EmpRepository empRep = new EmpRepository();
            EmployeeModel emp = empRep.GetSingleEmployeeOnId(EmpSal.EmpId.ToString());

            @ViewBag.EmpName = emp.EmpName + " (#" + emp.EmpId.ToString() + ")";
            EmpSal = empRep.GetSingleEmployeeSalaryOnId(Convert.ToString(EmpSal.id));
            @ViewBag.SalMonth = EmpSal.SalaryMonth;
            @ViewBag.EmpId = emp.EmpId.ToString();
            return View(EmpSal);
        }

        public ActionResult ShowMonth(string Year)
        {
            List<SelectListItem> lst = new List<SelectListItem>();

            int CurrentYear = DateTime.Now.Year;
            int MaxCount = 12;

            if (CurrentYear.ToString() == Year)
                MaxCount = DateTime.Now.Month - 1;

            for (int iloop = 1; iloop <= MaxCount; iloop++)
            {
                lst.Add(new SelectListItem
                {
                    Text = MonthName(iloop),
                    Value = iloop.ToString()
                });
            }

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        private string MonthName(int Month)
        {
            string mName = "";
            if (Month == 1)
                mName = "January";
            else if (Month == 2)
                mName = "February";
            else if (Month == 3)
                mName = "March";
            else if (Month == 4)
                mName = "April";
            else if (Month == 5)
                mName = "May";
            else if (Month == 6)
                mName = "June";
            else if (Month == 7)
                mName = "July";
            else if (Month == 8)
                mName = "August";
            else if (Month == 9)
                mName = "September";
            else if (Month == 10)
                mName = "October";
            else if (Month == 11)
                mName = "November";
            else if (Month == 12)
                mName = "December";

            return mName;
        }
    }
}
