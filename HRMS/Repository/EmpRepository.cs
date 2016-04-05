using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using HRMS.Models;
using System.Configuration;

namespace HRMS.Repository
{
    public class EmpRepository
    {
        private SqlConnection con;
        //To Handle connection related activities
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["getconn"].ToString();
            con = new SqlConnection(constr);
        }

        public List<EmployeeModel> GetAllEmployees()
        {
            connection();
            List<EmployeeModel> EmpList = new List<EmployeeModel>();
            string strSql = "select isnull(emp.EmpId, 0)as EmpId, isnull(emp.EmpName, '')as EmpName, isnull(emp.Email, '')as Email, isnull(st.StateName, '')as [State], ISNULL(emp.City, '')as City, isnull(emp.ZipCode, '') as ZipCode, convert(nvarchar(10), CreatedOn, 101)as StartDate, case Deactive when 0 then 'True' else 'False' end as Active from Employee emp left outer join States st on st.id = emp.State";
            SqlCommand com = new SqlCommand(strSql, con);
            com.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();

            //Bind EmpModel generic list using LINQ 
            EmpList = (from DataRow dr in dt.Rows

                       select new EmployeeModel()
                       {
                           EmpId = Convert.ToInt32(dr["EmpId"]),
                           EmpName = Convert.ToString(dr["EmpName"]),
                           Email = Convert.ToString(dr["Email"]),
                           City = Convert.ToString(dr["City"]),
                           State = Convert.ToString(dr["State"]),
                           ZipCode = Convert.ToString(dr["ZipCode"]),
                           CreatedOn = Convert.ToString(dr["StartDate"]),
                           ActiveStatus = Convert.ToString(dr["Active"])
                       }).ToList();


            return EmpList;


        }

        public List<EmployeeModel> GetSearchedEmployees(string EmployeeName, string status)
        {
            connection();
            List<EmployeeModel> EmpList = new List<EmployeeModel>();
            string strSql = "select isnull(emp.EmpId, 0)as EmpId, isnull(emp.EmpName, '')as EmpName, isnull(emp.Email, '')as Email, isnull(st.StateName, '')as [State], ISNULL(emp.City, '')as City, isnull(emp.ZipCode, '') as ZipCode, convert(nvarchar(10), CreatedOn, 101)as StartDate, case Deactive when 0 then 'True' else 'False' end as Active from Employee emp left outer join States st on st.id = emp.State where 1=1";
            if (EmployeeName != "" && EmployeeName != null)
            {
                strSql = strSql + " and emp.EmpName like '%" + EmployeeName.Replace("'", "''") + "%'";
            }
            if (status != "" && status != null)
            {
                strSql = strSql + " and deactive = " + status;
            }
            SqlCommand com = new SqlCommand(strSql, con);
            com.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();

            //Bind EmpModel generic list using LINQ 
            EmpList = (from DataRow dr in dt.Rows

                       select new EmployeeModel()
                       {
                           EmpId = Convert.ToInt32(dr["EmpId"]),
                           EmpName = Convert.ToString(dr["EmpName"]),
                           Email = Convert.ToString(dr["Email"]),
                           City = Convert.ToString(dr["City"]),
                           State = Convert.ToString(dr["State"]),
                           ZipCode = Convert.ToString(dr["ZipCode"]),
                           CreatedOn = Convert.ToString(dr["StartDate"]),
                           ActiveStatus = Convert.ToString(dr["Active"])
                       }).ToList();


            return EmpList;


        }

