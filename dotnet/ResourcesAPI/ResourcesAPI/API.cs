using Newtonsoft.Json;
using ResourcesAPI.Models;
using System.IO;
using System.Net;

namespace ResourcesAPI
{
    public class API
    {
        public string Key { get; private set; }

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

        public int GetApiCredits()
        {
            Query query = new Query(QueryType.ApiCredits, OutputType.JSON, Language.German);
            string str = this.Request(query);

            dynamic dyn = JsonConvert.DeserializeObject(str);

            return dyn.creditsleft;
        }
    }
}
