// menu voor ingelogde users

public static class UserMenu
{
    public static void Start(AccountModel user)
    {
        // logica voor reserveringen en boeken
        ReservationLogic reservationLogic = new ReservationLogic();
        BooksLogic booksLogic = new BooksLogic();
        BookView view = new BookView();

        // uitlog functie dingetje
        bool loggedOut = false;

        var options = new List<string>
        {
            "Reserveer een boek",
            "Bekijk je reserveringen",
            "Bekijk alle boeken",
            "Zoek een boek",
            "Bekijk je leen-geschiedenis",
            "Boek terugbrengen",
            "Uitloggen"
        };

        var actions = new List<Action>
        {
            // optie 1: boek reserveren
            () => {
                Console.Clear();
                Console.Write("Voer het ISBN van het boek in: ");
                string bookId = Console.ReadLine()!;
                BookModel book = booksLogic.GetById(bookId);
                if (string.IsNullOrWhiteSpace(bookId) || book == null)
                {
                    Console.WriteLine("Dit boek bestaat niet of ISBN is leeg.");
                    Console.WriteLine("Druk op een toets om terug te gaan naar het menu...");
                    Console.ReadKey(true);
                    return;
                }
                if (!reservationLogic.CanReserve((int)user.Id))
                {
                    Console.WriteLine("Je hebt het max aantal boeken geleend! Breng er een terug om verder te kunnen lenen.");
                    Console.ReadKey(true);
                    return;
                }
                var activeReservation = ReservationAccess.GetActiveByBookId(bookId);
                if (activeReservation != null)
                {
                    Console.WriteLine("Dit boek is nu uitgeleend!");
                    Console.WriteLine($"Wil je het boek reserveren voor zodra het beschikbaar is? (j/n)");
                    string keuze = Console.ReadLine()!.Trim().ToLower();
                    if (keuze == "j" || keuze == "ja")
                    {
                        string result = reservationLogic.TryReserveBook((int)user.Id, bookId);
                        Console.WriteLine(result);
                    }
                    else
                    {
                        Console.WriteLine("Reservering geannuleerd.");
                    }
                }
                else
                {
                    string result = reservationLogic.TryReserveBook((int)user.Id, bookId);
                    Console.WriteLine(result);
                }
                Console.WriteLine("Druk op een toets om terug te gaan naar het menu...");
                Console.ReadKey(true);
            },

            // optie 2: reserveringen bekijken
            () => {
                Console.Clear();
                var reservations = reservationLogic.GetActiveReservations((int)user.Id);

                if (reservations.Count == 0)
                {
                    Console.WriteLine("Je hebt geen actieve reserveringen.");
                }
                else
                {
                    Console.WriteLine($"Je hebt {reservations.Count} actieve reserveringen.");
                    foreach (var (r, bookTitle, authorName, status) in reservations)
                    {
                        Console.WriteLine($"ISBN: {r.BookId}");
                        Console.WriteLine($"Titel: {bookTitle}");
                        Console.WriteLine($"Auteur: {authorName}");
                        Console.WriteLine($"Geleend: {r.StartDate:dd-MM-yyyy}");
                        Console.WriteLine($"Deadline: {r.EndDate:dd-MM-yyyy}");
                        Console.WriteLine($"Status: {status}");
                        Console.WriteLine("-----------------------------");
                    }
                }
                Console.WriteLine("Druk op een toets om terug te gaan naar het menu...");
                Console.ReadKey(true);
            },

            // optie 3: alle boeken bekijken
            () => {
                Console.Clear();
                view.ShowAllBooks();
                Console.WriteLine("Druk op een toets om terug te gaan naar het menu...");
                Console.ReadKey(true);
            },

            // optie 4: zoeken naar een boek
            () => {
                Console.Clear();
                view.LiveSearchBooks();
                Console.WriteLine("Druk op een toets om terug te gaan naar het menu...");
                Console.ReadKey(true);
            },

            // optie 5: leen-geschiedenis bekijken
            () => {
                Console.Clear();
                var history = reservationLogic.GetHistory((int)user.Id);

                if (history.Count == 0)
                {
                    Console.WriteLine("Je hebt nog geen boeken geleend.");
                }
                else
                {
                    Console.WriteLine($"Je hebt {history.Count} boeken geleend in totaal.");
                    foreach (var (r, bookTitle, authorName, status, deadline) in history)
                    {
                        Console.WriteLine($"ISBN: {r.BookId}");
                        Console.WriteLine($"Titel: {bookTitle}");
                        Console.WriteLine($"Auteur: {authorName}");
                        Console.WriteLine($"Geleend: {r.StartDate:dd-MM-yyyy}");
                        Console.WriteLine($"Boek teruggebracht: {deadline}");
                        Console.WriteLine($"Status: {status}");
                        Console.WriteLine("-----------------------------");
                    }
                }
                Console.WriteLine("Druk op een toets om terug te gaan naar het menu...");
                Console.ReadKey(true);
            },

            // optie 6: boek terugbrengen
            () => {
                Console.Clear();
                Console.Write("Voer het ISBN van het boek in dat je wilt terugbrengen: ");
                string bookId = Console.ReadLine()!;
                string result = reservationLogic.ReturnBook((int)user.Id, bookId);
                Console.WriteLine(result);
                Console.WriteLine("Druk op een toets om terug te gaan naar het menu...");
                Console.ReadKey(true);
            },

            // optie 7: uitloggen
            () => {
                Console.Clear();
                Console.WriteLine("Uitgelogd.");
                loggedOut = true;
            }
        };

        var menu = new Menu(options, actions);

        // niet uitgelogd = menu
        while (!loggedOut)
        {
            menu.Link();
        }
        // terug naar vorige menu
        return;
    }
}