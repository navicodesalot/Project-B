using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
public class BookView
{
    private BooksLogic logic = new BooksLogic();

    /// <summary>
    /// Toont alle boeken met 5 boeken per pagina
    /// </summary>
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
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Lijst van boeken (pagina {currentPage + 1}):");
            Console.WriteLine("--------------------------------------------");
            Console.ResetColor();

            for (int i = currentPage * pageSize; i < Math.Min((currentPage + 1) * pageSize, totalBooks); i++)
            {
                var book = books[i];
                Console.WriteLine($"ISBN: {book.Id}, Titel: {book.Title}, Genre: {book.Genre}, Auteur ID: {book.AuthorId}");
            }

            Console.WriteLine();
            if (currentPage > 0)
                Console.WriteLine("Typ 'v' voor vorige pagina");
            if ((currentPage + 1) * pageSize < totalBooks)
                Console.WriteLine("Druk op Enter voor de volgende pagina");
            Console.WriteLine("Typ 'q' om te stoppen met bladeren.");

            string input = Console.ReadLine();
            if (input != null && input.ToLower() == "q")
            {
                break;
            }
            else if (input != null && input.ToLower() == "v" && currentPage > 0)
            {
                currentPage--;
            }
            else if ((currentPage + 1) * pageSize < totalBooks)
            {
                currentPage++;
            }
            else
            {
                break;
            }
        }
    }

    ////
    /// Live zoekfuntie filter op de boekenlijst
    /// 
    public void LiveSearchBooks()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("Zoek voor boek");
        Console.WriteLine("---------------");
        Console.ResetColor();

        // haal de boeken op 
        var allBooks = logic.GetAllBooks();
        // filer wat de gebruker invult
        string filter = string.Empty;
        ConsoleKeyInfo key;

        do
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Zoek voor boek");
            Console.WriteLine("---------------");
            Console.ResetColor();

            Console.Write("Zoek: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(filter);
            Console.ResetColor();

            // filter de boeken uit de lijst van de database
            var filtered = allBooks
                // filter wat de gebruiker intikt
                .Where(b =>
                    // ordinalIgnorecase is hoofletter gevoelig :)
                    b.Title.Contains(filter, StringComparison.OrdinalIgnoreCase) || // filter op titel
                    b.Genre.Contains(filter, StringComparison.OrdinalIgnoreCase) || // filter op genre
                    // vvv dit aanpassen als ik author naam heb!!!
                    b.AuthorId.ToString().Contains(filter, StringComparison.OrdinalIgnoreCase)) // filter op autuer
                .ToList();// naar een list yayayayay

            if (filtered.Count == 0)
            {
                // als er geen beoekn zijn
                Console.WriteLine("\nGeen boeken gevonden.");
            }
            else
            {
                Console.WriteLine();
                // tooon steeds 5 boeken 
                foreach (var book in filtered.Take(5))
                {
                    Console.WriteLine($"{book.Title} | Genre: {book.Genre} | Auteur: {book.AuthorId}");
                    // ik wil dit naar author name maar dat lukt nog niet!!!!!!

                }
                if (filtered.Count > 5)
                    Console.WriteLine($"\n.. en {filtered.Count - 5} meer resultaten");
            }
            // dit zorgt ervoor dat de invoer niet wordt getoont op de scerm.
            key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.Backspace && filter.Length > 0)
            { // controlleer als backsapce wordt ingetikt
                filter = filter[..^1];
            }
            else if (key.Key == ConsoleKey.Enter)
            {   // als er enter wordt gedrukt
                continue;
            }
            else if (key.Key == ConsoleKey.Escape)
            {   // als gebruiker stopt ga terug naar menu
                break;
            }
            else if (!char.IsControl(key.KeyChar))
            {   // voeg letter toe aan de filter
                filter += key.KeyChar;
            }
        } while (true);
        Console.ResetColor();
        Console.Clear();

    }
}