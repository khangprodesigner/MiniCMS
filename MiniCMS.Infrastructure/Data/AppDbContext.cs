using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MiniCMS.Application.Interfaces;
using MiniCMS.Domain.Entities;


namespace MiniCMS.Infrastructure.Data
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Cấu hình bảng Customers
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customers");
                entity.HasKey(c => c.Id);

                // CustomerCode: Unique Index, độ dài tối đa 20
                entity.Property(c => c.CustomerCode).HasMaxLength(20).IsRequired();
                entity.HasIndex(c => c.CustomerCode).IsUnique();

                // FullName: NVARCHAR(100), Index hỗ trợ tìm kiếm
                entity.Property(c => c.FullName).HasMaxLength(100).IsRequired();
                entity.HasIndex(c => c.FullName);

                // Email: VARCHAR(100), Unique Index
                entity.Property(c => c.Email).HasMaxLength(100).IsRequired();
                entity.HasIndex(c => c.Email).IsUnique();

                // PhoneNumber: VARCHAR(15), đánh Index để tìm kiếm nhanh
                // SDT VN chỉ 10 chữ số nhưng đôi khi lưu dạng +84 sẽ lỗi
                // Cần tối thiểu 15 chữ số theo Chuẩn viễn thông quốc tế (ITU-T E.164)
                entity.Property(c => c.PhoneNumber).HasMaxLength(15).IsRequired();
                entity.HasIndex(c => c.PhoneNumber);

                // DOB: Lưu dạng DATE trong SQL Server
                entity.Property(c => c.DateOfBirth)
                    .HasColumnType("date")
                    .IsRequired();

                // IsActive: Kiểu BIT, mặc định là 1 (Active)
                entity.Property(c => c.IsActive)
                    .HasDefaultValue(true)
                    .IsRequired();

                // Soft-Delete & Audit Fields
                entity.Property(c => c.IsDeleted).HasDefaultValue(false);

                // DefaultValueSql có nghĩa là câu lệnh này của SQL
                entity.Property(c => c.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

                // Mọi truy vấn ngầm định sẽ chỉ lấy IsDeleted == false
                entity.HasQueryFilter(c => !c.IsDeleted);
            });

            // 2. Cấu hình bảng Users và Seed sẵn tài khoản Admin mặc định
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(u => u.Id);
                // Username
                entity.Property(u => u.Username).HasMaxLength(50).IsRequired();
                entity.HasIndex(u => u.Username).IsUnique();
                // Password
                entity.Property(u => u.PasswordHash).HasMaxLength(256).IsRequired();
                entity.Property(u => u.FullName).HasMaxLength(100).IsRequired();
                entity.Property(u => u.Role).HasMaxLength(20).IsRequired();

                // Seed tài khoản Admin cứng
                entity.HasData(new User
                {
                    Id = 1,
                    Username = "admin",
                    PasswordHash = "$2a$11$lHHnA2kCMZmnFh9/8BR9FOQam04.ipAa..NK9t1vaeCPFhy1lphty",
                    FullName = "System Administrator",
                    Role = "Admin"
                });
            });
        }

        // Tự động gán thời gian UpdatedAt khi có thao tác chỉnh sửa
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<Customer>()
                .Where(e => e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
