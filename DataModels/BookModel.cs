public class BookModel
{
    public Int64 Id { get; set; }
    public string Title { get; set; }
    public string Genre { get; set; }
    public int AuthorId { get; set; }

    public BookModel(Int64 id, string title, string genre, int authorId)
    {
        Id = id;
        Title = title;
        Genre = genre;
        AuthorId = authorId;
    }
}