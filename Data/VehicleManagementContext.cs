using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VehicleManagement.Models;

namespace VehicleManagement.Data
{
    public class VehicleManagementContext : DbContext
    {
        public VehicleManagementContext (DbContextOptions<VehicleManagementContext> options)
            : base(options)
        {
        }

        public DbSet<VehicleManagement.Models.VehicleViewModel> VehicleViewModel { get; set; } = default!;
    }
}
