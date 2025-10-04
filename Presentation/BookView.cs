public class BookView
{
    private BooksLogic logic = new BooksLogic();

    public void ShowAllBooks()
    {
        var books = logic.GetAllBooks();

        if (books.Count == 0)
        {
            Console.WriteLine("Er zijn geen boeken gevonden.");
            return;
        }

        int pageSize = 5;
        int totalBooks = books.Count;
        int currentPage = 0;
        // logica voor pagina shit kill me

        while ((currentPage * pageSize) < totalBooks)
        {
            Console.WriteLine($"Lijst van boeken (pagina {currentPage + 1}):");
            for (int i = currentPage * pageSize; i < Math.Min((currentPage + 1) * pageSize, totalBooks); i++)
            {
                var book = books[i];
                Console.WriteLine($"Titel: {book.Title}, Genre: {book.Genre}, Auteur ID: {book.AuthorId}");
            }

            if ((currentPage + 1) * pageSize >= totalBooks)
            {
                Console.WriteLine("Geen andere boeken.");
                break;
            }

            Console.WriteLine("Druk op Enter voor de volgende pagina, of typ 'q' om te stoppen met bladeren.");
            string input = Console.ReadLine();
            if (input != null && input.ToLower() == "q")
            {
                break;
            }
            else // aka null = geen input = meteen enter WAYOOOO
            {
                currentPage++;
            }
        }
    }
}