using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient; //tells program to use these specified classes to connect to an SQL database

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
                switch (response)
                {
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

    public class Database
    {
        public static SqlConnection bookCollectionConnection()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Server=TERRY-PC; Database=BookCollection; Trusted_Connection=true";
            conn.Open();
            return conn;
        }
    }

    public class Book
    {
        public string Title { get; set; }
        public string Series { get; set; }
        public string ISBN { get; set; }
        public string AuthorFirst { get; set; }
        public string AuthorLast { get; set; }
        public string Review { get; set; }

        public static Book createNewBook(Book newBook) //scope, association with object, return type, function name, parameter type and name
        {
            Console.WriteLine("What is the name of the book?");
            newBook.Title = Console.ReadLine();
            Console.WriteLine("What series is this book part of? If not part of any, say 'None'.");
            newBook.Series = Console.ReadLine();
            Console.WriteLine("What is the ISBN? If unknown say '00'");
            newBook.ISBN = Console.ReadLine();
            Console.WriteLine("Who is the author? First name is:");
            newBook.AuthorFirst = Console.ReadLine();
            Console.WriteLine("Who is the author? Last name is:");
            newBook.AuthorLast = Console.ReadLine();
            Console.WriteLine("What is your review of the book? If you have not read it or do not want to write a review, say 'N/A'.");
            newBook.Review = Console.ReadLine();
            return newBook;
        }

        static List<Book> myBookCollection = new List<Book>();


        public static void addNewBook(Book newBook)
        {
            SqlConnection conn = Database.bookCollectionConnection();
            SqlCommand addNewBooks = new SqlCommand("Insert into Books (ISBN, Title) values (@ISBN, @Title)", conn);
            addNewBooks.Parameters.Add(new SqlParameter("ISBN", newBook.ISBN));
            addNewBooks.Parameters.Add(new SqlParameter("Title", newBook.Title));
            //SqlCommand addNewAuthors = new SqlCommand("insert into Authors (FirstName, LastName) values (newBook.AuthorFirst, newBook.AuthorLast)", conn);
            addNewBooks.ExecuteNonQuery();
            //addNewAuthors.BeginExecuteNonQuery();
        }

        public static void viewAllBooks()
        {
            SqlConnection conn = Database.bookCollectionConnection();
            SqlCommand viewAllBooks = new SqlCommand("Select * from Books join Authors on Author=A_ID", conn);
            using (SqlDataReader reader = viewAllBooks.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine("\nTitle: {0}| ISBN: {1}| Author: {2}| Review: {3}\n", reader[1], reader[0], reader[6] + " " + reader[7], reader[4]);
                }
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
                                           book.Title, book.Series, book.AuthorFirst + " " + book.AuthorLast, book.Review);
                    }
                    break;
                case "A":
                    Console.WriteLine("Enter the first name of the author you are searching for: ");
                    string authorFirst = Console.ReadLine();
                    Console.WriteLine("Enter the last name of the author you are searching for: ");
                    string authorLast = Console.ReadLine();
                    var resultsAuthor = from book in myBookCollection
                                        where book.AuthorFirst.Contains(authorFirst) &&
                                        book.AuthorLast.Contains(authorLast)
                                        select book;
                    foreach (Book book in resultsAuthor)
                    {
                        Console.WriteLine("\nTitle: {0}| Series: {1}| Author: {2}| Review: {3}\n",
                                           book.Title, book.Series, book.AuthorFirst + " " + book.AuthorLast, book.Review);
                    }
                    break;
                case "I":
                    Console.WriteLine("Enter the ISBN you are searching for: ");
                    var isbn = Console.ReadLine();
                    var resultsIsbn = from book in myBookCollection
                                      where book.ISBN == isbn
                                      select book;
                    foreach (Book book in resultsIsbn)
                    {
                        Console.WriteLine("\nTitle: {0}| Series: {1}| Author: {2}| Review: {3}\n",
                                           book.Title, book.Series, book.AuthorFirst + " " + book.AuthorLast, book.Review);
                    }
                    break;
                default:
                    Console.WriteLine("Select one of the options:");
                    break;
            }
        }
    }
}

      

