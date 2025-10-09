BookEdit bEdit = new BookEdit();
BookView view = new BookView();

var StartMenu = new Menu(
    new List<string> {
        "Inloggen",
        "Boekgegevens wijzigen",
        "Volledige boekenlijst bekijken",
        "Nieuw account registreren.",
        "Zoek boeken",
        "Blader door genres",
        "Afsluiten"
        },
    new List<Action> {
        UserLogin.Start,
        bEdit.Start,
        view.ShowAllBooks,
        UserRegister.Start,
        view.LiveSearchBooks,
        Menu.Placeholder, // place holder voor bladeren door genres
        Menu.Afsluiten
        });

StartMenu.Link();