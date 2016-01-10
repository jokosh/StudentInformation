using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace training
{
    public partial class New : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button1_Click(object sender, EventArgs e)
        {
            var error = new StringBuilder();

            error = inputChecker(error);

            if (error.Length !=0)
            {
                //ここまででエラーがあればそのエラーメッセージを表示して終了
                Label7.Text = error.ToString();
            }
            else
            {
                Label7.Text = "";
                if (duplicateChecker())
                {
                    //データベースに重複するID、測定年のデータがあればエラーメッセージ
                    error.Append("選択された年には既にレコードが存在しています。<br/>");
                    Label7.Text = error.ToString();
                }
                else
                {
                    insertRecord(); //INSERTする
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        private StringBuilder inputChecker(StringBuilder error)
        {
            var idCheck = IDValidator();
            if (idCheck == 1)
            {
                error.Append("生徒IDの入力が不正です。<br/>");

            }
            else if (idCheck == 2)
            {
                error.Append("生徒IDが存在しません。<br/>");

            }

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
            return error;
        }

        /// <summary>
        /// IDチェック
        /// </summary>
        /// <returns>エラー内容によって返り値が変わる</returns>
        private int IDValidator()
        {
            var flag = 0;
            var intId = 0;

            if ((!(int.TryParse(TextBox1.Text, out intId))))
            //数値でないか、intの範囲を超えた場合、エラーメッセージを表示
            {
                flag = 1;
            }
            else if ((intId < 0 || intId.ToString().Length > 6))
            //6桁以上で0より小さい数値の場合、エラーメッセージを表示
            {
                flag = 1;
            }
            else //正しい数値の場合、続行
            {
                string studentId = String.Format("{0:D6}", intId);

                //LINQ to SQL
                string conStr = "Data Source=TRAINING;Initial Catalog=RESTransaction;Integrated Security=True";

                var db = new StudentDataContext(conStr);

                var students = from s in db.STUDENT_BASIC
                               where s.StudentId == studentId
                               select s;

                bool hasMatch = students.Any();

                if (hasMatch == false)
                {
                    flag = 2;
                }

            }
            return flag;
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
            if (regex.IsMatch(TextBox2.Text) || regex2.IsMatch(TextBox2.Text))
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
            if (regex.IsMatch(TextBox3.Text) || regex2.IsMatch(TextBox3.Text))
            {
                flag = true;
            }

            return flag;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool duplicateChecker()
        {
            var flag = false;

            //LINQ to SQL

            using (var db = new StudentDataContext())
            {
                var studentId = String.Format("{0:D6}", int.Parse(TextBox1.Text));
                
                var students = from student in db.STUDENT_BASIC
                               join health in db.STUDENT_HEALTH on student.StudentId equals health.StudentId
                               where student.StudentId ==studentId && health.Year == System.Convert.ToInt32(DropDownList1.Text)
                               select student;

                bool hasMatch = students.Any();

                if (hasMatch == true)
                {
                    flag = true; //DBに重複レコード有
                }
            }


            return flag;
        }

        /// <summary>
        /// 
        /// </summary>
        private void insertRecord()
        {
            //結果表示用
            //string result = string.Empty;

            using (StudentDataContext student = new StudentDataContext())
            {
                var studentId = String.Format("{0:D6}", int.Parse(TextBox1.Text));
                var height = Math.Round(double.Parse(TextBox2.Text), 2, MidpointRounding.AwayFromZero);
                var weight = Math.Round(double.Parse(TextBox3.Text), 2, MidpointRounding.AwayFromZero);
                var idealWeight = (height - 100) * 0.9;
                var year = short.Parse(DropDownList1.Text);

                STUDENT_HEALTH health = new STUDENT_HEALTH
               {
                   StudentId = studentId,
                   Height = (decimal)height,
                   Weight = (decimal)weight,
                   IdealWeight = (decimal)idealWeight,
                   Year = year
               };
                student.STUDENT_HEALTH.InsertOnSubmit(health);

                try
                {
                    student.SubmitChanges();
                    DropDownList1.Text = "2015";
                    TextBox1.Text = "";
                    TextBox2.Text = "";
                    TextBox3.Text = "";
                    Label7.Text = "登録に成功しました。";
                }
                catch(Exception e)
                {
                    Debug.WriteLine(e);
                    Label7.Text = "登録に失敗しました。";
                }
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

    }

}