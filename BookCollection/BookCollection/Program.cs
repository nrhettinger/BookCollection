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
                Console.WriteLine("----------\nMAIN MENU\n----------");
                Console.WriteLine("C: Create a new book");
                Console.WriteLine("V: View collection");
                Console.WriteLine("Q: Quit the application");
                Console.WriteLine("S: Search collection");
                Console.WriteLine("U: Update books");
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
                    case "U":
                        Book.updateBooks();
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

        private static void displayBooks(SqlCommand command)
        {
            using (SqlDataReader reader = command.ExecuteReader())
            {
                Console.WriteLine("\nRESULTS:\n\n");
                while (reader.Read())
                {
                    Console.WriteLine("---------------\nTitle: {0}| ISBN: {1}| Author: {2}| Review: {3}\n---------------\n", reader[1], reader[0], reader[7] + " " + reader[8], reader[4]);
                }
            }
        }

        public static Book createNewBook(Book newBook) //scope, association with object, return type, function name, parameter type and name
        {
            Console.WriteLine("What is the name of the book?");
            newBook.Title = Console.ReadLine();
            Console.WriteLine("What series is this book part of? If not part of any, say 'None'.");
            newBook.Series = Console.ReadLine();
            Console.WriteLine("What is the ISBN?");
            newBook.ISBN = Console.ReadLine();
            Console.WriteLine("Who is the author? First name is:");
            newBook.AuthorFirst = Console.ReadLine();
            Console.WriteLine("Who is the author? Last name is:");
            newBook.AuthorLast = Console.ReadLine();
            Console.WriteLine("What is your review of the book? If you have not read it or do not want to write a review, say 'N/A'.");
            newBook.Review = Console.ReadLine();
            return newBook;
        }

        public static void addNewBook(Book newBook)
        {
            SqlConnection conn = Database.bookCollectionConnection();
            SqlCommand addNewBooks = new SqlCommand("Insert into Books (ISBN, Title, Series, Review) values (@ISBN, @Title, @Series, @Review)", conn);
            addNewBooks.Parameters.Add(new SqlParameter("ISBN", newBook.ISBN));
            addNewBooks.Parameters.Add(new SqlParameter("Title", newBook.Title));
            addNewBooks.Parameters.Add(new SqlParameter("Series", newBook.Series));
            addNewBooks.Parameters.Add(new SqlParameter("Review", newBook.Review));
            SqlCommand addNewAuthors = new SqlCommand("insert into Authors (FirstName, LastName) values (@AuthorFirst, @AuthorLast)", conn);
            addNewAuthors.Parameters.Add(new SqlParameter("AuthorFirst", newBook.AuthorFirst));
            addNewAuthors.Parameters.Add(new SqlParameter("AuthorLast", newBook.AuthorLast));
            addNewBooks.ExecuteNonQuery();
            addNewAuthors.ExecuteNonQuery();
            SqlCommand insertA_ID = new SqlCommand("spInsertA_ID @AF, @AL, @T", conn); 
            insertA_ID.Parameters.Add(new SqlParameter("AF", newBook.AuthorFirst));
            insertA_ID.Parameters.Add(new SqlParameter("AL", newBook.AuthorLast));
            insertA_ID.Parameters.Add(new SqlParameter("T", newBook.Title));
            insertA_ID.ExecuteNonQuery();
        }

        public static void viewAllBooks()
        {
            SqlConnection conn = Database.bookCollectionConnection();
            SqlCommand viewAllBooks = new SqlCommand("spViewAllBooks", conn);
            displayBooks(viewAllBooks);
        }

        public static void searchBooks()
        {
            SqlConnection conn = Database.bookCollectionConnection();
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
                    SqlCommand searchByTitle = new SqlCommand("spSearchByTitle @T", conn);
                    searchByTitle.Parameters.Add(new SqlParameter("T", title));
                    displayBooks(searchByTitle);
                    break;
                case "A":
                    Console.WriteLine("Enter the first name of the author you are searching for: ");
                    string authorFirst = Console.ReadLine();
                    Console.WriteLine("Enter the last name of the author you are searching for: ");
                    string authorLast = Console.ReadLine();
                    SqlCommand searchByAuthor = new SqlCommand("spSearchByAuthor @AF, @AL", conn);
                    searchByAuthor.Parameters.Add(new SqlParameter("AF", authorFirst));
                    searchByAuthor.Parameters.Add(new SqlParameter("AL", authorLast));
                    displayBooks(searchByAuthor);
                    break;
                case "I":
                    Console.WriteLine("Enter the ISBN you are searching for: ");
                    string isbn = Console.ReadLine();
                    SqlCommand searchByISBN = new SqlCommand("spSearchByISBN @I", conn);
                    searchByISBN.Parameters.Add(new SqlParameter("I", isbn));
                    displayBooks(searchByISBN);
                    break;
                default:
                    Console.WriteLine("Select one of the options:");
                    break;
            }
        }

        private static void updateField(string fieldName, string storedProcedure, string recordID, string recordName, string fieldName1 = null, string fieldName2 = null)
        {
            SqlConnection conn = Database.bookCollectionConnection();
            Console.WriteLine("Would you like to update the " + fieldName + "? Enter Y (yes) or N (no):");
            string recordN = recordName;
            string response = Console.ReadLine().ToUpper();
            switch (response) 
            {
                case "Y":
                    SqlCommand updateField = new SqlCommand();
                    updateField.CommandType = System.Data.CommandType.StoredProcedure;
                    updateField.CommandText = storedProcedure;
                    {
                        if (fieldName1 != null && fieldName2 !=null) //this scenario is when there are two parts to a field name. Example - the author field is divided into FirstName and LastName in the database. To change it two new values need to be sent in as arguments into the function. This block deals with that scenario
                        {
                            Console.WriteLine("Enter the new " + fieldName1 + ":");
                            string valueNew1 = Console.ReadLine();
                            Console.WriteLine("Enter the new " + fieldName2 + ":");
                            string valueNew2 = Console.ReadLine();
                            updateField.Parameters.Add(new SqlParameter("VN1", valueNew1));
                            updateField.Parameters.Add(new SqlParameter("VN2", valueNew2));
                        }
                        else
                        {
                            Console.WriteLine("Enter the new " + fieldName + ":");
                            string valueNew = Console.ReadLine();
                            updateField.Parameters.Add(new SqlParameter("VN", valueNew));
                        }
                        updateField.Parameters.Add(new SqlParameter("rID", recordID));
                        updateField.Parameters.Add(new SqlParameter("rName", recordN));
                        updateField.Connection = conn;
                        updateField.ExecuteNonQuery();
                        Console.WriteLine("The " +fieldName+ " is updated!");
                        break;
                    }
                case "N":
                    {
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Please enter Y (yes) or N (No) for updating the " +fieldName+ ".");
                        break;
                    }
            }
        }

        private static void runUpdateFieldFunctions(string recordName)
        {
            Console.WriteLine("You can update the following fields: Title, Series, Author and Review. Enter the "+recordName+" of the book you want to update:");
            string recordID = Console.ReadLine();
            string recordN = recordName;
            updateField("Title", "spUpdateTitle", recordID, recordN);
            updateField("Series", "spUpdateSeries", recordID, recordN);
            updateField("Author", "spUpdateAuthor", recordID, recordN,"first name", "last name");
            updateField("Review", "spUpdateReview", recordID, recordN);
        }

        public static void updateBooks() 
        {
            Console.WriteLine("Which book would you like to update? You can search by ISBN or title. Select I (ISBN) or T (Title) below:");
            string response = Console.ReadLine().ToUpper();
            switch (response)
            {
                case "I":
                    runUpdateFieldFunctions("ISBN");
                    break;
                case "T":
                    runUpdateFieldFunctions("Title");
                    break;
                default:
                    Console.WriteLine("Select one of the options:");
                    break;
            }
        }
    }
}

      

