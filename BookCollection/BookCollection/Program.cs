using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCollection
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the book collection, what would you like to do?");
            string Response = Console.ReadLine();
            if (Response == "Create a new book")
            {
                Book newBook = new Book();
                Book.createNewBook(newBook);
                Book.addNewBook(newBook);
                Book.viewAllBooks();
            }
            else if (Response == "View collection")
            {
                Book.viewAllBooks();
            }

        
        }
    }

    public class Book
        {
            public string Title { get; set; }
            public string Series { get; set; }
            public int ISBN { get; set; }
            public string Author { get; set; }
            public string Review { get; set; }

            public static Book createNewBook(Book newBook) //scope, association with object, return type, function name, parameter type and name
            {
                Console.WriteLine("What is the name of the book?");
                newBook.Title = Console.ReadLine();
                Console.WriteLine("What series is this book part of? If not part of any, say 'None'.");
                newBook.Series = Console.ReadLine();
                Console.WriteLine("What is the ISBN? If unknown say '00'");
                newBook.ISBN = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Who is the Author?");
                newBook.Author = Console.ReadLine();
                Console.WriteLine("What is your review of the book? If you have not read it or do not want to write a review, say 'N/A'.");
                newBook.Review = Console.ReadLine();
                return newBook;
            }

            static List<Book> myBookCollection = new List<Book>();


            public static void addNewBook(Book newBook)
            {
               myBookCollection.Add(newBook);
            }

            public static void viewAllBooks()
            {
                foreach (Book book in myBookCollection)
                {
                Console.WriteLine("Title: {0}| Author: {1}| Review: {2}", book.Title, book.Author, book.Review);
                Console.ReadLine();
                }
            }
        
    }
}      

