using Microsoft.EntityFrameworkCore;
using MiniCMS.Application.DTOs;
using MiniCMS.Application.Interfaces;
using MiniCMS.Domain.Entities;

namespace MiniCMS.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly IAppDbContext _context;

    public CustomerService(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CustomerDto>> GetAllAsync(string? searchTerm = null, bool? isActive = null)
    {
        // Nhờ Query Filter trong AppDbContext, IsDeleted == false đã được tự động áp dụng
        var query = _context.Customers.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.Trim().ToLower();
            query = query.Where(c => c.CustomerCode.ToLower().Contains(term)
                                  || c.FullName.ToLower().Contains(term)
                                  || c.PhoneNumber.Contains(term));
        }

        if (isActive.HasValue)
        {
            query = query.Where(c => c.IsActive == isActive.Value);
        }

        return await query
            .OrderByDescending(c => c.CreatedAt)
            .Select(c => new CustomerDto
            {
                Id = c.Id,
                CustomerCode = c.CustomerCode,
                FullName = c.FullName,
                Email = c.Email,
                PhoneNumber = c.PhoneNumber,
                DateOfBirth = c.DateOfBirth,
                IsActive = c.IsActive,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            })
            .ToListAsync();
    }

    public async Task<CustomerDto?> GetByIdAsync(int id)
    {
        var c = await _context.Customers.FindAsync(id);
        if (c == null) return null;

        return new CustomerDto
        {
            Id = c.Id,
            CustomerCode = c.CustomerCode,
            FullName = c.FullName,
            Email = c.Email,
            PhoneNumber = c.PhoneNumber,
            DateOfBirth = c.DateOfBirth,
            IsActive = c.IsActive,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt
        };
    }

    public async Task<CustomerDto> CreateAsync(CreateCustomerDto dto)
    {
        var customer = new Customer
        {
            CustomerCode = dto.CustomerCode.Trim().ToUpper(),
            FullName = dto.FullName.Trim(),
            Email = dto.Email.Trim().ToLower(),
            PhoneNumber = dto.PhoneNumber.Trim(),
            DateOfBirth = dto.DateOfBirth,
            IsActive = dto.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        return new CustomerDto
        {
            Id = customer.Id,
            CustomerCode = customer.CustomerCode,
            FullName = customer.FullName,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber,
            DateOfBirth = customer.DateOfBirth,
            IsActive = customer.IsActive,
            CreatedAt = customer.CreatedAt,
            UpdatedAt = customer.UpdatedAt
        };
    }

    public async Task<CustomerDto?> UpdateAsync(int id, UpdateCustomerDto dto)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null) return null;

        // CustomerCode không được cập nhật
        customer.FullName = dto.FullName.Trim();
        customer.Email = dto.Email.Trim().ToLower();
        customer.PhoneNumber = dto.PhoneNumber.Trim();
        customer.DateOfBirth = dto.DateOfBirth;
        customer.IsActive = dto.IsActive;

        await _context.SaveChangesAsync();

        return new CustomerDto
        {
            Id = customer.Id,
            CustomerCode = customer.CustomerCode,
            FullName = customer.FullName,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber,
            DateOfBirth = customer.DateOfBirth,
            IsActive = customer.IsActive,
            CreatedAt = customer.CreatedAt,
            UpdatedAt = customer.UpdatedAt
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null) return false;

        // Soft-delete
        customer.IsDeleted = true;
        customer.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistByCodeAsync(string customerCode)
    {
        return await _context.Customers.AnyAsync(c => c.CustomerCode.ToLower() == customerCode.Trim().ToLower());
    }

    public async Task<bool> ExistByEmailAsync(string email, int? excludeId = null)
    {
        var query = _context.Customers.Where(c => c.Email.ToLower() == email.Trim().ToLower());
        if (excludeId.HasValue)
        {
            query = query.Where(c => c.Id != excludeId.Value);
        }
        return await query.AnyAsync();
    }
}