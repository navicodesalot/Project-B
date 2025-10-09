using Microsoft.Data.Sqlite;
using Dapper;

class AccountsAccess
{
    private readonly SqliteConnection _connection = new($"Data Source=DataSources/project.db");
    private const string Table = "Accounts";

    public void Write(AccountModel account)
    {
        // voeg account toe
        string sql = $"INSERT INTO {Table} (email, password, firstname, lastname, dob, reservations, fines) VALUES (@Email, @Password, @FirstName, @LastName, @DOB, @Reservations, @Fines)";
        _connection.Execute(sql, account);
    }

    public AccountModel? GetByEmail(string email)
    {
        // haal account op via email
        string sql = $"SELECT * FROM {Table} WHERE email = @Email";
        return _connection.QueryFirstOrDefault<AccountModel>(sql, new { Email = email });
    }

    public void Update(AccountModel account)
    {
        // update account
        string sql = $"UPDATE {Table} SET email = @Email, password = @Password, firstname = @FirstName, lastname = @LastName, dob = @DOB, reservations = @Reservations, fines = @Fines WHERE id = @Id";
        _connection.Execute(sql, account);
    }

    public void Delete(AccountModel account)
    {
        // verwijder account
        string sql = $"DELETE FROM {Table} WHERE id = @Id";
        _connection.Execute(sql, new { account.Id });
    }
}