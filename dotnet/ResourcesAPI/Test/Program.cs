using ResourcesAPI;
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            API api = new API("keyhere");
            Console.WriteLine("Your credits left: " + api.GetApiCredits());
            Console.ReadLine();
        }
    }
}
