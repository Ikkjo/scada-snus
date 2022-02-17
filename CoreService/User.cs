using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CoreService
{
    public class User
    {
        [Key]
        public string UserID { get; set; }
        public string Username { get; set; }
        public string HashedPassword { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string email { get; set; }

        public User() { }
    }
}