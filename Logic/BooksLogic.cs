//This class is not static so later on we can use inheritance and interfaces
public class BooksLogic
{
    private readonly AccountsAccess _access = new();

    public BooksLogic()
    {
        // place holder
    }

    public List<BookDto> SearchBooks(string? title = null, string? author = null, string? isbn = null)
    {
        return BooksAccess.SearchBooks(title, author, isbn);

    }

    public List<BookDto> GetBookByGenre(string genre, string sortBy)
    {
        return BooksAccess.GetBooksByGenre(genre, sortBy);
    }

    public List<string> GetAllGernes()
    {
        return BooksAccess.GetAllGenres();
    }

    public BookModel GetByTitle(string title)
    {
        return BooksAccess.GetByTitle(title);
    }

    public void UpdateBook(BookModel book)
    {
        BooksAccess.Update(book);
    }

    public BookModel GetById(string id)
    {
        return BooksAccess.GetById(id);
    }

    public List<BookModel> GetAllBooks()
    {
        return BooksAccess.GetAll();
    }


} 