// menu voor ingelogde users

public static class UserMenu
{
    public static void Start(AccountModel user)
    {
        // logica voor reserveringen en boeken
        ReservationLogic reservationLogic = new ReservationLogic();
        BooksLogic booksLogic = new BooksLogic();

        while (true) // menu blijft draaien tot je uitlogt
        {
            // print opties voor de gebruiker
            Console.WriteLine("\nWelkom, " + user.FirstName + "!");
            Console.WriteLine("1. Reserveer een boek");
            Console.WriteLine("2. Bekijk je reserveringen");
            Console.WriteLine("3. Bekijk alle boeken");
            Console.WriteLine("4. Zoek een boek");
            Console.WriteLine("5. Uitloggen");

            string input = Console.ReadLine()!; // lees keuze van gebruiker

            // optie 1: boek reserveren
            if (input == "1")
            {
                // check of gebruiker nog mag reserveren (max 10 boeken)
                if (!reservationLogic.CanReserve((int)user.Id))
                {
                    Console.WriteLine("Je hebt het max aantal boeken geleend! Breng er een terug om verder te kunnen lenen.");
                    continue; // terug naar menu
                }
                Console.Write("Voer het ISBN van het boek in: ");
                string bookId = Console.ReadLine()!;
                // check of boek beschikbaar is
                if (!reservationLogic.IsBookAvailable(bookId))
                {
                    Console.WriteLine("Dit boek is al in lening, sorry!");
                }
                else
                {
                    // boek reserveren
                    reservationLogic.ReserveBook((int)user.Id, bookId);
                    BookModel book = booksLogic.GetById(bookId);
                    string bookTitle = book != null ? book.Title : "Onbekend";
                    string authorName = "Onbekend";
                    if (book != null)
                    {
                        AuthorsLogic authorsLogic = new AuthorsLogic();
                        AuthorModel author = authorsLogic.GetById(book.AuthorId);
                        if (author != null)
                            authorName = author.Name;
                    }
                    // bevestiging voor gebruiker
                    Console.WriteLine("Boek succesvol gereserveerd!");
                    Console.WriteLine($"ISBN: {bookId}\nTitel: {bookTitle}\nAuteur: {authorName}\nDatum van reservering: {DateTime.Now:dd-MM-yyyy}");
                }
            }
            // optie 2: reserveringen bekijken
            else if (input == "2")
            {
                Console.WriteLine("Work in progress! :D");
                // gaat direct terug naar menu
            }
            // optie 3: alle boeken bekijken
            else if (input == "3")
            {
                BookView view = new BookView();
                view.ShowAllBooks(); // toont boeken met paginering
            }
            // optie 4: zoeken naar een boek
            else if (input == "4")
            {
                Console.WriteLine("Work in progress! :D");
                // gaat direct terug naar menu
            }
            // optie 5: uitloggen
            else if (input == "5")
            {
                Console.WriteLine("Uitgelogd.");
                break; // stopt het menu
            }
            // als de input niet klopt
            else
            {
                Console.WriteLine("Ongeldige keuze, probeer opnieuw.");
            }
        }
    }
}