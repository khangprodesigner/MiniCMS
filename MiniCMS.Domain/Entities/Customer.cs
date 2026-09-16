using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCMS.Domain.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        // Mã KH (Unique)
        public string CustomerCode { get; set; } = string.Empty;
        // Họ và tên
        public string FullName { get; set; } = string.Empty;
        // Email (Unique)
        public string Email { get; set; } = string.Empty;
        // Số điện thoại
        public string PhoneNumber { get; set; } = string.Empty;
        // Ngày sinh (KH cá nhân >= 15 tuổi)
        public DateOnly DateOfBirth { get; set; }
        // Trạng thái hoạt động (true: đang hoạt động, false: không hoạt động)
        public bool IsActive { get; set; } = true;

        // Thuộc tính Soft-Delete & Audit cho nghiệp vụ ngân hàng
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
