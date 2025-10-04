class BookModel
{
    public Int64 Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }

    public BookModel(Int64 id, string title, string author)
    {
        Id = id;
        Title = title;
        Author = author;
    }
}