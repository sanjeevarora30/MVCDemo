using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRMS.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace HRMS.Repository
{
    public class StateRepository
    {
        private SqlConnection con;
        //To Handle connection related activities
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["getconn"].ToString();
            con = new SqlConnection(constr);
        }

        public List<StateModel> GetAllStates()
        {
            connection();
            List<StateModel> StateList = new List<StateModel>();
            string strSql = "select * from States order by StateName";
            SqlCommand com = new SqlCommand(strSql, con);
            com.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();

            //Bind EmpModel generic list using LINQ 
            StateList = (from DataRow dr in dt.Rows

                       select new StateModel()
                       {
                           id = Convert.ToInt32(dr["id"]),
                           StateName = Convert.ToString(dr["StateName"])
                       }).ToList();


            return StateList;


        }
    }
}