using MiniOrmApp.Data;
using MiniOrmApp.Data.Entities;
using System;
using System.Linq;

namespace MiniOrmApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var connectionString = "Server=LAPTOP-QV7V977H;Database=MiniORM;Trusted_Connection=True;";

            var context = new SoftUniDbContext(connectionString);

            context.Employees.Add(new Employee
            {
                FirstName = "Gosho",
                LastName = "Inserted",
                DepartmentId = context.Departments.First().Id,
                IsEmployed = true,
            });

            var employee = context.Employees.Last();
            employee.FirstName = "Modified";

            context.SaveChanges();
        }
    }
}
