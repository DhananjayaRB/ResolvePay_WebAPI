using Microsoft.EntityFrameworkCore;
using ResolvePay_WebAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResolvePay_WebAPI.Services
{
    public class DBContextPGSQL : DbContext
    {
        public DBContextPGSQL(DbContextOptions<DBContextPGSQL> options) : base(options)
        {
        }
        public DbSet<GetTestDetails_PGSQL> empmaster { get; set; }
    }
}
