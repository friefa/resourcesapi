using Newtonsoft.Json.Linq;
using ResourcesAPI.Localization;
using ResourcesAPI.Models.Factories;
using ResourcesAPI.Models.Items;
using ResourcesAPI.Models.Production;
using ResourcesAPI.Models.Query;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace ResourcesAPI
{
    public class API
    {
        public string Key { get; private set; }

        public int Credits 
        { 
            get
            {
                Query query = new Query(QueryType.ApiCredits, OutputType.JSON, Language.English);
                string str = this.Request(query);

                dynamic dyn = JArray.Parse(str);
                return dyn[0]["creditsleft"];
            }
        }

        public Language Language
        {
            get;
            set;
        } = Language.English;

        private ItemCollection items = null;

        private FactoryCollection factories = null;

        private ProductionCollection productions = null;

        public API(string key)
        {
            this.Key = key;
        }

        public string Request(Query query)
        {
            WebRequest request = WebRequest.Create(query.GetUri(this.Key));
            WebResponse response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader streamReader = new StreamReader(responseStream);

            string str = streamReader.ReadToEnd();

            streamReader.Close();
            responseStream.Close();
            response.Close();

            return str;
        }

        public ItemCollection Items
        {
            get
            {
                if (this.items == null)
                {
                    Query query = new Query(QueryType.ItemCollection, OutputType.JSON, this.Language);
                    string str = this.Request(query);

                    dynamic dyn = JArray.Parse(str);

                    Item[] arr = new Item[dyn.Count];

                    for (int i = 0; i < dyn.Count; i++)
                    {
                        string _id = dyn[i]["itemID"];

                        ushort id = ushort.Parse(_id);
                        string name = dyn[i]["name_" + this.Language.Identifier];
                        string icon_url = dyn[i]["iconURL"];

                        icon_url = icon_url.Replace(@"\/", "/");

                        arr[i] = new Item(id, name, icon_url);
                    }

                    this.items = new ItemCollection(arr);
                }

                return this.items;
            }
            set
            {
                this.items = value;
            }
        }

        public FactoryCollection Factories
        {
            get
            {
                if (this.factories == null)
                {
                    Query query = new Query(QueryType.FactoryUpgradeData, OutputType.JSON, this.Language);
                    string str = this.Request(query);

                    dynamic dyn = JArray.Parse(str);

                    Factory[] arr = new Factory[dyn.Count];

                    for (int i = 0; i < dyn.Count; i++)
                    {
                        string _id = dyn[i]["factoryID"];
                        ushort id = ushort.Parse(_id);

                        string name = dyn[i]["factoryName"];

                        string _pid = dyn[i]["productID"];
                        ushort pid = ushort.Parse(_pid);

                        string _baseUpgradeCost = dyn[i]["baseUpgCost"];
                        int baseUpgradeCost = int.Parse(_baseUpgradeCost);

                        string _minUserLevel = dyn[i]["minUsrLvl"];
                        ushort minUserLevel = ushort.Parse(_minUserLevel);

                        List<FactoryUpgradeElement> elements = new List<FactoryUpgradeElement>();
                        string _buffer = null;
                        int count = 1;

                        do
                        {
                            try 
                            {   
                                _buffer = dyn[i]["baseUpgItemID" + count];
                                string buffer = dyn[i]["baseUpgItemQty" + count];

                                ushort upgradeId = ushort.Parse(_buffer);
                                int upgradeQuantity = int.Parse(buffer);

                                elements.Add(new FactoryUpgradeElement(upgradeId, upgradeQuantity));
                            }
                            catch { _buffer = null; }

                            count++;
                        }
                        while (_buffer != null);

                        arr[i] = new Factory(id, name, pid, baseUpgradeCost, minUserLevel, elements.ToArray());
                    }

                    this.factories = new FactoryCollection(arr);
                }

                return this.factories;
            }
            set
            {
                this.factories = value;
            }
        }

        public ProductionCollection Productions
        {
            get
            {
                if (this.productions == null)
                {
                    Query query = new Query(QueryType.ItemProductionBaseData, OutputType.JSON, this.Language);
                    string str = this.Request(query);

                    dynamic dyn = JArray.Parse(str);

                    Production[] arr = new Production[dyn.Count];

                    for (int i = 0; i < dyn.Count; i++)
                    {
                        string _itemId = dyn[i]["itemID"];
                        ushort itemId = ushort.Parse(_itemId);

                        string itemName = dyn[i]["itemName"];

                        string _factoryId = dyn[i]["factoryID"];
                        ushort factoryId = ushort.Parse(_factoryId);

                        string factoryName = dyn[i]["factoryName"];

                        string _baseOutputPerHour = dyn[i]["baseOutputPerHour"];
                        int baseOutputPerHour = int.Parse(_baseOutputPerHour);

                        string _outputPerCycle = dyn[i]["outputPerCycle"];
                        int outputPerCycle = int.Parse(_outputPerCycle);

                        string _creditsPerCycle = dyn[i]["creditsPerCycle"];
                        int creditsPerCycle = int.Parse(_creditsPerCycle);

                        List<ProductionIngredient> elements = new List<ProductionIngredient>();
                        string _buffer = null;
                        int count = 1;

                        do
                        {
                            try
                            {
                                _buffer = dyn[i]["itemID" + count];
                                string buffer = dyn[i][string.Format("item{0}QtyPerCycle", count)];

                                ushort ingredientItemId = ushort.Parse(_buffer);
                                int ingredientPerCycle = int.Parse(buffer);

                                elements.Add(new ProductionIngredient(ingredientItemId, ingredientPerCycle));
                            }
                            catch { _buffer = null; }

                            count++;
                        }
                        while (_buffer != null);

                        arr[i] = new Production(itemId, itemName, factoryId, factoryName, baseOutputPerHour, outputPerCycle, creditsPerCycle, elements.ToArray());
                    }

                    this.productions = new ProductionCollection(arr);
                }

                return this.productions;
            }
            set
            {
                this.productions = value;
            }
        }
    }
}
