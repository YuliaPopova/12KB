using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server_Miner
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceHost = new ServiceHost(typeof(Service1));
            serviceHost.Open();
            Console.WriteLine("Miner is running...");
            Console.WriteLine("Press any key to stop");
            Console.ReadKey();
            serviceHost.Close();
            Console.WriteLine("Service was stopped");
        }
    }
}
