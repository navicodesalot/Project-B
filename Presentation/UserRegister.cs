using System.Text.RegularExpressions;

public static class UserRegister
{
    public static void Start()
    {
        RegisterLogic logic = new RegisterLogic();

        while (true) // restart loop
        {
            Console.WriteLine("Registreren:");

            string email = "";
            while (true)
            {
                Console.Write("E-mail: ");
                email = Console.ReadLine()!;
                if (string.IsNullOrWhiteSpace(email))
                {
                    Console.WriteLine("E-mail mag niet leeg zijn.");
                    continue;
                }
                // opgezochte regex voor email format
                var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(email, emailPattern))
                {
                    Console.WriteLine("E-mail is geen geldig e-mailadres.");
                    continue;
                }
                break;
            }

            if (!logic.IsEmailUnique(email))
            {
                Console.WriteLine("Dit e-mailadres is al in gebruik. Probeer een ander e-mailadres.");
                return;
            }

            string password = "";
            while (password == "")
            {
                Console.Write("Wachtwoord: ");
                password = Console.ReadLine()!;
                if (password == "")
                    Console.WriteLine("Wachtwoord mag niet leeg zijn.");
            }

            string firstName = "";
            while (firstName == "")
            {
                Console.Write("Voornaam: ");
                firstName = Console.ReadLine()!;
                if (firstName == "")
                    Console.WriteLine("Voornaam mag niet leeg zijn.");
            }

            string lastName = "";
            while (lastName == "")
            {
                Console.Write("Achternaam: ");
                lastName = Console.ReadLine()!;
                if (lastName == "")
                    Console.WriteLine("Achternaam mag niet leeg zijn.");
            }

            string dob = "";
            while (dob == "")
            {
                Console.Write("Geboortedatum (dd-mm-yyyy): ");
                dob = logic.FixDobFormat(Console.ReadLine()!);
                if (dob == "")
                    Console.WriteLine("Ongeldige geboortedatum. Gebruik het formaat: dd-mm-yyyy.");
            }

            int reservations = 0;
            double fines = 0;

            // acc deetz
            Console.WriteLine("\nControleer je gegevens:");
            Console.WriteLine($"Email: {email}");
            Console.WriteLine($"Wachtwoord: {password}");
            Console.WriteLine($"Voornaam: {firstName}");
            Console.WriteLine($"Achternaam: {lastName}");
            Console.WriteLine($"Geboortedatum: {dob}");
            Console.WriteLine("Ziijn deze gegevens correct? (j/n)");

            string confirm = Console.ReadLine()!.Trim().ToLower();
            if (confirm == "j" || confirm == "ja")
            {
                AccountModel account = new AccountModel(0, email, password, firstName, lastName, dob, reservations, fines);
                // 0 omdat de database auto id doet wnr iets toegevoegd wordt
                AccountsAccess access = new AccountsAccess();
                access.Write(account);

                Console.WriteLine("Account aangemaakt!");
                break;
            }
            else
            {
                Console.WriteLine("Registratie opnieuw starten...\n");
            }
        }
    }
}