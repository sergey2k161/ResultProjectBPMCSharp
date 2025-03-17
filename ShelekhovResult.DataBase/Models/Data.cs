using System.ComponentModel.DataAnnotations.Schema;

namespace ShelekhovResult.DataBase.Models;

/// <summary>
/// Модель Сделок
/// </summary>
public class Data
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    
    public User User { get; set; }
    
    public string? Entity { get; set; }
}