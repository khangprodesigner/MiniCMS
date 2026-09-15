using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniCMS.Domain.Entities;

namespace MiniCMS.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        // Tên đăng nhập Admin
        public string Username { get; set; } = string.Empty;
        // Mật khẩu đã mã hoá
        public string PasswordHash { get; set; } = string.Empty;
        // Họ tên người quản trị
        public string FullName { get; set; } = string.Empty;
        // Phân quyền (mặc định Admin)
        public string Role { get; set; } = "Admin";
    }
}
