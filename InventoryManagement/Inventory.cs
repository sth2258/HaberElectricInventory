using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace InventoryManagement
{
    public static class Inventory
    {
        public static bool IsVisibleItemSet(string selectedCategory)
        {
            if (new List<string>(ConfigurationManager.AppSettings["cfg_ValsThatTriggerDropdowns"].ToString().Split(',')).Contains(selectedCategory))
            {
                return true;
            }
            return false;
        }
        public static List<InventoryDAO> GetAllProducts()
        {
            AmazonDynamoDBClient client = new AmazonDynamoDBClient(Amazon.RegionEndpoint.USEast1);

            // Modify the client so that it accesses a different region.
            client.Config.RegionEndpoint = Amazon.RegionEndpoint.USEast1;
            
            string tableName = "HaberElectricInventory";
            var request = new ScanRequest
            {
                TableName = tableName
            };

            var response = client.Scan(request);
            var result = response.Items;

            List<InventoryDAO> list = new List<InventoryDAO>();

            foreach(var item in result)
            {
                InventoryDAO dao = new InventoryDAO();
                foreach (var row in item)
                {
                    switch (row.Key)
                    {
                        case "Count":
                            dao.Count = int.Parse(row.Value.N);
                            break;
                        case "HaberInventoryCode":
                            dao.HaberCode = row.Value.S;
                            break;
                        case "ItemDescription":
                            dao.ItemDesc = row.Value.S;
                            break;
                        case "UPC":
                            dao.Upc = row.Value.S;
                            break;
                        case "Updated":
                            dao.UpdateDate = long.Parse(row.Value.N);
                            break;
                        case "Brand":
                            dao.Brand = row.Value.S;
                            break;
                        case "BulbType":
                            dao.BulbType = row.Value.S;
                            break;
                        case "Category":
                            dao.Category = row.Value.S;
                            break;
                        case "Color":
                            dao.Color = row.Value.S;
                            break;
                        case "CriticalInventoryItem":
                            dao.CriticalInventoryItem = row.Value.BOOL;
                            break;
                        case "Wattage":
                            dao.Wattage = row.Value.S;
                            break;
                        case "Length":
                            dao.Length = row.Value.S;
                            break;
                    }
                }
                list.Add(dao);
            }


            return list;
        }

        public static InventoryDAO GetProductByUPC(string text)
        {
            AmazonDynamoDBClient client = new AmazonDynamoDBClient();

            // Modify the client so that it accesses a different region.
            client.Config.RegionEndpoint = Amazon.RegionEndpoint.USEast1;

            string tableName = "HaberElectricInventory";

            var request = new GetItemRequest
            {
                TableName = tableName,
                Key = new Dictionary<string, AttributeValue>() { { "UPC", new AttributeValue { S = text } } },
            };
            var response = client.GetItem(request);

            // Check the response.
            var result = response.Item;
            //var attributeMap = result.Item; // Attribute list in the response.
            if (result.Count == 0)
            {
                return null;
            }
            InventoryDAO dao = new InventoryDAO();
            foreach (var row in result)
            {
                switch (row.Key)
                {
                    case "Count":
                        dao.Count = int.Parse(row.Value.N);
                        break;
                    case "HaberInventoryCode":
                        dao.HaberCode = row.Value.S;
                        break;
                    case "ItemDescription":
                        dao.ItemDesc = row.Value.S;
                        break;
                    case "UPC":
                        dao.Upc = row.Value.S;
                        break;
                    case "Updated":
                        dao.UpdateDate = long.Parse(row.Value.N);
                        break;
                    case "Brand":
                        dao.Brand = row.Value.S;
                        break;
                    case "BulbType":
                        dao.BulbType = row.Value.S;
                        break;
                    case "Category":
                        dao.Category = row.Value.S;
                        break;
                    case "Color":
                        dao.Color = row.Value.S;
                        break;
                    case "CriticalInventoryItem":
                        dao.CriticalInventoryItem = row.Value.BOOL;
                        break;
                    case "Wattage":
                        dao.Wattage = row.Value.S;
                        break;
                    case "Length":
                        dao.Length = row.Value.S;
                        break;

                }
            }

            return dao;

        }
        public static InventoryDAO GetProductByHECode(string text)
        {
            List<InventoryDAO> a = Inventory.GetAllProducts();
            foreach (InventoryDAO b in a)
            {
                if (b.HaberCode.ToUpper() == text.ToUpper())
                {
                    return b;
                }
            }
            return null;
                
        }

        internal static void PutProduct(InventoryDAO dao)
        {
            AmazonDynamoDBClient client = new AmazonDynamoDBClient(Amazon.RegionEndpoint.USEast1);
            client.Config.RegionEndpoint = Amazon.RegionEndpoint.USEast1;
            string tableName = "HaberElectricInventory";
            Dictionary<string, AttributeValue> item = new Dictionary<string, AttributeValue>();
            item.Add("UPC", new AttributeValue { S = dao.Upc });
            item.Add("Count", new AttributeValue { N = dao.Count.ToString() });
            item.Add("HaberInventoryCode", new AttributeValue { S = dao.HaberCode });
            item.Add("ItemDescription", new AttributeValue { S = dao.ItemDesc });
            item.Add("Updated", new AttributeValue { N = dao.UpdateDate.ToString() });
            item.Add("CriticalInventoryItem", new AttributeValue { BOOL = dao.CriticalInventoryItem });
            item.Add("Category", new AttributeValue { S = dao.Category });
            //and for the optionals...
            if (IsVisibleItemSet(dao.Category))
            {
                item.Add("Brand", new AttributeValue { S = dao.Brand });
                item.Add("BulbType", new AttributeValue { S = dao.BulbType });
                item.Add("Color", new AttributeValue { S = dao.Color });
                item.Add("Wattage", new AttributeValue { S = dao.Wattage });
                item.Add("Length", new AttributeValue { S = dao.Length });
            }

            var request = new PutItemRequest
            {
                TableName = tableName,
                Item = item
            };
            var response = client.PutItem(request);
        }

        internal static void DeleteProduct(InventoryDAO dao)
        {
            AmazonDynamoDBClient client = new AmazonDynamoDBClient();
            client.Config.RegionEndpoint = Amazon.RegionEndpoint.USEast1;
            string tableName = "HaberElectricInventory";
            var request = new DeleteItemRequest
            {
                TableName = tableName,
                Key = new Dictionary<string, AttributeValue>() { { "UPC", new AttributeValue { S = dao.Upc } } },
            };
            var response = client.DeleteItem(request);
            Debug.WriteLine("Delete performed to " + dao.Upc);
        }


        internal static void UpdateProduct(string index, string attr, bool value)
        {
            Dictionary<string, AttributeValueUpdate> update = new Dictionary<string, AttributeValueUpdate>() { { attr, new AttributeValueUpdate { Action = AttributeAction.PUT, Value = new AttributeValue { BOOL = value } } } };
            performUpdate(index, update);
        }

        internal static void UpdateProduct(string index, string attr, int value)
        {
            Dictionary<string, AttributeValueUpdate> update = new Dictionary<string, AttributeValueUpdate>() { { attr, new AttributeValueUpdate { Action = AttributeAction.PUT, Value = new AttributeValue { N = value.ToString() } } } };
            performUpdate(index, update);
        }

        internal static void UpdateProduct(string index, string attr, long value)
        {
            Dictionary<string, AttributeValueUpdate> update = new Dictionary<string, AttributeValueUpdate>() { { attr, new AttributeValueUpdate { Action = AttributeAction.PUT, Value = new AttributeValue { N = value.ToString() } } } };
            performUpdate(index, update);
        }

        internal static void UpdateProduct(string index, string attr, string value)
        {
            Dictionary<string, AttributeValueUpdate> update = new Dictionary<string, AttributeValueUpdate>() { { attr, new AttributeValueUpdate { Action = AttributeAction.PUT, Value = new AttributeValue { S = value.ToString() } } } };
            performUpdate(index, update);
        }

        private static void performUpdate(string upc, Dictionary<string, AttributeValueUpdate> update)
        {
            AmazonDynamoDBClient client = new AmazonDynamoDBClient();
            client.Config.RegionEndpoint = Amazon.RegionEndpoint.USEast1;
            string tableName = "HaberElectricInventory";
            var request = new UpdateItemRequest
            {
                TableName = tableName,
                Key = new Dictionary<string, AttributeValue>() { { "UPC", new AttributeValue { S = upc } } },
                AttributeUpdates = update
            };
            var response = client.UpdateItem(request);
            Debug.WriteLine("Update performed to " + upc);
        }
    }

    public class InventoryDAO
    {
        private string upc;
        
        private string itemDesc;
        private string haberCode;
        private int count;
        private long updateDate;
        private bool criticalInventoryItem;
        private string category;
        private string brand;
        private string color;
        private string wattage;
        private string bulbType;
        private string length;

        public string Upc
        {
            get
            {
                return upc;
            }

            set
            {
                upc = value;
            }
        }

        

        public string ItemDesc
        {
            get
            {
                return itemDesc;
            }

            set
            {
                itemDesc = value;
            }
        }

        public string HaberCode
        {
            get
            {
                return haberCode;
            }

            set
            {
                haberCode = value;
            }
        }

        public int Count
        {
            get
            {
                return count;
            }

            set
            {
                count = value;
            }
        }

        public long UpdateDate
        {
            get
            {
                return updateDate;
            }

            set
            {
                updateDate = value;
            }
        }

        public bool CriticalInventoryItem
        {
            get
            {
                return criticalInventoryItem;
            }

            set
            {
                criticalInventoryItem = value;
            }
        }

        public string Category
        {
            get
            {
                return category;
            }

            set
            {
                category = value;
            }
        }

        public string Brand
        {
            get
            {
                return brand;
            }

            set
            {
                brand = value;
            }
        }

        public string Color
        {
            get
            {
                return color;
            }

            set
            {
                color = value;
            }
        }

        public string Wattage
        {
            get
            {
                return wattage;
            }

            set
            {
                wattage = value;
            }
        }

        public string BulbType
        {
            get
            {
                return bulbType;
            }

            set
            {
                bulbType = value;
            }
        }

        public string Length
        {
            get
            {
                return length;
            }

            set
            {
                length = value;
            }
        }
    }
}