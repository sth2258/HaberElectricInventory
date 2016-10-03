<%@ Page Title="Reports" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="InventoryManagement.Reports" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <a href="#" onclick="PrintPage('GridTable')">Printer Friendly Format</a>
    <br /><br /><div id="GridTable"><asp:GridView ID="GridView1" runat="server"></asp:GridView></div>
    
    
    <br /><br /><asp:Button ID="btn_email" runat="server" Text="E-Mail Current Inventory!" OnClick="btn_email_Click" />

    <br /><br /><a href="#" onclick="PrintPage('GridTable')">Printer Friendly Format</a>
    <script type="text/javascript"> 
    function PrintPage(divID) { 
        var windowOptions = "toolbar=no,location=no,directories=yes,menubar=no,"; 
        windowOptions += "scrollbars=yes,width=750,height=600,left=100,top=25";
        //windowOptions = "";
        var printContent = document.getElementById(divID); 
        var printWindow = window.open("", "", windowOptions); 
        printWindow.document.write("<style>table { font-size: 8pt }</style>")
        printWindow.document.write(printContent.innerHTML);

        printWindow.document.close(); 
        printWindow.focus(); 
        printWindow.print(); 
    } 
</script> 
</asp:Content>
