using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace training
{
    public partial class Report : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindTable();
           
        }

        protected void SelectedIndexChanged(object sender, EventArgs e)
        {
            BindTable();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("Search.aspx");
        }

        private void BindTable()
        {
            string constr = "Data Source=TRAINING;Initial Catalog=RESTransaction;Integrated Security=True";

            //ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    string sql = "SELECT STUDENT_BASIC.Gender, STUDENT_HEALTH.Height, STUDENT_HEALTH.Weight FROM STUDENT_BASIC INNER JOIN STUDENT_HEALTH ON STUDENT_BASIC.StudentId = STUDENT_HEALTH.StudentId WHERE STUDENT_HEALTH.Year = @year";
                    cmd.CommandText = sql;

                    object x = DropDownList1.SelectedValue;

                    if (!IsPostBack){
                        x = "2013";
                    }

                    cmd.Parameters.AddWithValue("@year", x);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        cmd.Connection = con;
                        int maleCount = 0;
                        int femaleCount = 0;
                        double maleHeightTotal = 0;
                        double maleWeightTotal = 0;
                        double femaleHeightTotal = 0;
                        double femaleWeightTotal = 0;
                        
                        // 男女の合計を計算します。
                        while (reader.Read())
                        {
                            if (reader.GetValue(0).ToString() == "M")
                            {
                                maleHeightTotal += System.Convert.ToDouble(reader.GetValue(1));
                                maleWeightTotal += System.Convert.ToDouble(reader.GetValue(2));
                                maleCount += 1;
                            }
                            else if ((string)reader.GetValue(0).ToString() == "F")
                            {
                                femaleHeightTotal += (double)System.Convert.ToDecimal(reader.GetValue(1));
                                femaleWeightTotal += (double)System.Convert.ToDecimal(reader.GetValue(2));
                                femaleCount += 1;
                            }
                        }
                        Literal1.Text = maleCount.ToString();
                        Literal2.Text = (maleHeightTotal / maleCount).ToString("f2");
                        Literal3.Text = (maleWeightTotal / maleCount).ToString("f2");
                        Literal4.Text = femaleCount.ToString();
                        Literal5.Text = (femaleHeightTotal / femaleCount).ToString("f2");
                        Literal6.Text = (femaleWeightTotal / femaleCount).ToString("f2");
                        Literal7.Text = (maleCount+femaleCount).ToString();
                        Literal8.Text = ((maleHeightTotal+femaleHeightTotal) / (maleCount + femaleCount)).ToString("f2");
                        Literal9.Text = ((maleWeightTotal + femaleWeightTotal) / (maleCount + femaleCount)).ToString("f2");
                    }
                }
            }
        }
    }
}