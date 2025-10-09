using System;
static class Menu
{
    //This shows the menu. You can call back to this method to show the menu again
    //after another presentation method is completed.
    //You could edit this to show different menus depending on the user's role
    public static void Start()
    {
        // maak instatntie aan vn bookviuw aan, zodat we die kunnen hergebruiken
        BookView view = new BookView();

        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(@" 
            ============================================================
            ||                                                        ||
            ||                 B I E B   ·   H R                      ||
            ||                                                        ||
            ||          Hogeschool Rotterdam – Informatica            ||
            ||                                                        ||
            ============================================================");
            Console.ResetColor();

            Console.WriteLine("1. Inloggen");
            Console.WriteLine("2. Boekgegevens wijzigen");
            Console.WriteLine("3. Volledige boekenlijst bekijken");
            Console.WriteLine("4. Nieuw account registreren.");
            Console.WriteLine("5. Zoek boeken");
            Console.WriteLine("6. Blader door genres");
            Console.WriteLine("0. Afsluiten");
            Console.Write("\nKnies een optie: ");

            string input = Console.ReadLine();

            // ik heb het veranders naar switch 
            // dat is wat makkelijker denk ik
            
            switch (input)
            {
                case "1":
                    UserLogin.Start();
                    break;

                case "2":
                    BookEdit bEdit = new BookEdit();
                    bEdit.Start();
                    break;
                case "3":
                    view.ShowAllBooks();
                    break;
                case "4":
                    UserRegister.Start();
                    break;
                case "5":
                    view.LiveSearchBooks();
                    break;
                case "0":
                    Console.WriteLine("\nTot ziens!");
                    return;

                default:
                    Console.WriteLine("\nngeldige keuze. Druk op een toets om opnieuw te proberen...");
                    Console.ReadKey();
                    break;
            }

        }
    }
}