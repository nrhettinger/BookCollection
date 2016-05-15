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
            if (Response == "Create a new book") {
                Book newBook = new Book();
                addNewBook(Book newBook) {

                }
        } 
      
 
    


    public class Book
    {
        public string Title { get; set; }
        public string Series { get; set; }
        public int ISBN { get; set; }
        public string Author { get; set; }
        public string Review { get; set; }

        

        public static Book addNewBook (Book newBook)
        {
                Console.WriteLine("What is the name of the book?");
                newBook.Title = Console.ReadLine();
                Console.WriteLine("What series is this book part of? If not part of any, say 'None'.");
                newBook.Series = Console.ReadLine();
                Console.WriteLine("What is the ISBN? If unknown say 'N/A'");
                newBook.ISBN = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Who is the Author?");
                newBook.Author = Console.ReadLine();
                Console.WriteLine("What is your review of the book? If you have not read it or do not want to write a review, say 'N/A'.");
                newBook.Review = Console.ReadLine();
                return newBook;
        }
    }
}
