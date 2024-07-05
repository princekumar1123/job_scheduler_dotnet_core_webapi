using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Job_Scheduler_webAPI.Models;

namespace Job_Scheduler_webAPI.Data
{
    public class Job_Scheduler_webAPIContext : DbContext
    {
        public Job_Scheduler_webAPIContext (DbContextOptions<Job_Scheduler_webAPIContext> options)
            : base(options)
        {
        }


        //public Job_Scheduler_webAPIContext()
        //{

        //}

        public DbSet<Job_Scheduler_webAPI.Models.Countries> Countries { get; set; } = default!;
    }
}
