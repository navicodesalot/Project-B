public class ReservationModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string BookId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public ReservationModel(int id, int userId, string bookId, DateTime startDate, DateTime endDate)
    {
        Id = id;
        UserId = userId;
        BookId = bookId;
        StartDate = startDate;
        EndDate = endDate;
    }

    // dapper wil deze
    public ReservationModel() { }
}