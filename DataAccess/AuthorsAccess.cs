using Microsoft.Data.Sqlite;
using Dapper;

public static class AuthorsAccess
{
    private static string Table = "Authors";

    public static AuthorModel? GetById(int id)
    {
        using var connection = new SqliteConnection($"Data Source=DataSources/project.db");
        string sql = $"SELECT * FROM {Table} WHERE id = @Id";
        return connection.QueryFirstOrDefault<AuthorModel>(sql, new { Id = id });
    }
}