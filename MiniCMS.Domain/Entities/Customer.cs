using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniCMS.Domain.Enums;

namespace MiniCMS.Domain.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        // Mã KH (Unique, không được sửa sau khi tạo)
        public string CustomerCode { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        // Email (Unique)
        public string Email { get; set; } = string.Empty;
        // SĐT VN (10 chữ số)
        public string PhoneNumber { get; set; } = string.Empty;
        // Ngày sinh (KH cá nhân >= 15 tuổi)
        public DateOnly DateOfBirth { get; set; }
        // Trạng thái hoạt động
        public CustomerStatus Status { get; set; } = CustomerStatus.Active;

        // Thuộc tính Soft-Delete & Audit cho nghiệp vụ ngân hàng
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
