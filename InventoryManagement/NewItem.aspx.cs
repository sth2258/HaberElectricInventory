using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InventoryManagement
{
    public partial class NewItem : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            new_UPC.Focus();
        }

        protected void new_BtnAddItem_Click(object sender, EventArgs e)
        {
            int inventoryCnt = 0;
            if(!int.TryParse(new_count.Text, out inventoryCnt))
            {
                Label1.Text = "Bad value for initial inventory count!";
                return;
            }

            InventoryDAO dao = new InventoryDAO
            {
                Count = inventoryCnt,
                Upc = new_UPC.Text,
                ItemDesc = new_itemDesc.Text,
                ItemName = new_itemName.Text,
                HaberCode = new_inventoryCode.Text.ToUpper(),
                UpdateDate = DateTime.Now.Ticks

            };

            Inventory.PutProduct(dao);

            Label1.Text = "Item successfully added to inventory!";
            new_UPC.Text = "";
            new_itemName.Text = "";
            new_itemDesc.Text = "";
            new_inventoryCode.Text = "";
            new_count.Text = "";
            
        }
    }
}