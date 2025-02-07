namespace PaypalExampleApp.Models;

public class PuteCommandModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal PriceHour { get; set; }

    public decimal Hours { get; set; }

}