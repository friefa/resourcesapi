using ResourcesAPI;
using ResourcesAPI.Models.Factories;
using System;
using System.Collections;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            API api = new API("yourkeyhere");

            FactoryCollection collection = api.Factories;

            IEnumerator enumerator = collection.GetEnumerator();

            Console.WriteLine(api.Credits + " CREDITS");

            while (enumerator.MoveNext())
            {
                Factory item = enumerator.Current as Factory;

                Console.WriteLine(string.Format("{0}\t{1}\t{2}", item.FactoryID, item.FactoryName, item.ProductID));
            }

            Console.ReadLine();
        }
    }
}
