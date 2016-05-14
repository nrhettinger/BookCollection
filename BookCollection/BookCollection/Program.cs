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
                addNewBook();
            }
        }
    }

    class Book
    {
        public string Title { get; set; }
        public string Series { get; set; }
        public int MyProperty { get; set; }
        public int ISBN { get; set; }
        public string Author { get; set; }
        public string Review { get; set; }
        
        public void addNewBook()
        {
            Book newBook = new Book();

        }
        createNewBook
        {
          
        }
    }
}
