using Newtonsoft.Json.Linq;
using ResourcesAPI.Models;
using ResourcesAPI.Models.Factories;
using ResourcesAPI.Models.Items;
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

                        List<FactoryUpgradeBaseElement> elements = new List<FactoryUpgradeBaseElement>();
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

                                elements.Add(new FactoryUpgradeBaseElement(upgradeId, upgradeQuantity));
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
    }
}
