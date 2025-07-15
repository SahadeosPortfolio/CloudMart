using Moq;
using Moq.EntityFrameworkCore;
using Products.Domain.Entities;
using Products.Infrastructure.Data;
using Products.Infrastructure.Repositories;

namespace Products.Infrastructure.Unit.Tests.Repositories;

public class ProductRepositoryTests
{
    private readonly Mock<ApplicationDbContext> _dbContextMock;
    private readonly ProductRepository _productRepository;

    public ProductRepositoryTests()
    {
        _dbContextMock = new Mock<ApplicationDbContext>();
        _productRepository = new ProductRepository(_dbContextMock.Object);
    }

    //todo : add tests for ProductRepository methods
}