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

    public static void UpdateNextReservationStart(string bookId, DateTime newEndDate)
    {
        using var connection = GetConnection();
        // stel dat een boek vroeg terug is
        // dan wordt dat geupdate
        string sql = $"SELECT * FROM Reservations WHERE bookid = @BookId AND startdate > @NewEndDate ORDER BY startdate LIMIT 1";
        var nextReservation = connection.QueryFirstOrDefault<ReservationModel>(sql, new { BookId = bookId, NewEndDate = DateTime.MinValue });
        if (nextReservation != null)
        {
            DateTime newStart = newEndDate.AddDays(2); // 2 dagen na terugbrengen
            DateTime newEnd = newStart.AddDays(21);
            string updateSql = $"UPDATE Reservations SET startdate = @NewStart, enddate = @NewEnd WHERE id = @Id";
            connection.Execute(updateSql, new { NewStart = newStart, NewEnd = newEnd, Id = nextReservation.Id });
        }
    }

    public static List<ReservationModel> GetByBookId(string bookId)
    {
        using var connection = GetConnection();
        string sql = $"SELECT * FROM Reservations WHERE bookid = @BookId";
        return connection.Query<ReservationModel>(sql, new { BookId = bookId }).ToList();
    }
}