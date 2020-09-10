using System;

namespace ResourcesAPI
{
    public class API
    {
        public string Key { get; private set; }

        public API(string key)
        {
            this.Key = key;
        }
    }
}
