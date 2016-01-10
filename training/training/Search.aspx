<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="training.Search" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel ="stylesheet" type ="text/css" href ="base.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div id ="header" class ="fixed">
            <asp:Label ID="Label1" runat="server" Text="Label">測定結果：検索／一覧</asp:Label>
            <br />
        </div>
        
        <div id="search_area" class ="fixed" aria-sort="other">
            <asp:Label ID="Label2" runat="server" Text="Label">検索条件を入力して下さい。</asp:Label>
            <br />
            <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="SqlDataSource1" DataTextField="Year" DataValueField="Year" AppendDataBoundItems="True">
                <asp:ListItem Value="All">全ての年</asp:ListItem>
            </asp:DropDownList>
            <br />
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:RESTransactionConnectionString %>" SelectCommand="SELECT DISTINCT [Year] FROM [STUDENT_HEALTH]"></asp:SqlDataSource>
            <asp:TextBox ID="TextBox1" runat="server" MaxLength="6"></asp:TextBox>
            <asp:Button ID="Button1" runat="server" Text="検索" OnClick="Button1_Click"　ValidationGroup ="AllValidators" />
            <asp:Label ID="Message1" runat="server" Text=""></asp:Label>
            <br />
            <asp:Button ID="Button2" runat="server" Text="年別レポート" OnClick="Button2_Click" />
            <br />
        </div>
        <br />
        <br />
        <div id="control_bar" class ="hidden fixed" runat="server" visible="false">
            <asp:Label ID="Message2" runat="server" Text=""></asp:Label>
            <asp:Button ID="Button3" runat="server" Text="新規" OnClick="Button3_Click" BackColor="#666666" BorderColor="#666666" ForeColor="White" />
            <asp:Button ID="Button4" runat="server" Text="修正" OnClick="Button4_Click" BackColor="#666666" BorderColor="#666666" ForeColor="White" />
            <asp:Button ID="Button5" runat="server" Text="削除" OnClick="Button5_Click" BackColor="#666666" BorderColor="#666666" ForeColor="White" />  
        </div>
        <br />
        <br />
        <div id ="result_area" class ="hidden" runat="server" visible ="false">

            <asp:GridView CssClass ="grid" ID="GridView1" runat="server" Style="margin-left: 0px" AutoGenerateColumns="False" CellPadding="1" CellSpacing="1" HorizontalAlign="Center" Width="600px">
                <Columns>
                    <asp:TemplateField>

                        <ItemTemplate>
                            <asp:CheckBox ID="Check" runat="server" />
                        </ItemTemplate>

                    </asp:TemplateField>
                    <asp:BoundField DataField="StudentId" HeaderText="生徒ID" SortExpression="StudentId" />
                    <asp:BoundField DataField="Gender" HeaderText="性別" SortExpression="Gender" />
                    <asp:BoundField DataField="Name" HeaderText="名前" SortExpression="Name" />
                    <asp:BoundField DataField="Height" HeaderText="身長" SortExpression="Height" />
                    <asp:BoundField DataField="Weight" HeaderText="体重" SortExpression="Weight" />
                    <asp:BoundField DataField="IdealWeight" HeaderText="理想体重" SortExpression="IdealWeight" />
                    <asp:BoundField DataField="Year" HeaderText="測定年" SortExpression="Year" />

                </Columns>
                <HeaderStyle BackColor="#99FF99" />
                <RowStyle BackColor="#CCFFCC" />
            </asp:GridView>
        </div>
        <div id ="footer" class ="fixed">

        </div>
    </form>
</body>
</html>
