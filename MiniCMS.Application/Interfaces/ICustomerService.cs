using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniCMS.Application.DTOs;

namespace MiniCMS.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDto>> GetAllAsync(string? searchTerm = null, bool? isActive = null);
        Task<CustomerDto?> GetByIdAsync(int id);
        Task<CustomerDto> CreateAsync(CreateCustomerDto dto);
        Task<CustomerDto?> UpdateAsync(int id, UpdateCustomerDto dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistByCodeAsync(string CustomerCode);
        Task<bool> ExistByEmailAsync(string email, int? excludeId = null);
    }
}
