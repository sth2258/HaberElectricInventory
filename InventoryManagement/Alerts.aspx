<%@ Page Title="Inventory Alert Thresholds" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" Trace="true" CodeBehind="Alerts.aspx.cs" Inherits="InventoryManagement.Alerts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <p>Use this page to set inventory thresholds for alerts. Set a lower bound or upper bound. Only critical inventory items are shown here.<br />
        <br />
        A lower bound would alert if you have less than the minimum amount of a product (i.e., order move of this).<br />
        <br />
        An upper bound would alert if you have more than the maximum amount of the product (i.e., sell more of this).<br />
        <br />
        If you wish to disable an alert, put a -1 in for the value.</p>
    <hr />
    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false">
        <Columns>
            <asp:TemplateField HeaderText="UPC" ItemStyle-Width="30">
                <ItemTemplate>
                    <asp:Label ID="lbl_UPC" runat="server" Text='<%# Eval("UPC") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Item Name" ItemStyle-Width="150">
                <ItemTemplate>
                    <asp:Label ID="lbl_ItemName" runat="server" Text='<%# Eval("ItemName") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Category" ItemStyle-Width="150">
                <ItemTemplate>
                    <asp:Label ID="lbl_Category" runat="server" Text='<%# Eval("Category") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Lower Bound" ItemStyle-Width="150">
                <ItemTemplate>
                    <asp:TextBox ID="txt_Lower" runat="server" Text='<%# Eval("LowerBound") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Upper Bound" ItemStyle-Width="150">
                <ItemTemplate>
                    <asp:TextBox ID="txt_Upper" runat="server" Text='<%# Eval("UpperBound") %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <br />
    <br />
    <asp:Button ID="Button1" runat="server" Text="Update" OnClick="Button1_Click" />

</asp:Content>
