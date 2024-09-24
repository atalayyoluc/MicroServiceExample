namespace OrderService.Domain.Entities;

public class ProductDetails
{
    public int Id { get; set; } 
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
}
