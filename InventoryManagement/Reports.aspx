<%@ Page Title="Reports" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="InventoryManagement.Reports" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br /><br /><asp:GridView ID="GridView1" runat="server"></asp:GridView>
    
    
    <br /><br /><asp:Button ID="btn_email" runat="server" Text="E-Mail Current Inventory!" OnClick="btn_email_Click" />

</asp:Content>
