namespace PaypalExampleApp.Models;

public record class PuteModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal PriceHour { get; set; }

}

