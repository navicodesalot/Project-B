static class Menu
{
    //This shows the menu. You can call back to this method to show the menu again
    //after another presentation method is completed.
    //You could edit this to show different menus depending on the user's role
    public static void Start()
    {
        while (true)
        {
            Console.WriteLine("Enter 1 om in te loggen");
            Console.WriteLine("Enter 2 om boek gegevens te wijzigen");
            Console.WriteLine("Enter 3 om de volledige boekenlijst in te zien");
            Console.WriteLine("Enter 4 om een nieuw account te registreren.");

            string input = Console.ReadLine()!;
            if (input == "1")
            {
                UserLogin.Start();
            }
            else if (input == "2")
            {
                BookEdit bEdit = new BookEdit();
                bEdit.Start();
            }
            else if (input == "3")
            {
                BookView view = new BookView();
                view.ShowAllBooks();
            }
            else if (input == "4")
            {
                UserRegister.Start();
            }
            else
            {
                Console.WriteLine("Probeer het opnieuw");
                return;
            }
        }
    }
}