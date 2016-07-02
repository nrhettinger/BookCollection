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
            bool menuBreaker = false;
            Console.WriteLine("Welcome to the book collection, what would you like to do?");
            while (menuBreaker == false)
            {
                Console.WriteLine("C: Create a new book");
                Console.WriteLine("V: View collection");
                Console.WriteLine("Q: Quit the application");
                Console.WriteLine("S: Search collection");
                string response = Console.ReadLine().ToUpper();
                switch (response) {
                    case "V":
                        Book.viewAllBooks();
                        break;
                    case "C":
                        Book newBook = new Book();
                        Book.createNewBook(newBook);
                        Book.addNewBook(newBook);
                        break;
                    case "Q":
                        menuBreaker = true; //changes the menubreaker value, setting the while condition false and breaking the loop
                        break;
                    case "S":
                        Book.searchBooks();
                        break;
                    default:
                        Console.WriteLine("Select one of the options:");
                        break;
                }
            } 
        }
    }

    public class Book
    {
        public string Title { get; set; }
        public string Series { get; set; }
        public string ISBN { get; set; }
        public string Author { get; set; }
        public string Review { get; set; }

        public static Book createNewBook(Book newBook) //scope, association with object, return type, function name, parameter type and name
        {
            Console.WriteLine("What is the name of the book?");
            newBook.Title = Console.ReadLine();
            Console.WriteLine("What series is this book part of? If not part of any, say 'None'.");
            newBook.Series = Console.ReadLine();
            Console.WriteLine("What is the ISBN? If unknown say '00'");
            newBook.ISBN = Console.ReadLine();
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
            }
        }

        public static void searchBooks()
        {
            Console.WriteLine("Search by the following:");
            Console.WriteLine("T: Title");
            Console.WriteLine("A: Author");
            Console.WriteLine("I: ISBN");
            string response = Console.ReadLine().ToUpper();
            switch (response)
            {
                case "T":
                    Console.WriteLine("Enter the title you are searching for: ");
                    string title = Console.ReadLine();
                    var resultsTitle = from book in myBookCollection
                                       where book.Title.Contains(title)
                                       select book;
                    foreach (Book book in resultsTitle)
                    {
                        Console.WriteLine("\nTitle: {0}| Series: {1}| Author: {2}| Review: {3}\n",
                                           book.Title, book.Series, book.Author, book.Review);
                    }
                    break;
                case "A":
                    Console.WriteLine("Enter the author you are searching for: ");
                    string author = Console.ReadLine();
                    var resultsAuthor = from book in myBookCollection
                                    where book.Author.Contains(author)
                                    select book;
                    foreach (Book book in resultsAuthor)
                    {
                        Console.WriteLine("\nTitle: {0}| Series: {1}| Author: {2}| Review: {3}\n",
                                           book.Title, book.Series, book.Author, book.Review);
                    }
                    break;
                case "I":
                    Console.WriteLine("Enter the ISBN you are searching for: ");
                    var isbn = Convert.ToInt64(Console.ReadLine());
                    var resultsIsbn = from book in myBookCollection
                                  where book.ISBN == isbn
                                  select book;
                    foreach (Book book in resultsIsbn)
                    {
                        Console.WriteLine("\nTitle: {0}| Series: {1}| Author: {2}| Review: {3}\n",
                                           book.Title, book.Series, book.Author, book.Review);
                    }
                    break;
                default:
                    Console.WriteLine("Select one of the options:");
                    break;
            } 
        }
    }
}      

