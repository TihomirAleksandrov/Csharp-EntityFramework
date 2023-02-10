using MiniORM;
using MiniOrmApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniOrmApp.Data
{
    public class SoftUniDbContext : DbContext
    {
        public SoftUniDbContext(string connectionString) : base(connectionString)
        {

        }

        public DbSet<Employee> Employees { get; }

        public DbSet<Department> Departments { get; }

        public DbSet<Project> Projects { get; }

        public DbSet<EmployeeProject> EmployeeProjects { get; }
    }
}
