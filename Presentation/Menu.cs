static class Menu
{
    //This shows the menu. You can call back to this method to show the menu again
    //after another presentation method is completed.
    //You could edit this to show different menus depending on the user's role
    public static void Start()
    {
        while (true)
        {
            Console.WriteLine("Enter 1 to login");
            Console.WriteLine("Enter 2 to change book details");
            Console.WriteLine("Enter 3 to view all books"); 

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
            else
            {
                Console.WriteLine("Invalid input");
                return;
            }
        }
    }
}