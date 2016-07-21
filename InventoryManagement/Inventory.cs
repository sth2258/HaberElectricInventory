using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace InventoryManagement
{
    public static class Inventory
    {

        public static List<InventoryDAO> GetAllProducts()
        {
            AmazonDynamoDBClient client = new AmazonDynamoDBClient();

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
                        case "ItemName":
                            dao.ItemName = row.Value.S;
                            break;
                        case "UPC":
                            dao.Upc = row.Value.S;
                            break;
                        case "Updated":
                            dao.UpdateDate = long.Parse(row.Value.N);
                            break;

                    }
                }
                list.Add(dao);
            }


            return list;
        }

        public static InventoryDAO GetProduct(string text)
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
                    case "ItemName":
                        dao.ItemName = row.Value.S;
                        break;
                    case "UPC":
                        dao.Upc = row.Value.S;
                        break;
                    case "Updated":
                        dao.UpdateDate = long.Parse(row.Value.N);
                        break;

                }
            }

            return dao;

        }

        internal static void PutProduct(InventoryDAO dao)
        {
            AmazonDynamoDBClient client = new AmazonDynamoDBClient();
            client.Config.RegionEndpoint = Amazon.RegionEndpoint.USEast1;
            string tableName = "HaberElectricInventory";
            var request = new PutItemRequest
            {
                TableName = tableName,
                Item = new Dictionary<string,AttributeValue>(){
                    { "UPC", new AttributeValue { S = dao.Upc } },
                    { "Count", new AttributeValue { N = dao.Count.ToString() } },
                    { "HaberInventoryCode", new AttributeValue { S = dao.HaberCode } },
                    { "ItemName", new AttributeValue { S = dao.ItemName } },
                    { "ItemDescription", new AttributeValue { S = dao.ItemDesc } },
                    { "Updated", new AttributeValue { N = dao.UpdateDate.ToString() } }
                }
            };
            var response = client.PutItem(request);
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
        private string itemName;
        private string itemDesc;
        private string haberCode;
        private int count;
        private long updateDate;

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

        public string ItemName
        {
            get
            {
                return itemName;
            }

            set
            {
                itemName = value;
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
    }
}