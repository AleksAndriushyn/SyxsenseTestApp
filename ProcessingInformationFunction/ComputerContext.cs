using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace ProcessingInformationFunction
{
    public class ComputerContext : DbContext
    {   
        public ComputerContext(DbContextOptions<ComputerContext> options)
            : base(options)
        { }

        public DbSet<ComputerInfoModel> ComputerInfoModels { get; set; }
    }
}
