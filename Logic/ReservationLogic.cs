using Dapper;

public class ReservationLogic
{
    public bool CanReserve(int userId)
    {
        // check of max reserveringen al zijn gedaan
        int activeCount = ReservationAccess.GetByUserId(userId).Count(r => r.EndDate > DateTime.Now);
        return activeCount < 10;
    }

    public bool IsBookAvailable(string bookId)
    {
        // check of boek niet al is uitgeleend
        ReservationModel? active = ReservationAccess.GetActiveByBookId(bookId);
        return active == null;
    }

    public void ReserveBook(int userId, string bookId)
    {
        // maak nieuwe reservering aan
        DateTime start = DateTime.Now;
        DateTime end = start.AddDays(21); // 3 weken :P
        ReservationModel reservation = new ReservationModel(0, userId, bookId, start, end);
        ReservationAccess.Write(reservation);

        // update reservations teller in account
        AccountsAccess access = new AccountsAccess();
        AccountModel? acc = access.GetByEmail("");
        if (acc != null)
        {
            acc.Reservations = ReservationAccess.GetByUserId(userId).Count(r => r.EndDate > DateTime.Now);
            access.Update(acc);
        }
    }

    public string TryReserveBook(int userId, string bookId)
    {
        if (string.IsNullOrWhiteSpace(bookId))
            return "ISBN mag niet leeg zijn.";

        BookModel book = new BooksLogic().GetById(bookId);
        if (book == null)
            return "Dit boek bestaat niet.";

        if (!CanReserve(userId))
            return "Je hebt het max aantal boeken geleend! Breng er een terug om verder te kunnen lenen.";

        var activeReservation = ReservationAccess.GetActiveByBookId(bookId);
        AuthorsLogic authorsLogic = new AuthorsLogic();
        string authorName = "Onbekend";
        if (book != null)
        {
            var author = authorsLogic.GetById(book.AuthorId);
            if (author != null)
                authorName = author.Name;
        }

        if (activeReservation != null)
        {
            // boek is nu uitgeleend, reservering start dag na terugbrengen
            DateTime start = activeReservation.EndDate.AddDays(1);
            DateTime end = start.AddDays(21);
            ReservationModel reservation = new ReservationModel(0, userId, bookId, start, end);
            ReservationAccess.Write(reservation);

            return $"Het boek is gereserveerd voor zodra het beschikbaar is\nISBN: {bookId}\nTitel: {book.Title}\nAuteur: {authorName}\nJe kunt het boek ophalen vanaf {start:dd-MM-yyyy}\nStartdatum: {start:dd-MM-yyyy}\nEinddatum: {end:dd-MM-yyyy}";
        }
        else
        {
            // boek is beschikbaar, reservering start vandaag
            DateTime start = DateTime.Now;
            DateTime end = start.AddDays(22);
            // nu 22 dagen, zodat de gebruiker een dag heeft om het boek op te halen
            ReservationModel reservation = new ReservationModel(0, userId, bookId, start, end);
            ReservationAccess.Write(reservation);

            return $"Boek succesvol gereserveerd!\nJe kunt het boek morgen ophalen.\nISBN: {bookId}\nTitel: {book.Title}\nAuteur: {authorName}\nStartdatum: {start:dd-MM-yyyy}\nEinddatum: {end:dd-MM-yyyy}";
        }
    }

