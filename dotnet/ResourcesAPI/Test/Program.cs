using ResourcesAPI;
using ResourcesAPI.Localization;
using ResourcesAPI.Models.Production;
using System;
using System.Collections;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            API api = new API("yourkeyhere")
            {
                Language = Language.German
            };

            ProductionCollection collection = api.Productions;

            IEnumerator enumerator = collection.GetEnumerator();

            Console.WriteLine(api.Credits + " CREDITS");

            while (enumerator.MoveNext())
            {
                Production item = enumerator.Current as Production;

                Console.WriteLine(string.Format("{0}\t{1}\t{2}", item.ItemName, item.FactoryName, item.BaseOutputPerHour));
            }

            Console.ReadLine();
        }
    }
}
