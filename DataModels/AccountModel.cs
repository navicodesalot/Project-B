public class AccountModel
{
    public Int64 Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string DOB { get; set; }
    public int Reservations { get; set; }
    public double Fines { get; set; }

    public AccountModel(Int64 id, string email, string password, string firstName, string lastName, string dob, int reservations, double fines)
    {
        Id = id;
        Email = email;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
        DOB = dob;
        Reservations = reservations;
        Fines = fines;
    }

    // this is for dapper it needs it
    public AccountModel() { }
}