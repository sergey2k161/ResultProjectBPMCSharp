using System.ComponentModel.DataAnnotations.Schema;

namespace ShelekhovResult.DataBase.Models;

/// <summary>
/// Модель пользователя
/// </summary>
public class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public string UserDomainName { get; set; }
    
    public ICollection<Data> Data { get; set; }
}