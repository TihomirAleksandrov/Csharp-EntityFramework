namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);

            //Task2
            //var command = Console.ReadLine();
            //string result =GetBooksByAgeRestriction(db, command);
            //Task3
            //string result = GetGoldenBooks(db);
            //Task4
            //string result = GetBooksByPrice(db);
            //Task5
            //var year = int.Parse(Console.ReadLine());
            //string result = GetBooksNotReleasedIn(db, year);
            //Task6
            //var input = Console.ReadLine();
            //var result = GetBooksByCategory(db, input);
            //Task7
            //var input = Console.ReadLine();
            //var result = GetBooksReleasedBefore(db, input);
            //Task8
            //var input = Console.ReadLine();
            //var result = GetAuthorNamesEndingIn(db, input);
            //Task9
            //var input = Console.ReadLine();
            //var result = GetBookTitlesContaining(db, input);
            //Task10
            //var input = Console.ReadLine();
            //var result = GetBooksByAuthor(db, input);
            //Task11
            //var lengthCheck = int.Parse(Console.ReadLine());
            //var result = CountBooks(db, lengthCheck);
            //Task12
            //var result = CountCopiesByAuthor(db);
            //Task13
            //var result = GetTotalProfitByCategory(db);
            //Task14
            //var result = GetMostRecentBooks(db);
            //Task16
            var result = RemoveBooks(db);

            Console.WriteLine(result);
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            AgeRestriction ageRestriction;

            var isParseCuccess = Enum.TryParse<AgeRestriction>(command, true, out ageRestriction);

            if (!isParseCuccess)
            {
                return string.Empty;
            }

            var bookTitles = context.Books
                .Where(b => b.AgeRestriction == ageRestriction)
                .Select(b => b.Title)
                .OrderBy(t => t)
                .ToArray();

            return String.Join(Environment.NewLine, bookTitles);
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            var goldenBooks = context.Books
                .Where(b => b.Copies < 5000 && b.EditionType == EditionType.Gold)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            return String.Join(Environment.NewLine, goldenBooks);
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var books = context.Books
                .OrderByDescending(b => b.Price)
                .Where(b => b.Price > 40m)
                .Select(b => new
                {
                    Title = b.Title,
                    Price = b.Price
                })
                .ToArray();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var notReleasedBooks = context.Books
                .OrderBy(b => b.BookId)
                .Where(b => b.ReleaseDate.Value.Year != year)
                .Select(b => b.Title)
                .ToArray();

            return String.Join(Environment.NewLine, notReleasedBooks);
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var inputCategories = input
                .ToLower()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var categories = context.Books
                .Include(x => x.BookCategories)
                .ThenInclude(x => x.Category)
                .ToArray()
                .Where(b => b.BookCategories.Any(c => inputCategories.Contains(c.Category.Name.ToLower())))
                .Select(b => b.Title)
                .OrderBy(t => t)
                .ToArray();

            return String.Join(Environment.NewLine, categories);
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {


            var convertedDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            StringBuilder sb = new StringBuilder();

            var booksInfo = context.Books
                .ToArray()
                .Where(b => b.ReleaseDate < convertedDate)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => new
                {
                    Title = b.Title,
                    Edition = b.EditionType,
                    Price = b.Price
                })
                .ToArray();

            foreach (var book in booksInfo)
            {
                sb.AppendLine($"{book.Title} - {book.Edition} - ${book.Price:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();

            var authors = context.Authors
                .ToArray()
                .Where(a => a.FirstName.EndsWith(input))
                .OrderBy(a => a.FirstName)
                .Select(a => new
                {
                    FullName = $"{a.FirstName} {a.LastName}"
                })
                .ToArray();

            foreach (var author in authors)
            {
                sb.AppendLine(author.FullName);
            }


            return sb.ToString().TrimEnd();
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context.Books
                .ToArray()
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();

            var books = context.Books
                .Include(x => x.Author)
                .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(b => b.BookId)
                .Select(b => new
                {
                    Title = b.Title,
                    AuthorFullName = $"{b.Author.FirstName} {b.Author.LastName}"
                })
                .ToArray();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} ({book.AuthorFullName})");
            }

            return sb.ToString().TrimEnd();
        }
      
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var booksCount = context.Books
                .Where(b => b.Title.Length > lengthCheck)
                .Count();

            return booksCount;
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var authors = context.Authors
                .Include(x => x.Books)
                .Select(a => new
                {
                    FullName = $"{a.FirstName} {a.LastName}",
                    TotalBookCopies = a.Books.Sum(b => b.Copies)
                }).OrderByDescending(a => a.TotalBookCopies)
                .ToArray();

            foreach (var author in authors)
            {
                sb.AppendLine($"{author.FullName} - {author.TotalBookCopies}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var categories = context.Categories
                .Include(x => x.CategoryBooks)
                .ThenInclude(x => x.Book)
                .Select(c => new
                {
                    Name = c.Name,
                    Profit = c.CategoryBooks.Sum(bc => bc.Book.Copies * bc.Book.Price)
                })
                .OrderByDescending(c => c.Profit)
                .ThenBy(c => c.Name)
                .ToArray();

            foreach (var category in categories)
            {
                sb.AppendLine($"{category.Name} ${category.Profit:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var sb = new StringBuilder();

            var categories = context.Categories
                .Include(x => x.CategoryBooks)
                .ThenInclude(x => x.Book)
                .Select(c => new
                {
                    Name = c.Name,
                    Books = c.CategoryBooks
                    .OrderByDescending(cb => cb.Book.ReleaseDate)
                    .Select(b => new
                    {
                        Title = b.Book.Title,
                        ReleaseYear = b.Book.ReleaseDate.Value.Year
                    })
                    .Take(3)
                })
                .OrderBy(c => c.Name)
                .ToArray();

            foreach(var category in categories)
            {
                sb.AppendLine($"--{category.Name}");

                foreach (var book in category.Books)
                {
                    sb.AppendLine($"{book.Title} ({book.ReleaseYear})");
                }    
            }

            return sb.ToString().TrimEnd();
        }

        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year < 2010);

            foreach (var book in books)
            {
                book.Price += 5;
            }

            context.SaveChanges();
        }

        public static int RemoveBooks(BookShopContext context)
        {
            int count = 0;

            var books = context.Books
                .Where(b => b.Copies < 4200);

            count = books.Count();

            context.Books.RemoveRange(books);
            context.SaveChanges();

            return count;
        }
    }
}
