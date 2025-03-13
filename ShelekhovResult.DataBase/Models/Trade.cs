namespace ShelekhovResult.DataBase.Models;

/// <summary>
/// Модель сделки
/// </summary>
public class Trade
{
    public decimal Amount { get; set; }
    
    public DateTime CreatedAt { get; set; }
}