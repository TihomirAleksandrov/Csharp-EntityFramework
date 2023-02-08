using CodeFirstDemo.Models;
using System;
using System.Collections.Generic;

namespace CodeFirstDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var db = new ApplicationDbContext();
            db.Database.EnsureCreated();

            db.Categories.Add(new Category
            {
                Title = "Sport",
                News = new List<News>
                {
                    new News
                    {
                        Title = "Charles Leclerc is 2023 drivers champion",
                        Content = "Шарл Льоклер върна титлата при Ферари за пръв път от 2007.",
                        Comments = new List<Comment>
                        {
                            new Comment { Author = "Pierre", Content = "I like this"},
                            new Comment { Author = "Sbinalla", Content = "Ferrari master Blan"}
                        }
                    }
                }
            });
            db.SaveChanges();
        }
    }
}
