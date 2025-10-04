public class BookModel
{
    public Int64 Id { get; set; }
    public string Title { get; set; }
    public string Genre { get; set;  }
    public string Author { get; set; }

    public BookModel(Int64 id, string title, string genre, string author)
    {
        Id = id;
        Title = title;
        Genre = genre;
        Author = author;
    }
}