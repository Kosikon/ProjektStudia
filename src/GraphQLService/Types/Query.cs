using Infrastructure.Persistence;
using System.Linq;

public class Query
{
    public IQueryable<User> GetUsers([Service] ApplicationDbContext context) =>
        context.Users;

    public IQueryable<Product> GetProducts([Service] ApplicationDbContext context) =>
        context.Products;
}