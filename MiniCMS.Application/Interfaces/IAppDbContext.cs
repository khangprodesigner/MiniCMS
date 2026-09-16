using Microsoft.EntityFrameworkCore;
using MiniCMS.Domain.Entities;

namespace MiniCMS.Application.Interfaces;

public interface IAppDbContext
{
    DbSet<Customer> Customers { get; }
    DbSet<User> Users { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}