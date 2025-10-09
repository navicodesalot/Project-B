public class RegisterLogic
{
    private readonly AccountsAccess _access = new();

    public bool IsEmailUnique(string email) // is je mail wel special snowflake?
    {
        return _access.GetByEmail(email) == null;
    }

    // WE GAAN HET NIET HEBBEN OVER DEZE CODE I HAD TO DO WHAT I HAD TO DO
    public string FixDobFormat(string dobInput)
    {
        dobInput = dobInput.Trim();

        // geen -? geen zorgen ;)
        if (dobInput.Length == 8 && dobInput.All(char.IsDigit))
        {
            dobInput = dobInput.Insert(2, "-").Insert(5, "-");
        }
        else
        {
            dobInput = dobInput.Replace("/", "-").Replace(".", "-").Replace(" ", "-");
        }

        string[] parts = dobInput.Split('-');
        if (parts.Length == 3 && parts[0].Length == 2 && parts[1].Length == 2 && parts[2].Length == 4)
        {
            return dobInput;
        }
        else
        {
            return "";
            // activeert de lege check filter om opnieuw dob te vragen
        }
    }
}