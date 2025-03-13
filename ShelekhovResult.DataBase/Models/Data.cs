namespace ShelekhovResult.DataBase.Models;

/// <summary>
/// Модель Сделок
/// </summary>
public class Data
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    
    public User User { get; set; }
    
    public string? Entity { get; set; }
}