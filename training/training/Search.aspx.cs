
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web.UI.WebControls;

namespace training
{
    public partial class Search : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            //セッションの初期化
            Session["studentId"] = "";
            Session["year"] = "";
            Session["height"] = "";
            Session["weight"] = "";

            if (!this.IsPostBack)
            {
                //他画面から戻ってきた時にもとの検索結果を表示
                if (Session["searchYear"] != null && Session["searchId"] != null)
                {
                    this.BindGrid((string)Session["searchYear"], (string)Session["searchId"]);
                }
                //削除画面から削除後に削除完了メッセージを表示
                if (Session["deleted"] != null&& Session["idList"] != null)
                {
                    List<string> idList = (List<string>)Session["idList"];
                    Message2.Text = idList.Count + "件のレコードの削除に成功しました。";
                    //セッションの初期化
                    Session["yearList"] = null;
                    Session["idList"] = null;
                }
                else
                {
                    //セッションの初期化
                    Session["yearList"] = null;
                    Session["idList"] = null;
                }
            }
        }


        /// <summary>
        /// 検索ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button1_Click(object sender, EventArgs e)
        {
            //入力チェック
            var intId = 0;

            if (((!(int.TryParse(TextBox1.Text, out intId))) || intId < 0) && TextBox1.Text != "")
            {
                //数値でないか、intの範囲を超えた場合、エラーメッセージを表示
                Message1.Text = "生徒IDの入力が不正です。";
            }
            else
            {
                //メッセージエリア①を初期化
                Message1.Text = "";
                
                //GridViewに検索結果を表示
                this.BindGrid(DropDownList1.SelectedValue, TextBox1.Text);
                
                //検索条件をセッションに保存
                Session["searchId"] = TextBox1.Text;
                Session["searchYear"] = DropDownList1.SelectedValue;
            }

        }

        /// <summary>
        /// データベースの問い合わせ結果をGridViewに表示する。
        /// </summary>
        private void BindGrid(string year, string studentId)
        {
            //操作メニューと一覧を表示
            control_bar.Visible = true;
            result_area.Visible = true;
            
            string constr = "Data Source=TRAINING;Initial Catalog=RESTransaction;Integrated Security=True";
            //ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    string sql = GenerateSqlStatement();

                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@year", year);
                    if (studentId == "")
                    {
                        cmd.Parameters.AddWithValue("@studentId", (String.Format("{0:D6}", studentId)));
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@studentId", (String.Format("{0:D6}", int.Parse(studentId))));
                    }
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            
                            foreach (DataRow r in dt.Rows)
                            {
                                var gender = r[1].ToString();

                                if (gender == "M")
                                {
                                    r["Gender"] = "男";
                                }
                                else if (gender == "F")
                                {
                                    r["Gender"] = "女";
                                }

                                      }
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
        private string GenerateSqlStatement()
        {
            string sql1 = "SELECT STUDENT_BASIC.StudentId, STUDENT_BASIC.Gender, STUDENT_BASIC.Name, STUDENT_HEALTH.Height, STUDENT_HEALTH.Weight, STUDENT_HEALTH.Year, STUDENT_HEALTH.IdealWeight FROM STUDENT_BASIC INNER JOIN STUDENT_HEALTH ON STUDENT_BASIC.StudentId = STUDENT_HEALTH.StudentId ";
            string sql2 = "";
            string sql3 = "ORDER BY STUDENT_BASIC.StudentId, STUDENT_BASIC.Gender, STUDENT_BASIC.Name, STUDENT_HEALTH.Height, STUDENT_HEALTH.Weight, STUDENT_HEALTH.IdealWeight, STUDENT_HEALTH.Year";
            //デフォルトは生徒IDも年も全件検索
            //生徒ID全件検索、年は特定検索
            if (TextBox1.Text == "" && DropDownList1.SelectedValue != "All")
            {
                sql2 = "WHERE STUDENT_HEALTH.Year = @year ";
            }
            //生徒ID特定検索、年は全件検索
            else if (TextBox1.Text != "" && DropDownList1.SelectedValue == "All")
            {
                sql2 = "WHERE STUDENT_BASIC.StudentId = @studentId ";
            }
            //生徒IDも年も特定検索
            else if (TextBox1.Text != "" && DropDownList1.SelectedValue != "All")
            {
                sql2 = "WHERE STUDENT_HEALTH.Year = @year and STUDENT_BASIC.StudentId = @studentId ";
            }

            string sql = sql1 + sql2 + sql3;

            return sql;
        }

        /// <summary>
        /// 年別レポートボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect("Report.aspx");
        }

        /// <summary>
        /// 新規ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button3_Click(object sender, EventArgs e)
        {
            Response.Redirect("New.aspx");
        }

        /// <summary>
        /// 修正ボタン押下
        /// チェックの数が1個ならチェックされた行の値をSessionに保存後、
        /// 修正画面へ遷移する。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button4_Click(object sender, EventArgs e)
        {
            string studentId = string.Empty;
            int year = 0;
            string height = string.Empty;
            string weight = string.Empty;
            int checkCount = 0;

            foreach(GridViewRow gRow in GridView1.Rows)
            {
                CheckBox check =gRow.FindControl("Check") as CheckBox;
               
                if(check.Checked)
                {
                    studentId = gRow.Cells[1].Text;
                    year =int.Parse(gRow.Cells[7].Text);
                    height = gRow.Cells[4].Text;
                    weight = gRow.Cells[5].Text;
                    checkCount += 1;
                }
            }
            if (checkCount == 1){
                Session["studentId"] = studentId;
                Session["year"] = year;
                Session["height"] = height;
                Session["weight"] = weight;
                Response.Redirect("Modify.aspx");
            }
            else if(checkCount > 1)
            {
                Message2.Text = "複数選択できません。";
            }
            else
            {
                Message2.Text = "未選択です。";
            }
        }

        /// <summary>
        /// 削除ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button5_Click(object sender, EventArgs e)
        {
            if (checkCheckBox())
            {
                Response.Redirect("DeleteConfirm.aspx");
                Session["deleted"] = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private bool checkCheckBox()
        {
            bool flag = false;
            using (StudentDataContext db = new StudentDataContext())
            {
                List<string> studentId = new List<string>();
                List<int> year = new List<int>();
                var checkCount = 0;
                foreach (GridViewRow gRow in GridView1.Rows)
                {
                    CheckBox check = gRow.FindControl("Check") as CheckBox;

                    if (check.Checked)
                    {
                        studentId.Insert(checkCount, gRow.Cells[1].Text);
                        year.Insert(checkCount, int.Parse(gRow.Cells[7].Text));
                        checkCount += 1;
                    }
                }
                if (checkCount < 1)
                {
                    Message2.Text = "未選択です。";
                }
                else
                {
                    Session["idList"] = studentId;
                    Session["yearList"] = year;
                    flag = true;
                }
            }
            return flag;
        }
    }

}
