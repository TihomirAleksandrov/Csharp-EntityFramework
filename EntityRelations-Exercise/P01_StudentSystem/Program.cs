using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data;
using System;

namespace P01_StudentSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            StudentSystemContext db = new StudentSystemContext();

            db.Database.Migrate();

            Console.WriteLine("db created");
        }
    }
}
