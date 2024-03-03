using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EmployeeDirectory.Classes;

namespace EmployeeDirectory.Context
{
    public class EmployeeDirectoryContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = AppsettingsParser.GetConnectionString();
            optionsBuilder.UseNpgsql(connectionString);
        }
    }
}
