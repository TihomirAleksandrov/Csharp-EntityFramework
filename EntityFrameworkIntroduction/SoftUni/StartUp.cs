using SoftUni.Data;
using System;
using System.Linq;
using System.Text;

namespace SoftUni
{
    internal class Startup
    {
        static void Main(string[] args)
        {
            SoftUniContext context = new SoftUniContext();

            using (context)
            {
                Console.WriteLine(GetEmployeesFullInformation(context));
            }
        }


        //Task3
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            
            var employees = context.Employees
                .ToArray();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary:f2}");
            }

            return sb.ToString();
        }
    }
}
