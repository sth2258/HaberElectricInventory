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

        protected void Page_Init(object sender, EventArgs e)
        {

            // Create the dropdown list items.

            string[] categories = ("N/A," + ConfigurationManager.AppSettings["dd_Category"]).Split(',');
            string[] brands = ("N/A," + ConfigurationManager.AppSettings["dd_Brands"]).Split(',');
            AddItems(categories, dd_Category);
            AddItems(brands, dd_Brand);
        }

        private void AddItems(string[] items, DropDownList dd_lst)
        {
            foreach (string item in items)
            {
                dd_lst.Items.Add(new ListItem(item, item));
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                UpdateResults(true);
            }
        }

        protected void btn_email_Click(object sender, EventArgs e)
        {
           
            string subject = "Haber Electric Inventory List - " + DateTime.Now.ToShortDateString();
            string body = GetGridviewData(GridView1);
            Utils.SendMail(subject, body);
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

        protected void dd_Category_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateResults(false);
        }

        protected void cb_CriticalOnly_CheckedChanged(object sender, EventArgs e)
        {
            UpdateResults(false);
        }

        protected void UpdateResults(bool noFilter)
        {
            
            List<InventoryDAO> a = Inventory.GetAllProducts();
            
            a.Sort((emp1, emp2) => emp1.ItemDesc.CompareTo(emp2.ItemDesc));
            
            if (cb_CriticalOnly.Checked)
            {
                List<InventoryDAO> b = new List<InventoryDAO>();
                foreach (InventoryDAO z in a)
                {
                    if (z.CriticalInventoryItem)
                    {
                        b.Add(z);
                    }
                }
                a = b;
            }
            if (dd_Category.SelectedValue != "N/A")
            {
                List<InventoryDAO> b = new List<InventoryDAO>();
                foreach (InventoryDAO z in a)
                {
                    if (z.Category == dd_Category.SelectedValue)
                    {
                        b.Add(z);
                    }
                }
                a = b;
            }
            if (dd_Brand.SelectedValue != "N/A")
            {
                List<InventoryDAO> b = new List<InventoryDAO>();
                foreach (InventoryDAO z in a)
                {
                    if (z.Brand == dd_Brand.SelectedValue)
                    {
                        b.Add(z);
                    }
                }
                a = b;
            }

            if (a.Count == 0) return;
            GridView1.DataSource = a.Select(g => new { UPC = g.Upc, ItemName = g.ItemDesc, HECode = g.HaberCode, Quantity = g.Count, Category = g.Category, Brand = g.Brand, UpdateDate = g.UpdateDate });
            GridView1.DataBind();

            GridView1.HeaderRow.Cells[0].Text = "UPC";
            GridView1.HeaderRow.Cells[1].Text = "Item Description";
            GridView1.HeaderRow.Cells[2].Text = "Haber Electric Inventory Code";
            GridView1.HeaderRow.Cells[3].Text = "Quantity";
            GridView1.HeaderRow.Cells[4].Text = "Category";
            GridView1.HeaderRow.Cells[5].Text = "Brand";
            GridView1.HeaderRow.Cells[6].Text = "Updated Date";

            for (int aa = 0; aa < GridView1.Rows.Count; aa++)
            {
                DateTime real = new DateTime(long.Parse(GridView1.Rows[aa].Cells[6].Text));
                GridView1.Rows[aa].Cells[6].Text = real.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        protected void dd_Brand_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateResults(false);
        }
    }
}