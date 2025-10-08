public class AuthorsLogic
{
    public AuthorModel? GetById(int id)
    {
        return AuthorsAccess.GetById(id);
    }
}