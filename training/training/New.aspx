<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="New.aspx.cs" Inherits="training.New" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel ="stylesheet" type ="text/css" href ="base.css" />
    <script type="text/javascript">
        function check() {
            var studentId = document.getElementById("TextBox1").value;
            var height = document.getElementById("TextBox2").value;
            var weight = document.getElementById("TextBox3").value;

            var flag = 0;

            if (studentId == "") { 
                flag = 1;
            }
            else if (height == "") {
                flag = 1;
            }
            else if (weight == "") { 
                flag = 1;
            }

            if (flag) {
                document.getElementById("Label7").innerHTML = '必須項目が未入力です。';
                return false; // 送信を中止
            }
            else {
                return true; // 送信を実行

            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="header" class="fixed">
            <asp:Label ID="Label1" runat="server" Text="Label">測定結果：新規登録</asp:Label>
            <br />
        </div>
        <asp:Label ID="Label2" runat="server" Text="Label">必要事項を入力して下さい。</asp:Label>
        <br />
        <br />
        <asp:Label ID="Label3" runat="server" Text="Label">測定年</asp:Label>
        <asp:DropDownList ID="DropDownList1" runat="server">
            <asp:ListItem>2015</asp:ListItem>
            <asp:ListItem>2014</asp:ListItem>
            <asp:ListItem>2013</asp:ListItem>
            <asp:ListItem>2012</asp:ListItem>
            <asp:ListItem>2011</asp:ListItem>
            <asp:ListItem>2010</asp:ListItem>
            <asp:ListItem>2009</asp:ListItem>
            <asp:ListItem>2008</asp:ListItem>
            <asp:ListItem>2007</asp:ListItem>
            <asp:ListItem>2006</asp:ListItem>
            <asp:ListItem>2005</asp:ListItem>
            <asp:ListItem>2004</asp:ListItem>
            <asp:ListItem>2003</asp:ListItem>
            <asp:ListItem>2002</asp:ListItem>
            <asp:ListItem>2001</asp:ListItem>
            <asp:ListItem>2000</asp:ListItem>
        </asp:DropDownList>
        <br />
        <asp:Label ID="Label4" runat="server" Text="Label">生徒ID</asp:Label>
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        <br />
        <asp:Label ID="Label5" runat="server" Text="Label">身長</asp:Label>
        <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
        <br />
        <asp:Label ID="Label6" runat="server" Text="Label">体重</asp:Label>
        <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
        <br />
        <br />
        <asp:Button ID="Button1" runat="server" Text="登録" OnClientClick="return check();" OnClick="Button1_Click" />
        <asp:Button ID="Button2" runat="server" Text="戻る" OnClick="Button2_Click" />
        <br />
        <asp:Label ID="Label7" runat="server" Text=""></asp:Label>
    </form>
</body>
</html>
