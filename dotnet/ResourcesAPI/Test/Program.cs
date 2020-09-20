using ResourcesAPI;
using ResourcesAPI.Models;
using ResourcesAPI.Models.Items;
using System;
using System.Collections;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            API api = new API("yourkeyhere");

            ItemCollection collection = api.Items;

            IEnumerator enumerator = collection.GetEnumerator();

            Console.WriteLine(api.Credits + " CREDITS");

            while (enumerator.MoveNext())
            {
                Item item = enumerator.Current as Item;

                Console.WriteLine(string.Format("{0}\t{1}\t{2}", item.ItemId, item.Name, item.IconUrl));
            }

            Console.ReadLine();
        }
    }
}
