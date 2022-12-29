using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BookManagementSystem
{
    public class Program
    {
        public static string optionsFile = @"D:\bms\options.txt";
        public static string booksFile = @"D:\bms\books.txt";
        static string differ = "**********";
        static string welcome = $"Welcome to Book Management System\n{differ}";
        static void Main(string[] args)
        {
            Console.WriteLine(welcome);
            while (true)
            {
                Console.WriteLine("Please select any option");
                Console.WriteLine("1. Add book");
                Console.WriteLine("2. Get all books");
                Console.WriteLine("3. Get single book by details");
                Console.WriteLine("0. Exit");

                int choice = int.Parse(Console.ReadLine());

                OptionSelection(choice);
            }
        }

        /// <summary>
        /// OptionSelection
        /// </summary>
        /// <param name="choice"></param>
        public static void OptionSelection(int choice)
        {
            switch (choice)
            {
                case 1:
                    Add();
                    break;
                case 2:
                    List();
                    break;
                case 3:
                    Console.WriteLine("Please enter book name, id or author's name");
                    string searchCriteria = Console.ReadLine();
                    List(searchCriteria);
                    break;
                case 0:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Add
        /// </summary>
        public static void Add()
        {
            Book book = new Book();
            Console.Clear();
            Console.WriteLine(welcome + "\n");
            Console.WriteLine("Please enter the book id");
            book.Id = int.Parse(Console.ReadLine());
            Console.WriteLine("Please enter the book name");
            book.Name = Console.ReadLine();
            Console.WriteLine("Please enter the author");
            book.Author = Console.ReadLine();
            book.IsActive = true;
            book.CreatedOn = DateTime.Now;
            string data = $"{book.Id},{book.Name},{book.Author},{book.CreatedOn},{book.IsActive}";
            bool result = WriteToFile(data);
            if (result == true)
            {
                Console.Clear();
                Console.WriteLine(welcome);
                Console.WriteLine("Book successfully added");
                Thread.Sleep(3);
            } 
            else
            {
                Console.WriteLine("Something went wrong");

            }

        }

        /// <summary>
        /// WriteToFile
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool WriteToFile(string input)
        {
            try
            {
                FileStream fs = new FileStream(booksFile, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                StreamWriter sr = new StreamWriter(fs);
                sr.WriteLine(input);
                sr.Flush();
                fs.Close();
                return true;
            }
            catch (IOException ex)
            {
                Console.WriteLine("Exception " + ex.Message);

            }
            return false;
        }

        /// <summary>
        /// List
        /// </summary>
        public static void List()
        {
            Console.Clear();
            Console.WriteLine(welcome);
            foreach (string line in File.ReadLines(booksFile))
            {
                if (line.EndsWith("True"))
                {
                    BookBinding(line);
                }
            }
        }

        /// <summary>
        /// List
        /// </summary>
        public static void List(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                throw new ArgumentException($"'{nameof(search)}' cannot be null or whitespace.", nameof(search));
            }

            Console.Clear();
            Console.WriteLine(welcome);
            int count = 0;
            foreach (string line in File.ReadLines(booksFile))
            {
                if (line.Contains(search) && line.EndsWith("True"))
                {
                    BookBinding(line);
                    count++;
                }
            }
            Console.WriteLine("Total Book Count = " + count);
        }

        /// <summary>
        /// NewMethod
        /// </summary>
        /// <param name="line"></param>
        static void BookBinding(string line)
        {
            Book book = new Book();
            string[] result = line.Split(',');
            book.Id = int.Parse(result[0]);
            book.Name = result[1];
            book.Author = result[2];
            book.CreatedOn = DateTime.Parse(result[3]);
            string date = book.CreatedOn.ToString("dd/MM/yyyy hh:mm tt");
            Console.WriteLine($"Book Id: {book.Id}\nBook Name: {book.Name}\nBook Author: {book.Author}\nCreatedOn: {date}\n");
        }
    }
}
