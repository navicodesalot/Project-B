public class BookModel
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Genre { get; set; }
    public int AuthorId { get; set; }

    public BookModel(string id, string title, string genre, int authorId)
    {
        Id = id;
        Title = title;
        Genre = genre;
        AuthorId = authorId;
    }

    // dont ask dapper needs it
    public BookModel() { }
}