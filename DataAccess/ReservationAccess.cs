using Microsoft.Data.Sqlite;
using Dapper;

public static class ReservationAccess
{
    private static string Table = "Reservations";

    private static SqliteConnection GetConnection()
    {
        // db connectie
        return new SqliteConnection($"Data Source=DataSources/project.db");
    }

    public static void Write(ReservationModel reservation)
    {
        // voeg reservering toe
        using var connection = GetConnection();
        string sql = $"INSERT INTO {Table} (userid, bookid, startdate, enddate) VALUES (@UserId, @BookId, @StartDate, @EndDate)";
        connection.Execute(sql, reservation);
    }

    public static List<ReservationModel> GetByUserId(int userId)
    {
        // alle reserveringen van user
        using var connection = GetConnection();
        string sql = $"SELECT * FROM {Table} WHERE userid = @UserId";
        return connection.Query<ReservationModel>(sql, new { UserId = userId }).ToList();
    }

    public static ReservationModel? GetActiveByBookId(string bookId)
    {
        // check of boek nu is uitgeleend
        using var connection = GetConnection();
        string sql = $"SELECT * FROM {Table} WHERE bookid = @BookId AND enddate > @Now";
        return connection.QueryFirstOrDefault<ReservationModel>(sql, new { BookId = bookId, Now = DateTime.Now });
    }
}