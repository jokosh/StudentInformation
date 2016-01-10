using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace training
{
    public partial class DeleteConfirm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((List<int>)Session["yearList"] != null && (List<string>)Session["idList"] != null)
            {
                BindGrid((List<int>)Session["yearList"], (List<string>)Session["idList"]);
            }
                
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            deleteRecord();
            Response.Redirect("Search.aspx");
        }


        private void BindGrid(List<int> yearList, List<string>idList)
        {

            string constr = "Data Source=TRAINING;Initial Catalog=RESTransaction;Integrated Security=True";
            //ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            
            //SQLを使ってGridViewにデータをバインド
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    
                    string sql = GenerateSqlStatement(yearList,idList);
                
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@year", yearList[0]);
                    cmd.Parameters.AddWithValue("@studentId", (String.Format("{0:D6}", int.Parse(idList[0]))));

                    if (idList.Count > 1)
                    {
                        for (int i = 1; i < idList.Count; i++)
                        {
                            cmd.Parameters.AddWithValue("@year" + i.ToString(), yearList[i]);
                            cmd.Parameters.AddWithValue("@studentId" + i.ToString(), (String.Format("{0:D6}", int.Parse(idList[i]))));
                        }
                    }

                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            GridView1.DataSource = dt;
                            GridView1.DataBind();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 一覧出力用SQLクエリを生成する。
        /// </summary>
        /// <returns></returns>
        private string GenerateSqlStatement(List<int> yearList, List<string> idList)
        {
            string sql1 = "SELECT STUDENT_BASIC.StudentId, STUDENT_BASIC.Name, STUDENT_HEALTH.Height, STUDENT_HEALTH.Weight, STUDENT_HEALTH.Year FROM STUDENT_BASIC INNER JOIN STUDENT_HEALTH ON STUDENT_BASIC.StudentId = STUDENT_HEALTH.StudentId ";
            string sql2 ="WHERE (STUDENT_HEALTH.Year = @year AND STUDENT_BASIC.StudentId = @studentId)";
            string sql3 = " ORDER BY STUDENT_BASIC.StudentId, STUDENT_BASIC.Name, STUDENT_HEALTH.Height, STUDENT_HEALTH.Weight, STUDENT_HEALTH.Year";

            string addStmt = "";
 
            if (idList.Count > 1)
            {
                for (int i = 1; i < idList.Count; i++)
                {
                    addStmt = " or (STUDENT_HEALTH.Year = @year" + i + " AND STUDENT_BASIC.StudentId = @studentId"+ i +")";
                    sql2 = sql2 + addStmt;
                }
            }

            string sql = sql1 + sql2 + sql3;

            return sql;
        }


        /// <summary>
        /// 
        /// </summary>
        private void deleteRecord()
        {
            using (StudentDataContext db = new StudentDataContext())
            {
                List<string> studentId = (List<string>)Session["idList"];
                List<int> year = (List<int>)Session["yearList"];

                for (int i = 0; i < studentId.Count; i++)
                {
                    var deleteQuery = from record in db.STUDENT_HEALTH
                                      where record.StudentId == studentId[i] && record.Year == year[i]
                                      select record;

                    foreach (var deleteData in deleteQuery)
                    {
                        db.STUDENT_HEALTH.DeleteOnSubmit(deleteData);
                    }

                    try
                    {
                        db.SubmitChanges();
                        Session["deleted"] = true;
                    }
                    catch (Exception error)
                    {
                        Console.WriteLine(error);
                    }
                }
            }
        }


        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect("Search.aspx");
        }

    }

}

