class AuthorModel
{
    public Int64 Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public AuthorModel(Int64 id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
}