using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;

namespace CoreService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "RealTimeUnitService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select RealTimeUnitService.svc or RealTimeUnitService.svc.cs at the Solution Explorer and start debugging.
    public class RealTimeUnitService : IRealTimeUnitService
    {
        private CspParameters csp;
        private RSACryptoServiceProvider rsa;

        const string IMPORT_FOLDER = @"C:\public_key\";
        const string PUBLIC_KEY_FILE = @"rsaPublicKey.txt";

        private static List<string> rtUnits = new List<string>();

        private static readonly object locker = new object();

        public RealTimeUnitService()
        {
            ImportPublicKey();
        }

        public bool AddRTU(string message, byte[] signature)
        {
            string[] tokens = message.Split(',');
            string id = tokens[0];
            string address = tokens[1];
            if (VerifySignedMessage(message, signature) && !rtUnits.Contains(id) && !RealTimeDriver.values.ContainsKey(address))
            {
                lock (locker)
                {
                    rtUnits.Add(id);
                    RealTimeDriver.values.Add(address, -1);
                }
                return true;
            }
            return false;
        }

        public void SendValue(string message, byte[] signature)
        {
            string[] tokens = message.Split(',');
            string address = tokens[0];
            double value = Double.Parse(tokens[1]);
            if (VerifySignedMessage(message, signature))
            {
                lock (locker)
                {
                    RealTimeDriver.values[address] = value;
                }
            }
        }

        private void ImportPublicKey()
        {
            string path = Path.Combine(IMPORT_FOLDER, PUBLIC_KEY_FILE);
            //Provera da li fajl sa javnim ključem postoji na prosleđenoj lokaciji
            FileInfo fi = new FileInfo(path);
            if (fi.Exists)
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    csp = new CspParameters();
                    rsa = new RSACryptoServiceProvider(csp);
                    string publicKeyText = reader.ReadToEnd();
                    rsa.FromXmlString(publicKeyText);
                }
            }
        }

        private bool VerifySignedMessage(string message, byte[] signature)
        {
            using (SHA256 sha = SHA256Managed.Create())
            {
                var hashValue = sha.ComputeHash(Encoding.UTF8.GetBytes(message));
                var deformatter = new RSAPKCS1SignatureDeformatter(rsa);
                deformatter.SetHashAlgorithm("SHA256");
                return deformatter.VerifySignature(hashValue, signature);
            }
        }
    }
}
