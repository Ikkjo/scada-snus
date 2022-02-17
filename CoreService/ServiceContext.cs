using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CoreService
{
    public class ServiceContext: DbContext
    {
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Alarm> Alarms { get; set; }
    }
}