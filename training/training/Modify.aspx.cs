using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace training
{
    public partial class Modify : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["studentId"] != null && Session["year"] != null)
            {
                Label4.Text = (string)Session["studentId"];
                Label3.Text = Session["year"].ToString();
                TextBox1.Text = (string)Session["height"];
                TextBox2.Text = (string)Session["weight"];
            }
            
            if (!this.IsPostBack)
            {
                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button1_Click(object sender, EventArgs e)
        {
            //入力チェック（身長・体重）
            var error = new StringBuilder();

            error = inputChecker(error);

            if (error.Length != 0)
            {
                //ここまででエラーがあればそのエラーメッセージを表示して終了
                Label7.Text = error.ToString();
            }
            else
            {
                updateRecord();//UPDATEする（身長・体重・理想体重)

            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect("Search.aspx");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        private StringBuilder inputChecker(StringBuilder error)
        {
            if (TextBox1.Text == (string)Session["height"] && TextBox2.Text == (string)Session["weight"])
            {
                error.Append("値が変更されていません。<br/>");
            }
            else
            {
                var heightCheck = heightValidator();
                if (!heightCheck)
                {
                    error.Append("身長の入力が不正です。<br/>");
                }
                var weightCheck = weightValidator();
                if (!weightCheck)
                {
                    error.Append("体重の入力が不正です。<br/>");
                }
            }

            return error;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool heightValidator()
        {
            var flag = false;
            Regex regex = new System.Text.RegularExpressions.Regex(@"^[0-9]{1,3}\.[0-9]{1,2}$");//小数点あり
            Regex regex2 = new System.Text.RegularExpressions.Regex(@"^[0-9]{1,3}$"); //小数点なし
            if (regex.IsMatch(TextBox1.Text) || regex2.IsMatch(TextBox1.Text))
            {
                flag = true;
            }

            return flag;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool weightValidator()
        {
            var flag = false;
            Regex regex = new System.Text.RegularExpressions.Regex(@"^[0-9]{1,3}\.[0-9]{1,2}$");//小数点あり
            Regex regex2 = new System.Text.RegularExpressions.Regex(@"^[0-9]{1,3}$"); //小数点なし
            if (regex.IsMatch(TextBox2.Text) || regex2.IsMatch(TextBox2.Text))
            {
                flag = true;
            }

            return flag;
        }

        /// <summary>
        /// 
        /// </summary>
        private void updateRecord()
        {
            using (StudentDataContext db = new StudentDataContext())
            {
                var height = Math.Round(double.Parse(TextBox1.Text), 2, MidpointRounding.AwayFromZero);
                var weight = Math.Round(double.Parse(TextBox2.Text), 2, MidpointRounding.AwayFromZero);
                var idealWeight = (height - 100) * 0.9;

                var updateQuery = from student in db.STUDENT_HEALTH
                               where student.StudentId == Label4.Text && student.Year == System.Convert.ToInt32(Label3.Text)
                               select student;

                foreach (var student in updateQuery)
                {
                    student.Height = (decimal)height;
                    student.Weight = (decimal)weight;
                    student.IdealWeight = (decimal)idealWeight;
                }

                try
                {
                    db.SubmitChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            
            }

        }
    }
}