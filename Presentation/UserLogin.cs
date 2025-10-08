static class UserLogin
{
    private static readonly AccountsLogic _accountsLogic = new();

    public static void Start()
    {
        Console.WriteLine("Welkom bij het inlog scher,");
        Console.WriteLine("Vul als eerst uw volledige e-mail adres in.");
        string email = Console.ReadLine()!;
        Console.WriteLine("Vul ut wachtwoord in.");
        string password = Console.ReadLine()!;
        AccountModel acc = _accountsLogic.CheckLogin(email, password)!;

        if (acc == null)
        {
            Console.WriteLine("E-mail adres en/of wachtwoord klopt, of bestaat niet.\nProbeer het opnieuw.");
            return;
        }

        Console.WriteLine($"Welkom terug, {acc.FirstName} {acc.LastName}!");
        Console.WriteLine("Jouw e-mail adres is " + acc.Email);

        UserMenu.Start(acc); // menu voor ingelogde user
    }
}