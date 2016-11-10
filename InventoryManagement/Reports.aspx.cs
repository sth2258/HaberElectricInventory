using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InventoryManagement
{
    public partial class Reports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<InventoryDAO> a = Inventory.GetAllProducts();
            a.Sort((emp1, emp2) => emp1.ItemDesc.CompareTo(emp2.ItemDesc));
            GridView1.DataSource = a.Select(g => new { UPC = g.Upc, ItemName = g.ItemDesc, HECode = g.HaberCode, Quantity = g.Count, UpdateDate = g.UpdateDate });
            GridView1.DataBind();

            GridView1.HeaderRow.Cells[0].Text = "UPC";
            GridView1.HeaderRow.Cells[1].Text = "Item Description";
            GridView1.HeaderRow.Cells[2].Text = "Haber Electric Inventory Code";
            GridView1.HeaderRow.Cells[3].Text = "Quantity";
            GridView1.HeaderRow.Cells[4].Text = "Updated Date";
            
            for (int aa = 0; aa < GridView1.Rows.Count; aa++)
            {
                DateTime real = new DateTime(long.Parse(GridView1.Rows[aa].Cells[4].Text));
                GridView1.Rows[aa].Cells[4].Text = real.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        protected void btn_email_Click(object sender, EventArgs e)
        {
            var fromAddress = new MailAddress("root@"+Environment.MachineName, "HE Inventory");
            string subject = "Haber Electric Inventory List - " + DateTime.Now.ToShortDateString();
            string body = GetGridviewData(GridView1);

            var smtp = new SmtpClient
            {
                Host = "localhost",
                Port = 25
            };
            var message = new MailMessage();
            message.From = fromAddress;
            //message.To.Add(new MailAddress("arthur@haberelectric.com"));
            //message.To.Add(new MailAddress("guy@haberelectric.com"));
            //message.To.Add(new MailAddress("jaclyn@haberelectric.com"));
            
            message.IsBodyHtml = true;
            message.Subject = subject;
            message.Body = body;
            smtp.Send(message);
        }
        public string GetGridviewData(GridView gv)
        {
            StringBuilder strBuilder = new StringBuilder();
            StringWriter strWriter = new StringWriter(strBuilder);
            HtmlTextWriter htw = new HtmlTextWriter(strWriter);
            gv.RenderControl(htw); 
            return strBuilder.ToString();
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

    }
}