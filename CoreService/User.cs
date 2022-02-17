using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CoreService
{
    [DataContract]
    public class User
    {
        [Key]
        [DataMember]
        public string UserID { get; set; }
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string EncryptedPassword { get; set; }

        public User() { }

        public User(string username, string password)
        {
            Username = username;
            EncryptedPassword = password;
        }
    }
}