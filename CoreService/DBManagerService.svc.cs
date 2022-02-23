using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace CoreService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DBManagerService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select DBManagerService.svc or DBManagerService.svc.cs at the Solution Explorer and start debugging.
    public class DBManagerService : IDBManagerService
    {
        private static Dictionary<string, User> authenticatedUsers = new Dictionary<string, User>();

        // Interface methods

        public bool AddTag(Tag newTag)
        {
            return TagProcessing.AddTag(newTag);
        }

        public bool ChangeOutputValue(string tagName, float newOutputValue)
        {
            return TagProcessing.ChangeOutputValue(tagName, newOutputValue);
        }

        public float? GetOutputValue(string tagName)
        {
            return TagProcessing.GetOutputValue(tagName);
        }

        public string LogIn(string username, string password)
        {
            addAdminIfNotPresent();

            using (var db = new UserContext())
            {
                foreach (var user in db.Users)
                {
                    if (username == user.Username &&
                    ValidateEncryptedData(password, user.EncryptedPassword))
                    {
                        string token = GenerateToken(username);
                        authenticatedUsers.Add(token, user);
                        return token;
                    }
                }
            }
            return "invalid";
        }

        public bool LogOut(string token)
        {
            return authenticatedUsers.Remove(token);
        }

        public bool RegisterUser(string username, string password)
        {
            if (DoesUserExist(username))
            {
                return false;
            } 

            string encryptedPassword = EncryptData(password);
            User user = new User(username, encryptedPassword);
            using (var db = new UserContext())
            {
                try
                {
                    user.UserID = db.Users.Count().ToString();
                    db.Users.Add(user);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            return true;
        }

        public bool RemoveTag(string tagName)
        {
            return TagProcessing.RemoveTag(tagName);
        }

        public bool SetScan(string tagName, bool scan)
        {
            return TagProcessing.SetScan(tagName, scan);
        }

        // Private methods

        private bool DoesUserExist(string username)
        {
            using (var db = new UserContext())
            {
                foreach (User u in db.Users) 
                {
                    if (u.Username == username)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        private void addAdminIfNotPresent()
        {
            using (var db = new UserContext())
            {
                bool found = false;

                foreach (User u in db.Users)
                {
                    if (u.Username == "admin")
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    RegisterUser("admin", "admin");
                }
            }
        }

        private static string EncryptData(string valueToEncrypt)
        {
            string GenerateSalt()
            {
                RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
                byte[] salt = new byte[32];
                crypto.GetBytes(salt);
                return Convert.ToBase64String(salt);
            }
            string EncryptValue(string strValue)
            {
                string saltValue = GenerateSalt();
                byte[] saltedPassword = Encoding.UTF8.GetBytes(saltValue + strValue);
                using (SHA256Managed sha = new SHA256Managed())
                {
                    byte[] hash = sha.ComputeHash(saltedPassword);
                    return $"{Convert.ToBase64String(hash)}:{saltValue}";
                }
            }
            return EncryptValue(valueToEncrypt);
        }

        private static bool ValidateEncryptedData(string valueToValidate, string valueFromDatabase)
        {
            string[] arrValues = valueFromDatabase.Split(':');
            string encryptedDbValue = arrValues[0];
            string salt = arrValues[1];
            byte[] saltedValue = Encoding.UTF8.GetBytes(salt + valueToValidate);
            using (var sha = new SHA256Managed())
            {
                byte[] hash = sha.ComputeHash(saltedValue);
                string enteredValueToValidate = Convert.ToBase64String(hash);
                return encryptedDbValue.Equals(enteredValueToValidate);
            }
        }

        private string GenerateToken(string username)
        {
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            byte[] randVal = new byte[32];
            crypto.GetBytes(randVal);
            string randStr = Convert.ToBase64String(randVal);
            return username + randStr;
        }


    }
}
