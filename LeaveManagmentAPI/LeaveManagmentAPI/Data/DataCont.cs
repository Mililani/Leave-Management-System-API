using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace LeaveManagmentAPI.Models.Data
{
    public class DataCont : DbContext
    {
            public DataCont(DbContextOptions options) : base(options)
            {
            }
            public DbSet<User> Users { get; set; }
            public DbSet<Leave> LeaveRequest { get; set; }

        
    }
}
