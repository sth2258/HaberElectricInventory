<%@ Page Title="New Item" Language="C#" AutoEventWireup="true" CodeBehind="NewItem.aspx.cs" MasterPageFile="~/Site.Master" Inherits="InventoryManagement.NewItem" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <br /><br />
    <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
    <table>
            <tbody>
                <tr><td>UPC:</td><td><asp:TextBox ID="new_UPC" runat="server" Enabled="True" Width="295px"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="UPC's are required for all products" ControlToValidate="new_UPC" ForeColor="Red">*</asp:RequiredFieldValidator></td></tr>
                <tr><td>Item Name:</td><td><asp:TextBox ID="new_itemName" runat="server" Width="295px"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Item Names are required" ControlToValidate="new_itemName" ForeColor="Red">*</asp:RequiredFieldValidator></td></tr>
                <tr><td>Item Description:</td><td><asp:TextBox ID="new_itemDesc" runat="server" Width="295px"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Item Description is required" ControlToValidate="new_itemDesc" ForeColor="Red">*</asp:RequiredFieldValidator></td></tr>
                <tr><td>Initial Amount:</td><td><asp:TextBox ID="new_count" runat="server" Width="80px"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Initial number in inventory is required" ControlToValidate="new_count" ForeColor="Red">*</asp:RequiredFieldValidator></td></tr>
                <tr><td>Haber Electric Inventory Code:</td><td> <asp:TextBox ID="new_inventoryCode" runat="server" Width="295px"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="A Haber Electric Inventory Code ID is required" ControlToValidate="new_inventorycode" ForeColor="Red">*</asp:RequiredFieldValidator></td></tr>
            </tbody>
        </table>
        <br /><br />
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="Red" />
    <asp:Button ID="new_BtnAddItem" runat="server" Text="Add New Item!" OnClick="new_BtnAddItem_Click" />
    </asp:Content>