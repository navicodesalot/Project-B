static class UserLogin
{
    private static readonly AccountsLogic _accountsLogic = new();

    public static void Start()
    {
        while (true)
        {
            Console.WriteLine("Welkom bij het inlog scherm!");
            Console.WriteLine("Vul als eerst je volledige e-mail adres in.");
            string email = Console.ReadLine()!;
            Console.WriteLine("Vul je wachtwoord in.");
            string password = Console.ReadLine()!;
            AccountModel acc = _accountsLogic.CheckLogin(email, password)!;

            if (acc == null)
            {
                Console.WriteLine("E-mail adres en/of wachtwoord klopt, of bestaat niet.\nProbeer het opnieuw.");
                continue; // opnieuw proberen
            }

            acc.Reservations = ReservationAccess.GetByUserId((int)acc.Id).Count(r => r.EndDate > DateTime.Now);

            Console.WriteLine($"Welkom terug, {acc.FirstName} {acc.LastName}!");
            Console.WriteLine("Jouw e-mail adres is " + acc.Email);

            UserMenu.Start(acc); // menu voor ingelogde user
            break; // uitloggen = terug naar main menu
        }
    }
}