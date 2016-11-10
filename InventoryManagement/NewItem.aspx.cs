using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InventoryManagement
{
    public partial class NewItem : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {

            // Create the dropdown list items.
            string[] brands = ConfigurationManager.AppSettings["dd_Brands"].Split(',');
            string[] categories = ConfigurationManager.AppSettings["dd_Category"].Split(',');
            string[] colors = ConfigurationManager.AppSettings["dd_Colors"].Split(',');
            string[] wattage = ConfigurationManager.AppSettings["dd_Wattage"].Split(',');
            string[] bulbType = ConfigurationManager.AppSettings["dd_BulbType"].Split(',');
            string[] length = ConfigurationManager.AppSettings["dd_Length"].Split(',');

            AddItems(brands, dd_Brand);
            AddItems(categories, dd_Category);
            AddItems(colors, dd_Color);
            AddItems(wattage, dd_Wattage);
            AddItems(bulbType, dd_BulbType);
            AddItems(length, dd_Length);


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
            new_UPC.Focus();
            dd_Category_SelectedIndexChanged(dd_Category, null);
        }

        protected void new_BtnAddItem_Click(object sender, EventArgs e)
        {
            int inventoryCnt = 0;
            if (!int.TryParse(new_count.Text, out inventoryCnt))
            {
                Label1.Text = "Bad value for initial inventory count!";
                return;
            }

            //These are the required fields
            InventoryDAO dao = new InventoryDAO
            {
                Count = inventoryCnt,
                Upc = new_UPC.Text,
                ItemDesc = new_itemDesc.Text,
                HaberCode = new_inventoryCode.Text.ToUpper(),
                CriticalInventoryItem = cb_CriticalInventoryItem.Checked,
                UpdateDate = DateTime.Now.Ticks,
                Category = dd_Category.SelectedValue,
        };

            //Now the optional fields
            if (Inventory.IsVisibleItemSet(dd_Category.SelectedValue))
            {
                dao.Brand = dd_Brand.SelectedValue;
                dao.Color = dd_Color.SelectedValue;
                dao.Wattage = dd_Wattage.SelectedValue;
                dao.BulbType = dd_BulbType.SelectedValue;
                dao.Length = dd_Length.SelectedValue;
            }


            Inventory.PutProduct(dao);

            Label1.Text = "Item successfully added to inventory!";
            new_UPC.Text = "";
            new_itemDesc.Text = "";
            new_inventoryCode.Text = "";
            new_count.Text = "";

        }

        protected void dd_Category_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList list = (DropDownList)sender;
            if (Inventory.IsVisibleItemSet(list.SelectedValue))
            {
                pnl_Lighting.Visible = true;
            }
            else
            {
                pnl_Lighting.Visible = false;
            }
        }
    }
}