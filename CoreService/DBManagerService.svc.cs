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
        private static Dictionary<string, Tag> tags = new Dictionary<string, Tag>();
        private static Dictionary<string, Alarm> alarms = new Dictionary<string, Alarm>();

        // Interface methods

        public bool AddTag(Tag newTag)
        {
            if(!DoesTagExist(newTag.TagName))
            {
                using (var db = new TagContext())
                {
                    db.Tags.Add(newTag);
                    db.SaveChanges();
                    tags.Add(newTag.TagName, newTag);
                    return true;
                }
            }
            return false;
            
        }

        public bool ChangeOutputValue(string tagName, float newOutputValue)
        {
            if(DoesTagExist(tagName))
            {
                try
                {
                    OutputTag t = (OutputTag) tags[tagName];


                } catch (System.InvalidCastException e)
                {
                    return false;
                }                
                
            }
            return false;
        }

        public float GetOutputValue(string tagName)
        {
            throw new NotImplementedException();
        }

        public string LogIn(string username, string password)
        {
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
            return "Login failed";
        }

        public bool LogOut(string token)
        {
            return authenticatedUsers.Remove(token);
        }

        public bool RegisterUser(string username, string password)
        {
            string encryptedPassword = EncryptData(password);
            User user = new User(username, encryptedPassword);
            using (var db = new UserContext())
            {
                try
                {
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
            throw new NotImplementedException();
        }

        public bool SetScan(string tagName, bool scan)
        {
            throw new NotImplementedException();
        }

        // Private methods

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

        private bool DoesTagExist(string tagName)
        {
            return tags.ContainsKey(tagName);
        }

    }
}
