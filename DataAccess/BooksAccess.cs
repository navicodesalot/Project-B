using Microsoft.Data.Sqlite;
using Dapper;
using System.Data.Common;

public static class BooksAccess
{
    private static SqliteConnection _connection = new($"Data Source=DataSources/project.db");
    private static string Table = "Books";

    public static void Write(BookModel book)
    {
        string sql = $"INSERT INTO {Table} (id, title, genre, author_id) VALUES (@Id, @Title, @Genre, @AuthorId)";
        _connection.Execute(sql, book);
    }

    public static BookModel? GetById(string id)
    {
        string sql = $"SELECT * FROM {Table} WHERE id = @Id";
        return _connection.QueryFirstOrDefault<BookModel>(sql, new { Id = id });
    }

    public static BookModel? GetByTitle(string title)
    {
        string sql = $"SELECT * FROM {Table} WHERE title = @Title";
        return _connection.QueryFirstOrDefault<BookModel>(sql, new { Title = title });
    }

    public static List<BookModel> GetAll()
    {
        string sql = $"SELECT * FROM {Table}";
        return _connection.Query<BookModel>(sql).ToList();
    }

    public static void Update(BookModel book)
    {
        string sql = $"UPDATE {Table} SET title = @Title, genre = @Genre, author_id = @AuthorId WHERE id = @Id";
        _connection.Execute(sql, book);
    }

    public static void Delete(string id)
    {
        string sql = $"DELETE FROM {Table} WHERE id = @Id";
        _connection.Execute(sql, new { Id = id });
    }
}