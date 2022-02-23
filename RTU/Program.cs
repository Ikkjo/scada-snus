using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RTU
{
    class Program
    {
        private static CspParameters csp;
        private static RSACryptoServiceProvider rsa;

        const string EXPORT_FOLDER = @"C:\public_key\";
        const string PUBLIC_KEY_FILE = @"rsaPublicKey.txt";

        private static RTUServiceReference.RealTimeUnitServiceClient proxy = new RTUServiceReference.RealTimeUnitServiceClient();

        static void Main(string[] args)
        {
            Program program = new Program();
            CreateAsmKeys();
            ExportPublicKey();
            program.AddRealTimeUnit();
        }

        private void AddRealTimeUnit()
        {
            while (true)
            {
                Console.WriteLine("ADD RTU\n\n");
                string id = GetTagDataFromUser("ID");
                double lowLimit = double.Parse(GetTagDataFromUser("dnju granicu"));
                double highLimit = double.Parse(GetTagDataFromUser("gornju granicu"));
                string address = GetTagDataFromUser("Address");

                string message = id + "," + address + "," + lowLimit + "," + highLimit;
                byte[] signature = SignMessage(message);

                if (!proxy.AddRTU(message, signature))
                {
                    Console.WriteLine("Greška pri dodavanju RTU");
                    Console.WriteLine("Pritisni bilo koji taster za nastavak ili 'x' za izlazak");
                    switch (Console.ReadLine().ToLower())
                    {
                        case "x":
                            System.Environment.Exit(0);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Uspešno dodat RTU");
                    Console.WriteLine("Pritisni bilo koji taster za slanje vrednosti na ulaze...");
                    Console.ReadKey();

                    Console.WriteLine("Slanje vrednosti u toku...");
                    while (true)
                    {
                        double value = new Random().NextDouble() * (highLimit - lowLimit) + lowLimit;
                        Console.WriteLine($"Vrednost: {value}, poslata na adresu: {address}");
                        message = address + "," + value;
                        signature = SignMessage(message);
                        proxy.SendValue(message, signature);
                        Thread.Sleep(3000);
                    }
                }
            }

        }

        private static string GetTagDataFromUser(string description)
        {
            System.Console.WriteLine($"Unesi {description}:");
            string data = System.Console.ReadLine();

            return data == "" ? null : data;
        }

        public static void CreateAsmKeys()
        {
            csp = new CspParameters();
            rsa = new RSACryptoServiceProvider(csp);
        }

        private static byte[] SignMessage(string message)
        {
            using (SHA256 sha = SHA256Managed.Create())
            {
                var hashValue = sha.ComputeHash(Encoding.UTF8.GetBytes(message));
                var formatter = new RSAPKCS1SignatureFormatter(rsa);
                formatter.SetHashAlgorithm("SHA256");
                return formatter.CreateSignature(hashValue);
            }
        }


        private static void ExportPublicKey()
        {
            //Kreiranje foldera za eksport ukoliko on ne postoji
            if (!(Directory.Exists(EXPORT_FOLDER)))
                Directory.CreateDirectory(EXPORT_FOLDER);
            string path = Path.Combine(EXPORT_FOLDER, PUBLIC_KEY_FILE);
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.Write(rsa.ToXmlString(false));
            }
        }

        private static void DisplayMessage(string message, bool keyToContinue = false)
        {
            Console.WriteLine(message);
            if (keyToContinue)
            {
                Console.WriteLine("Pritisni bilo koji taster za nastavak...");
                Console.ReadKey();
            }
        }
    }
}
