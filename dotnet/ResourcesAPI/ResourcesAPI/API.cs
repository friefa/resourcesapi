using Newtonsoft.Json.Linq;
using ResourcesAPI.Models;
using ResourcesAPI.Models.Items;
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

        public API(string key)
        {
            this.Key = key;
        }

        private ItemCollection items = null;

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
                        string test = dyn[i]["itemID"];

                        ushort id = ushort.Parse(test);
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
    }
}
