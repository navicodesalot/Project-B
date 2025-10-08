public class ReservationLogic
{
    public bool CanReserve(int userId)
    {
        // check hoeveel boeken user heeft, max 10
        List<ReservationModel> reservations = ReservationAccess.GetByUserId(userId);
        int activeCount = reservations.Count(r => r.EndDate > DateTime.Now);
        return activeCount < 10;
    }

    public bool IsBookAvailable(string bookId)
    {
        // check of boek niet al is uitgeleend
        ReservationModel? active = ReservationAccess.GetActiveByBookId(bookId);
        return active == null;
    }

    public void ReserveBook(int userId, string bookId)
    {
        // maak nieuwe reservering aan
        DateTime start = DateTime.Now;
        DateTime end = start.AddDays(21); // 3 weken :P
        ReservationModel reservation = new ReservationModel(0, userId, bookId, start, end);
        ReservationAccess.Write(reservation);
    }
}