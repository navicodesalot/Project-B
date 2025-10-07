using System.Security.Cryptography.X509Certificates;

public class BookEdit
{
    private BooksLogic logic = new BooksLogic();
    public void Start()
    {
        Console.WriteLine("Welk boek wil je wijzigen? Voer een titel in");
        string search = Console.ReadLine();

        BookModel book = logic.GetByTitle(search);

        if (book != null)
        {
            book = EditBook(book);
            logic.UpdateBook(book);
            Console.WriteLine("Het boek is bewaard!");
        }
        else
        {
            Console.WriteLine("Dit boek staat er niet in, helaas!");
        }
    }

    public BookModel EditBook(BookModel book)
    {
        Console.WriteLine($"Vul een nieuwe titel in voor {book.Title}");
        string newTitle = Console.ReadLine();

        Console.WriteLine($"Vul een nieuwe genre in voor {book.Genre}");
        string newGenre = Console.ReadLine();

        Console.WriteLine($"Vul een nieuwe auteur ID in voor {book.AuthorId}");
        int newAuthorId;
        while (!int.TryParse(Console.ReadLine(), out newAuthorId))
        {
            Console.WriteLine("Ongeldig ID, probeer opnieuw:");
        }

        book.Title = newTitle;
        book.Genre = newGenre;
        book.AuthorId = newAuthorId;

        return book;
    }
}