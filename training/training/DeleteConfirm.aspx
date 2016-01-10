<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeleteConfirm.aspx.cs" Inherits="training.DeleteConfirm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="stylesheet" type="text/css" href="base.css" />

</head>
<body>
    <form id="form1" runat="server">
        <div id="header" class="fixed">
            <asp:Label ID="Label1" runat="server" Text="Label">測定結果：削除</asp:Label>
            <br />
        </div>
        <asp:Label ID="Label2" runat="server" Text="Label">以下のレコードを削除してもよろしいですか？</asp:Label>
        <br />
        <br />
        <asp:GridView ID="GridView1" runat="server" Style="margin-left: 0px" AutoGenerateColumns="False"> 
            <Columns>
                <asp:BoundField DataField="StudentId" HeaderText="生徒ID" SortExpression="StudentId" />
                <asp:BoundField DataField="Name" HeaderText="名前" SortExpression="Name" />
                <asp:BoundField DataField="Height" HeaderText="身長" SortExpression="Height" />
                <asp:BoundField DataField="Weight" HeaderText="体重" SortExpression="Weight" />
                <asp:BoundField DataField="Year" HeaderText="測定年" SortExpression="Year" />
            </Columns>
        </asp:GridView>
        <br />
        <br />
        <asp:Button ID="Button1" runat="server" Text="削除" OnClick="Button1_Click" />
        <asp:Button ID="Button2" runat="server" Text="キャンセル" OnClick="Button2_Click" />
        <br />
        <asp:Label ID="Label13" runat="server" Text=""></asp:Label>
    </form>
</body>
</html>
