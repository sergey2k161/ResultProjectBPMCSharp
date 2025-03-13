namespace ShelekhovResult.DataBase.Models;

public class User
{
    public Guid Id { get; set; }

    public string UserDomainName { get; set; }
    
    public ICollection<Data> Data { get; set; }
}