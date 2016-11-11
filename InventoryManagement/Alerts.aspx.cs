using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InventoryManagement
{
    public partial class Alerts : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                LoadResults(true);
            }
        }

        protected void LoadResults(bool noFilter)
        {

            List<InventoryDAO> a = Inventory.GetAllProducts();
            /*foreach(var item in a)
            {
                Label label = new Label();
                label.Text = item.Category+" - "+item.Upc + " - " + item.ItemDesc;

                

                Literal br = new Literal();
                br.Text = "<br />";
                Panel1.Controls.Add(label);
                Panel1.Controls.Add(new Literal { Text = "<span style='padding-left:25px'>Lower Bound: </span>"});
                Panel1.Controls.Add(br);
            }*/

            Session["PreAlertUpdate"] = a;
            GridView1.DataSource = a.Where(g=> g.CriticalInventoryItem == true).Select(g => new { UPC = g.Upc, ItemName = g.ItemDesc, Category = g.Category, Quantity = g.Count, LowerBound = g.LowerBound, UpperBound = g.UpperBound });
            GridView1.DataBind();

            //GridView1.HeaderRow.Cells[0].Text = "UPC";
            //GridView1.HeaderRow.Cells[1].Text = "Item Description";
            //GridView1.HeaderRow.Cells[2].Text = "Haber Electric Inventory Code";
            //GridView1.HeaderRow.Cells[3].Text = "Quantity";
            //GridView1.HeaderRow.Cells[4].Text = "Category";
            //GridView1.HeaderRow.Cells[5].Text = "Updated Date";

            //for (int aa = 0; aa < GridView1.Rows.Count; aa++)
            //{
            //    DateTime real = new DateTime(long.Parse(GridView1.Rows[aa].Cells[5].Text));
            //    GridView1.Rows[aa].Cells[5].Text = real.ToString("yyyy-MM-dd HH:mm:ss");
            //}
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            List<InventoryDAO> a = (List <InventoryDAO >)Session["PreAlertUpdate"];
            Literal1.Text = "";
            foreach (GridViewRow row in GridView1.Rows)
            {
                string upc = ((Label)row.Cells[0].Controls[1]).Text;
                string lowerBound = ((TextBox)row.Cells[3].Controls[1]).Text;
                string upperBound = ((TextBox)row.Cells[4].Controls[1]).Text;

                InventoryDAO obj = (a.Where(g => g.Upc == upc)).First();
                bool updatesMade = false;
                if (obj.UpperBound == null && upperBound != "")
                {
                    Inventory.UpdateProduct(upc, "UpperBound", upperBound);
                    updatesMade = true;
                }
                else if (obj.UpperBound != null)
                {
                    if (obj.UpperBound.ToString() != upperBound)
                    {
                        Inventory.UpdateProduct(upc, "UpperBound", upperBound);
                        updatesMade = true;
                    }
                }


                if (obj.LowerBound == null && lowerBound != "")
                {
                    Inventory.UpdateProduct(upc, "LowerBound", lowerBound);
                    updatesMade = true;
                }
                else if (obj.LowerBound != null)
                {
                    if (obj.LowerBound.ToString() != lowerBound)
                    {
                        Inventory.UpdateProduct(upc, "LowerBound", lowerBound);
                        updatesMade = true;
                    }
                }
                if (updatesMade)
                {
                    Literal1.Text += "Updates performed to UPC " + upc + "<br />";
                }
            }
            Session["PreAlertUpdate"] = null;
        }
    }
}