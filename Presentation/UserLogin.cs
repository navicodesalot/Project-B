static class UserLogin
{
    private static readonly AccountsLogic _accountsLogic = new();

    public static void Start()
    {
        Console.WriteLine("Welcome to the login page");
        Console.WriteLine("Please enter your email address");
        string email = Console.ReadLine()!;
        Console.WriteLine("Please enter your password");
        string password = Console.ReadLine()!;
        AccountModel acc = _accountsLogic.CheckLogin(email, password)!;

        if (acc == null)
        {
            Console.WriteLine("No account found with that email and password");
            return;
        }

        Console.WriteLine("Welcome back " + acc.FullName);
        Console.WriteLine("Your email number is " + acc.EmailAddress);
    }
}