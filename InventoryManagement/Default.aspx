<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="InventoryManagement._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <br />
     <p>Scan UPC (or enter HE Inventory Code): <asp:TextBox ID="upcCode" runat="server"></asp:TextBox><br /><br /><asp:Button ID="searchBtn" runat="server" Text="Item Search!" /></p>
    <asp:Label ID="Label1" runat="server" Text=""></asp:Label>

    <asp:Panel ID="updatePannel" runat="server" Visible="False">
        <table>
            <tbody>
                <tr><td>UPC:</td><td><asp:TextBox ID="upd_UPC" runat="server" Enabled="False" Width="295px"></asp:TextBox></td></tr>
                <tr><td>Item Name:</td><td><asp:TextBox ID="upd_itemName" runat="server" Width="295px"></asp:TextBox></td></tr>
                <tr><td>Item Description:</td><td><asp:TextBox ID="upd_itemDesc" runat="server" Width="295px"></asp:TextBox></td></tr>
                <tr><td>Amount on hand:</td><td><asp:TextBox ID="upd_count" runat="server" Width="80px"></asp:TextBox></td></tr>
                <tr><td>Haber Electric Inventory Code:</td><td> <asp:TextBox ID="upd_inventoryCode" runat="server" Width="295px"></asp:TextBox></td></tr>
                <tr><td>Last Updated: </td><td><asp:TextBox ID="upd_lastUpdated" runat="server" Enabled="False" Width="295px"></asp:TextBox></td></tr>
            </tbody>
        </table>
        <br /><br />
        <asp:Button ID="upd_UpdateButton" runat="server" Text="Update Inventory!" OnClick="upd_UpdateButton_Click" />
        <br /><br />
        <asp:Button ID="upd_Plus1" runat="server" Text="+1" OnClick="upd_Plus1_Click" />
        <asp:Button ID="upd_Minus1" runat="server" Text="-1" OnClick="upd_Minus1_Click" />
        <br />
        <asp:Button ID="upd_Plus10" runat="server" Text="+10" OnClick="upd_Plus10_Click" />
        <asp:Button ID="upd_Minus10" runat="server" Text="-10" OnClick="upd_Minus10_Click" />
        <br /><br />
        <asp:Button ID="btn_Delete" runat="server" Text="Delete Item" OnClientClick="return confirm('Are you certain you want to delete this product?');" OnClick="btn_Delete_Click" />
    </asp:Panel>
</asp:Content>
