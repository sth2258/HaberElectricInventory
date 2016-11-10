using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System.Configuration;

namespace InventoryManagement
{
    public partial class _Default : Page
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
            upcCode.Focus();
            updatePannel.Visible = false;


            if (Page.IsPostBack)
            {
                if (upcCode.Text != string.Empty)
                {
                    Label1.Text = upcCode.Text;
                    InventoryDAO dao = Inventory.GetProductByUPC(upcCode.Text);

                    if (dao == null)
                    {
                        //try to get the product from the HE Code
                        InventoryDAO dao2 = Inventory.GetProductByHECode(upcCode.Text);
                        if (dao2 == null)
                        {
                            Label1.Text = "UPC or HE Code Not Found! Please add to inventory database!";
                        }
                        else
                        {
                            dao = dao2;
                            Label1.Text = dao.Upc;
                        }
                    }
                    if(dao != null)
                    {
                        updatePannel.Visible = true;
                        upd_count.Text = dao.Count.ToString();
                        upd_inventoryCode.Text = dao.HaberCode;
                        upd_itemDesc.Text = dao.ItemDesc;
                        if (dao.CriticalInventoryItem) cb_CriticalInventoryItem.Checked = true;
                        else cb_CriticalInventoryItem.Checked = false;
                        
                         DateTime a = new DateTime(dao.UpdateDate);
                        upd_lastUpdated.Text = a.ToString("yyyy-MM-dd HH:mm:ss");
                        upd_UPC.Text = dao.Upc;
                        dd_Category.SelectedValue = dao.Category;
                        dd_Category.Enabled = false;
                        dd_Category_SelectedIndexChanged(dd_Category, null);
                        
                        //this means something other than misc
                        if (pnl_Lighting.Visible)
                        {
                            dd_Brand.SelectedValue = dao.Brand;
                            dd_Color.SelectedValue = dao.Color;
                            dd_Wattage.SelectedValue = dao.Wattage;
                            dd_BulbType.SelectedValue = dao.BulbType;
                            dd_Length.SelectedValue = dao.Length;
                        }
                        Session["PreviousDAO"] = dao;
                    }
                    upcCode.Text = "";
                    upcCode.Focus();


                }
            }
        }

        protected void upd_UpdateButton_Click(object sender, EventArgs e)
        {
            InventoryDAO previousDAO = (InventoryDAO)Session["PreviousDAO"];
            bool updatedPerformed = false;
            int updatedCount = 0;
            if ((int.TryParse(upd_count.Text, out updatedCount)))
            {
                if (updatedCount != previousDAO.Count)
                {
                    Inventory.UpdateProduct(previousDAO.Upc, "Count", updatedCount);
                    updatedPerformed = true;
                }
            }
            else
            {
                Label1.Text = "Bad value inside the updated count! Try again.";
                updatePannel.Visible = false;
                return;
            }
            
            if(previousDAO.HaberCode != upd_inventoryCode.Text)
            {
                Inventory.UpdateProduct(previousDAO.Upc, "HaberInventoryCode", upd_inventoryCode.Text);
                updatedPerformed = true;
            }

            if(previousDAO.ItemDesc != upd_itemDesc.Text)
            {
                Inventory.UpdateProduct(previousDAO.Upc, "ItemDescription", upd_itemDesc.Text);
                updatedPerformed = true;
            }
            //brand color wattage bulbtype length criticalinventoryitem
            if (previousDAO.Brand != dd_Brand.SelectedValue)
            {
                Inventory.UpdateProduct(previousDAO.Upc, "Brand", dd_Brand.SelectedValue);
                updatedPerformed = true;
            }
            if (previousDAO.Color != dd_Color.SelectedValue)
            {
                Inventory.UpdateProduct(previousDAO.Upc, "Color", dd_Color.SelectedValue);
                updatedPerformed = true;
            }
            if (previousDAO.Wattage != dd_Wattage.SelectedValue)
            {
                Inventory.UpdateProduct(previousDAO.Upc, "Wattage", dd_Wattage.SelectedValue);
                updatedPerformed = true;
            }
            if (previousDAO.BulbType != dd_BulbType.SelectedValue)
            {
                Inventory.UpdateProduct(previousDAO.Upc, "BulbType", dd_BulbType.SelectedValue);
                updatedPerformed = true;
            }
            if (previousDAO.Length != dd_Length.SelectedValue)
            {
                Inventory.UpdateProduct(previousDAO.Upc, "Length", dd_Length.SelectedValue);
                updatedPerformed = true;
            }
            if (previousDAO.CriticalInventoryItem != cb_CriticalInventoryItem.Checked)
            {
                Inventory.UpdateProduct(previousDAO.Upc, "CriticalInventoryItem", cb_CriticalInventoryItem.Checked);
                updatedPerformed = true;
            }

            if (updatedPerformed)
            {
                Inventory.UpdateProduct(previousDAO.Upc, "Updated", DateTime.Now.Ticks);
            }
            //upd_itemName.Text
            //dao.UpdateDate
            Label1.Text = "Updates have been done!";

        }

        protected void upd_Minus1_Click(object sender, EventArgs e)
        {
            quickUpdate(-1);
        }

        protected void upd_Plus1_Click(object sender, EventArgs e)
        {
            quickUpdate(1);
        }

        protected void upd_Plus10_Click(object sender, EventArgs e)
        {
            quickUpdate(10);
        }

        protected void upd_Minus10_Click(object sender, EventArgs e)
        {
            quickUpdate(-10);
        }

        protected void quickUpdate(int amt)
        {
            InventoryDAO previousDAO = (InventoryDAO)Session["PreviousDAO"];
            if (previousDAO.Count + amt < 0)
            {
                Label1.Text = "Cannot remove that many! It would take your inventory below zero!";
            }
            else
            {
                Inventory.UpdateProduct(previousDAO.Upc, "Count", previousDAO.Count + amt);
                Inventory.UpdateProduct(previousDAO.Upc, "Updated", DateTime.Now.Ticks);
                if(amt < 0)
                {
                    Label1.Text = (amt * -1) + " item(s) have been removed from inventory!";
                }else
                {
                    Label1.Text = amt + " item(s) have been added to inventory!";
                }
            }
        }

        protected void btn_Delete_Click(object sender, EventArgs e)
        {
            InventoryDAO a = (InventoryDAO)Session["PreviousDAO"];
            InventoryDAO dao = Inventory.GetProductByUPC(a.Upc);
            Inventory.DeleteProduct(dao);

            Session["PreviousDAO"] = null;
            Label1.Text = a.Upc + " successfully deleted.";

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