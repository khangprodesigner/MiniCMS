using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniCMS.Application.DTOs;

namespace MiniCMS.Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> AuthenticateAsync(LoginRequestDto request); 
    }
}
