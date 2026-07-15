namespace PrintHub.Database.Models;

public class InventoryTransaction : BaseEntity
{
    public string Item_Type { get; set; } = null!; // "Filament" or "Material"
    public int Item_ID { get; set; }

    public decimal Change_Amount { get; set; }
    public string Reason { get; set; } = null!;    // PrintUsage, ManualAdjustment, Refill
    public DateTime Timestamp { get; set; }
}