        public bool CheckDuplicateEmailId(string EmpId, string Email)
        {
            connection();
            bool RetValue = false;
            string strSql = "select EmpId from Employee where Email = '" + Email + "' and Deactive = 0 and EmpId <> " + EmpId;
            SqlCommand com = new SqlCommand(strSql, con);
            com.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    RetValue = true;
                }
            }

            return RetValue;
        }

        public bool CheckDuplicateEmpSalary(string SalId, string EmpId, string year,string month)
        {
            connection();
            bool RetValue = false;
            string strSql = "select * from EmpSalary where EmpId = " + EmpId + " and SalaryMonth = " + month + " and SalaryYear = " + year + " and id <> " + SalId;
            SqlCommand com = new SqlCommand(strSql, con);
            com.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    RetValue = true;
                }
            }

            return RetValue;
        }

        public EmployeeModel GetSingleEmployeeOnId(string EmpId)
        {
            connection();
            List<EmployeeModel> EmpList = new List<EmployeeModel>();
            string strSql = "select * from Employee where EmpId = " + EmpId;
            SqlCommand com = new SqlCommand(strSql, con);
            com.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();

            //Bind EmpModel generic list using LINQ 
            EmpList = (from DataRow dr in dt.Rows

                       select new EmployeeModel()
                       {
                           EmpId = Convert.ToInt32(dr["EmpId"]),
                           EmpName = Convert.ToString(dr["EmpName"]),
                           Email = Convert.ToString(dr["Email"]),
                           City = Convert.ToString(dr["City"]),
                           State = Convert.ToString(dr["State"]),
                           ZipCode = Convert.ToString(dr["ZipCode"]),
                           Deactive = Convert.ToBoolean(dr["Deactive"]),
                           Address = Convert.ToString(dr["Address"])
                       }).ToList();


            return EmpList.FirstOrDefault();


        }

        public bool EmployeeAddEdit(EmployeeModel obj, string CallType)
        {
            connection();
            SqlCommand com = new SqlCommand("sp_ManageEmployee", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@EmpId", obj.EmpId);
            com.Parameters.AddWithValue("@EmpName", obj.EmpName);
            com.Parameters.AddWithValue("@Email", obj.Email);
            com.Parameters.AddWithValue("@State", obj.State);
            com.Parameters.AddWithValue("@City", obj.City);
            com.Parameters.AddWithValue("@Address", obj.Address);
            com.Parameters.AddWithValue("@ZipCode", obj.ZipCode);
            com.Parameters.AddWithValue("@Deactive", obj.Deactive);
            com.Parameters.AddWithValue("@CallType", CallType);

            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<EmpSalaryModel> GetEmpSalaryList(string EmpId, string SalaryYear, string SalaryMonth)
        {
            connection();
            List<EmpSalaryModel> EmpSalList = new List<EmpSalaryModel>();
            string strSql = "select * from EmpSalary where EmpId = " + EmpId;

            if (SalaryYear != "")
            {
                strSql = strSql + " and SalaryYear = " + SalaryYear;
            }

            if (SalaryMonth != "")
            {
                strSql = strSql + " and SalaryMonth = " + SalaryMonth;
            }

            strSql = strSql + " order by SalaryYear desc, SalaryMonth desc";

            SqlCommand com = new SqlCommand(strSql, con);
            com.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();

            //Bind EmpModel generic list using LINQ 
            EmpSalList = (from DataRow dr in dt.Rows

                          select new EmpSalaryModel()
                          {
                              id = Convert.ToInt32(dr["id"]),
                              Salary = Convert.ToInt32(dr["Salary"]),
                              SalaryMonth = Convert.ToInt32(dr["SalaryMonth"]),
                              SalaryYear = Convert.ToInt32(dr["SalaryYear"])
                          }).ToList();


            return EmpSalList;


        }

        public bool EmployeeSalaryAddEdit(EmpSalaryModel obj, string CallType)
        {
            connection();
            SqlCommand com = new SqlCommand("sp_ManageEmpSalary", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@id", obj.id);
            com.Parameters.AddWithValue("@EmpId", obj.EmpId);
            com.Parameters.AddWithValue("@Salary", obj.Salary);
            com.Parameters.AddWithValue("@SalaryMonth", obj.SalaryMonth);
            com.Parameters.AddWithValue("@SalaryYear", obj.SalaryYear);
            com.Parameters.AddWithValue("@CallType", CallType);

            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {

                return true;

            }
            else
            {

                return false;
            }
        }

        public EmpSalaryModel GetSingleEmployeeSalaryOnId(string id)
        {
            connection();
            List<EmpSalaryModel> EmpSalList = new List<EmpSalaryModel>();
            string strSql = "select * from EmpSalary where id = " + id;
            SqlCommand com = new SqlCommand(strSql, con);
            com.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();

            //Bind EmpModel generic list using LINQ 
            EmpSalList = (from DataRow dr in dt.Rows

                          select new EmpSalaryModel()
                          {
                              id = Convert.ToInt32(dr["id"]),
                              EmpId = Convert.ToInt32(dr["EmpId"]),
                              Salary = Convert.ToInt32(dr["Salary"]),
                              SalaryMonth = Convert.ToInt32(dr["SalaryMonth"]),
                              SalaryYear = Convert.ToInt32(dr["SalaryYear"])
                          }).ToList();


            return EmpSalList.FirstOrDefault();


        }
    }
}