    public List<(ReservationModel, string, string, string)> GetActiveReservations(int userId)
    {
        // haalt alle reserveringen op die nog niet zijn afgelopen
        // als startdatum in de toekomst is, check of iemand anders het boek nu heeft
        var reservations = ReservationAccess.GetByUserId(userId)
            .Where(r => r.EndDate > DateTime.Now)
            .OrderBy(r => r.StartDate)
            .ToList();

        var authorsLogic = new AuthorsLogic();
        var booksLogic = new BooksLogic();

        var result = new List<(ReservationModel, string, string, string)>();
        foreach (var r in reservations)
        {
            var book = booksLogic.GetById(r.BookId);
            string bookTitle = book != null ? book.Title : "Onbekend";
            string authorName = "Onbekend";
            if (book != null)
            {
                var author = authorsLogic.GetById(book.AuthorId);
                if (author != null)
                    authorName = author.Name;
            }
            string status;
            // als het vandaag de startdatum is, dan mag je ophalen
            if (r.StartDate.Date == DateTime.Now.Date)
            {
                status = "Klaar om op te halen";
            }
            // als het na vandaag is, dan wachten
            else if (r.StartDate > DateTime.Now)
            {
                // check of iemand anders het boek nu heeft
                var activeReservation = ReservationAccess.GetActiveByBookId(r.BookId);
                if (activeReservation != null && activeReservation.EndDate > DateTime.Now)
                {
                    status = "Wacht op beschikbaarheid";
                }
                else
                {
                    // check of jouw reservering de eerstvolgende is van ALLE reserveringen voor dit boek
                    var allReservationsForBook = ReservationAccess.GetByBookId(r.BookId)
                        .Where(x => x.StartDate > DateTime.Now)
                        .OrderBy(x => x.StartDate)
                        .ToList();

                    // als jouw reservering de eerste is in de lijst, dan mag je ophalen
                    if (allReservationsForBook.Count > 0 && allReservationsForBook[0].Id == r.Id)
                    {
                        // boek is teruggebracht en jij bent de volgende, dus je mag ophalen
                        status = "Klaar om op te halen";
                    }
                    else
                    {
                        status = "Wacht op beschikbaarheid";
                    }
                }
            }
            // als het na de startdatum is, dan heb je het boek in bezit
            else if (r.StartDate < DateTime.Now && r.EndDate > DateTime.Now)
            {
                // je hebt het boek nu
                status = "In bezit";
            }
            else
            {
                status = "Teruggebracht";
            }
            result.Add((r, bookTitle, authorName, status));
        }
        return result;
    }

    public List<(ReservationModel, string, string, string, string)> GetHistory(int userId)
    {
        // haalt alle reserveringen op, gesorteerd van nieuw naar oud
        // als boek niet teruggebracht is, deadline is ???
        var history = ReservationAccess.GetByUserId(userId)
            .OrderByDescending(r => r.EndDate)
            .ToList();

        var authorsLogic = new AuthorsLogic();
        var booksLogic = new BooksLogic();

        var result = new List<(ReservationModel, string, string, string, string)>();
        foreach (var r in history)
        {
            var book = booksLogic.GetById(r.BookId);
            string bookTitle = book != null ? book.Title : "Onbekend";
            string authorName = "Onbekend";
            if (book != null)
            {
                var author = authorsLogic.GetById(book.AuthorId);
                if (author != null)
                    authorName = author.Name;
            }
            string status;
            string teruggebracht;

            if (r.EndDate <= DateTime.Now)
            {
                // boek is teruggebracht
                status = "Teruggebracht";
                teruggebracht = "Ja";
            }
            else if (r.StartDate > DateTime.Now)
            {
                // reservering is nog niet gestart
                var activeReservation = ReservationAccess.GetActiveByBookId(r.BookId);
                if (activeReservation != null && activeReservation.EndDate > DateTime.Now)
                {
                    status = "Wacht op beschikbaarheid";
                }
                else
                {
                    status = "Klaar om op te halen";
                }
                teruggebracht = "Nee";
            }
            else
            {
                // je hebt het boek nu
                status = "In bezit";
                teruggebracht = "Nee";
            }
            result.Add((r, bookTitle, authorName, status, teruggebracht));
        }
        return result;
    }

    public string ReturnBook(int userId, string bookId)
    {
        // zoek de actieve reservering van deze user en dit boek
        var reservations = ReservationAccess.GetByUserId(userId);
        var active = reservations.FirstOrDefault(r => r.BookId == bookId && r.StartDate <= DateTime.Now && r.EndDate > DateTime.Now);

        if (active == null)
            return "Je hebt dit boek niet nu in bezit..";

        // update de einddatum naar vandaag
        using var connection = new Microsoft.Data.Sqlite.SqliteConnection($"Data Source=DataSources/project.db");
        string sql = "UPDATE Reservations SET enddate = @EndDate WHERE id = @Id";
        connection.Execute(sql, new { EndDate = DateTime.Now, Id = active.Id });

        // update de eerstvolgende reservering voor dit boek
        ReservationAccess.UpdateNextReservationStart(bookId, DateTime.Now);

        return "Boek succesvol teruggebracht!";
    }
}