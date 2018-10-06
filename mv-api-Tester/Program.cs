using System;
using mv_api;

namespace mv_api_Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            mvapi api = new mvapi();
            try
            {
                //string result = api.getAccessPermissions("", "google.be").Result;
                string result = api.postAccessToken("", "",mvapi.eGrant_type.password).Result;
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(ex);
            }

            Console.ReadKey();

        }
    }
}
