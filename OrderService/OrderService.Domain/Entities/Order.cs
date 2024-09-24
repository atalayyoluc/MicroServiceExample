using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using OrderService.Domain.Enums;

namespace OrderService.Domain.Entities;
public class Order
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public int CustomerId { get; set; }
    public List<ProductDetails> Products { get; set; }
    public decimal TotalPrice { get; set; }
    public OrderStatus Status { get; set; }
}