//This class is not static so later on we can use inheritance and interfaces
public class BooksLogic
{
    private readonly AccountsAccess _access = new();

    public BooksLogic()
    {
        // place holder
    }

    public BookModel GetByTitle(string title)
    {
        return BooksAccess.GetByTitle(title);
    }

    public void UpdateBook(BookModel book)
    {
        BooksAccess.Update(book);
    }

    public BookModel GetById(int id)
    {
        return BooksAccess.GetById(id);
    }

    public List<BookModel> GetAllBooks()
    {
        return BooksAccess.GetAll();
    }

}