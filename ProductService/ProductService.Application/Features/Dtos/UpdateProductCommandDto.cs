namespace ProductService.Application.Features.Dtos;

public record UpdateProductCommandDto
(
string? Name,
decimal? Price,
int? Stock
);
