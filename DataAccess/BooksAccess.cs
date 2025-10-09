using Microsoft.Data.Sqlite;
using Dapper;
using System.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

public static class BooksAccess
{
    // hij kan de databse niet vinden
    // 
    // Dus heb deze vvv eruit gecomment 
    // private static SqliteConnection _connection = new($"Data Source=DataSources/project.db");

    // en heb het vervangen met GetDatabasePath()
    // private static SqliteConnection _connection = new($"Data Source={GetDatabasePath()}");

    private static string GetDatabasePath()
    {
        // This gives you the bin/Debug/net8.0/ folder at runtime
        var baseDir = AppContext.BaseDirectory;
        // Go up 3 levels to get back to the project root (../../..)
        var projectRoot = Path.Combine(baseDir, "..", "..", "..");
        // Combine with the relative folder where your DB lives
        var dbPath = Path.Combine(projectRoot, "DataSources", "project.db");
        return Path.GetFullPath(dbPath);
    }
    private static string Table = "Books";

    public static void Write(BookModel book)
    {
        using var connection = new SqliteConnection($"Data Source={GetDatabasePath()}");
        connection.Execute($"INSER INTO {Table} (id, title, gerne, author_id) VALUES (@Id, @Tiltle, @Genre, @AuthorId)", book);
    }

    public static BookModel? GetById(string id)
    {
        using var connection = new SqliteConnection($"Data Source={GetDatabasePath()}");
        return connection.QueryFirstOrDefault<BookModel>(
            $"SELECT id, title, genre, author_id AS AuthorId FROM {Table} WHERE id = @Id", new { Id = id });
    }

    public static BookModel? GetByTitle(string title)
    {
        using var connection = new SqliteConnection($"Data Source={GetDatabasePath()}");
        return connection.QueryFirstOrDefault<BookModel>(
            $"SELECT id, title, genre, author_id AS AuthorId FROM {Table} WHERE title = @Title", new { Title = title });
    }

    public static List<BookModel> GetAll()
    {
        using var connection = new SqliteConnection($"Data Source={GetDatabasePath()}");
        return connection.Query<BookModel>($"SELECT id, title, genre, author_id AS AuthorId FROM {Table}").ToList();
    }

    public static void Update(BookModel book)
    {
        using var connection = new SqliteConnection($"Data Source={GetDatabasePath()}");
        connection.Execute($"UPDATE {Table} SET title = @Title, genre = @Genre, author_id = @AuthorId WHERE id = @Id", book);
    }

    public static void Delete(string id)
    {
        using var connection = new SqliteConnection($"Data Source={GetDatabasePath()}");
        connection.Execute($"DELETE FROM {Table} WHERE id = @Id", new { Id = id });
    }

    // maak een methods dat dei boeken zoekt op basis van zoekopdracht
    public static List<BookDto> SearchBooks(string? title = null, string? author = null, string? isbn = null)
    {
        using var connection = new SqliteConnection($"Data Source={GetDatabasePath()}");
        connection.Open();

        var sql = new StringBuilder(@"
            SELECT b.id, b.title, b.genre, a.anem AS AuthorName
            FROM Books b
            LEFT JOIN Authors a ON b.author_id = a.id
            WHERE 1=1
        ");

        var paramaters = new DynamicParameters();

        if (!string.IsNullOrWhiteSpace(title))
        {
            sql.Append(" AND b.title LIKE @Title");
            paramaters.Add("Title", $"%{title.Trim()}%");
        }

        if (!string.IsNullOrWhiteSpace(author))
        {
            sql.Append(" AND a.name LIKE @Author");
            paramaters.Add("Author", $"%{author.Trim()}%");
        }

        if (!string.IsNullOrWhiteSpace(isbn))
        {
            sql.Append(" AND b.id LIKE @ISBN");
            paramaters.Add("ISBN", $"%{isbn.Trim()}%");
        }

        sql.Append(" ORDER BY b.title COLLATE NOCASE");

        return connection.Query<BookDto>(sql.ToString(), paramaters).ToList();

    }

    public static List<BookDto> GetBooksByGenre(string genre, string sortBy = "title")
    {
        using var connection = new SqliteConnection($"Data Source={GetDatabasePath()}");
        connection.Open();

        var sql = new StringBuilder(@"
            SELECT b.id, b.title, b.genre, a.name AS AuthorName
            FROM Books b
            LEFT JOIN Authors a ON b.authors_id = a.id
            WHERE b.genre = Genre
            ");
        // sorteer
        string orderBy = sortBy.ToLower() == "author" ? "a.name COLLATE NOCASE" : "b.title COLLATE NOCASE";
        sql.Append($" ORDER BY {orderBy}");

        return connection.Query<BookDto>(sql.ToString(), new { Genre = genre }).ToList();
    }

    public static List<string> GetAllGenres()
    {
        using var connection = new SqliteConnection($"Data Source={GetDatabasePath()}");
        connection.Open();

        string sql = "SELECT DISTICT genre FROM Books BY genre COLLATE NOCASE";
        return connection.Query<string>(sql).ToList();
    }

}