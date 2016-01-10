<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="training.Report" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel ="stylesheet" type ="text/css" href ="base.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div id="header" class="fixed">
            <asp:Label ID="Label1" runat="server" Text="Label">測定結果：年別レポート</asp:Label>
            <br />
        </div>
        <div id="dropdown" class="fixed">
            <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource1" DataTextField="Year" DataValueField="Year">
            </asp:DropDownList>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:RESTransactionConnectionString %>" SelectCommand="SELECT DISTINCT [Year] FROM [STUDENT_HEALTH]"></asp:SqlDataSource>
            <br />
        </div>
        <div id ="result_area">

            <asp:Table ID="Table1" runat="server" GridLines="Both">
                <asp:TableRow>
                    <asp:TableHeaderCell>
                        
                    </asp:TableHeaderCell>
                    <asp:TableHeaderCell>
                        人数
                    </asp:TableHeaderCell>
                    <asp:TableHeaderCell>
                        平均身長
                    </asp:TableHeaderCell>
                    <asp:TableHeaderCell>
                        平均体重
                    </asp:TableHeaderCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableHeaderCell>
                        男
                    </asp:TableHeaderCell>
                    <asp:TableCell>
                        <asp:Literal ID="Literal1" runat="server"></asp:Literal>人
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Literal ID="Literal2" runat="server"></asp:Literal>cm
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Literal ID="Literal3" runat="server"></asp:Literal>kg
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableHeaderCell>
                        女
                    </asp:TableHeaderCell>
                    <asp:TableCell>
                        <asp:Literal ID="Literal4" runat="server"></asp:Literal>人
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Literal ID="Literal5" runat="server"></asp:Literal>cm
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Literal ID="Literal6" runat="server"></asp:Literal>kg
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableHeaderCell>
                        全体
                    </asp:TableHeaderCell>
                    <asp:TableCell>
                        <asp:Literal ID="Literal7" runat="server"></asp:Literal>人
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Literal ID="Literal8" runat="server"></asp:Literal>cm
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Literal ID="Literal9" runat="server"></asp:Literal>kg
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>

            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="戻る" />

        </div>
    </form>
</body>
</html>
