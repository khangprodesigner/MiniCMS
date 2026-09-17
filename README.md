# Hệ thống Quản lý Khách hàng cơ bản (Mini Customer Management System - MiniCMS)

Dự án của vòng sơ tuyển: Hệ thống quản lý khách hàng cơ bản, tuân thủ mô hình Clean Architecture, cơ chế xác thực JWT, giao diện tương tác Blazor Server tích hợp thư viện UI Component MudBlazor.

Người thực hiện: `Phạm Minh Khang`

---

## Phần 1: Yêu Cầu Môi Trường (Prerequisites)

Để chạy được dự án, máy tính cần cài đặt sẵn:
* [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* [SQL Server](https://www.microsoft.com/sql-server) (SQL Server Express, LocalDB hoặc bản Enterprise)
* [Visual Studio](https://visualstudio.microsoft.com/) (với workload *ASP.NET and web development*)

---

## Phần 2: Hướng Dẫn Cách Chạy Project:

### Bước 1: Clone Repository
```
git clone https://github.com/khangprodesigner/MiniCMS.git
cd MiniCMS
```

### Bước 2: Thiết lập Chuỗi kết nối CSDL (Connection String)
Mở file `MiniCMS.Api/appsettings.json` và kiểm tra cấu hình chuỗi kết nối `DefaultConnection`
``` json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=BẠN_SỬA_Ở_ĐÂY;Database=MiniCms_Db;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```
* Nếu sử dụng SQL Server LocalDB, có thể đổi `Server=(localdb)\\mssqllocaldb`.
* Nhưng thường thì developer chúng ta đều cài sẵn SQL Server trong máy, Server sẽ có dạng `Server=localhost\\(TÊN_SERVER_CỦA_BẠN)`
* Nếu không nhớ tên Server, có thể mở SSMS để kiểm tra lại:
<img width="509" height="715" alt="image" src="https://github.com/user-attachments/assets/0e103ad9-5a41-4182-aee5-4f4752a8bf0e" />

* Như vậy, tham số của tôi sẽ là `Server=localhost\\MSSQLSERVER03`
> [!IMPORTANT]
> Lưu ý: lấy tiền tố `localhost\\` và nối tên Server vào, không copy cả cụm Server Name trong SSMS (Ví dụ: `KHANG\MSSQLSERVER03`)

### Bước 3: Chạy lệnh Migration khởi tạo Cơ sở dữ liệu
Cách 1: Sử dụng .NET Core CLI (Terminal):
Mở terminal tại thư mục gốc của Solution và chạy lệnh:
``` bash
dotnet ef database update --project MiniCMS.Infrastructure --startup-project MiniCMS.Api
```
Cách 2 (tác giả dùng): Sử dụng Package Manager Console trong Visual Studio
1. Mở Solution bằng Visual Studio
2. Vào Tools $\rightarrow$ NuGet Package Manager $\rightarrow$ Package Manager Console.
   
   <img width="930" height="516" alt="image" src="https://github.com/user-attachments/assets/ae330726-f9fc-4821-84d9-b10ea261cefe" />
3. Chọn `MiniCMS.Api` làm Startup project
   
   <img width="359" height="678" alt="image" src="https://github.com/user-attachments/assets/716c2831-545d-4f9d-8150-be352b231cf6" />

4. Chọn Default project trong cửa sổ Package Manager góc dưới là `MiniCMS.Infrastructure`
   <img width="704" height="231" alt="image" src="https://github.com/user-attachments/assets/5f1ffdd2-4f96-4260-a473-316d9e37cd19" />

5. Chạy lệnh
```pwsh
Update-Database
```
> Ghi chú: Quá trình Migration sẽ tự động khởi tạo 2 bảng (Users, Customers), cấu hình lọc xóa mềm (IsDeleted = false), thiết lập chỉ mục duy nhất cho CustomerCode và nạp sẵn tài khoản Quản trị viên mặc định.

### Bước 4: Hướng dẫn chạy ứng dụng bằng Visual Studio
Hệ thống bao gồm 2 ứng dụng cần chạy đồng thời:
1. Backend API
2. Frontend Web
 
**Cách chạy 2 ứng dụng bằng Visual Studio (khuyên dùng)**
1. Chuột phải vào Solution 'MiniCMS', chọn **Configure Startup Projects...**
   
   <img width="358" height="501" alt="image" src="https://github.com/user-attachments/assets/37a2ef96-987d-41cc-a019-4061b7fa8f5d" />
2. Chọn Multiple startup projects:
  - Đặt action của `MiniCMS.Api` thành **Start**
  - Đặt action của `MiniCMS.Web` thành **Start**
<img width="798" height="540" alt="image" src="https://github.com/user-attachments/assets/6c0bbdad-1ccd-468d-b92c-8db85abac4c2" />

3. Nhấn Apply $\rightarrow$ **OK**
4. Bấm F5 (hoặc `Ctrl + F5`) để Visual Studio tự động chạy cả API lẫn giao diện Web trên trình duyệt.

### Bước 5: Tài khoản đăng nhập mặc định
Hệ thống đã tạo tài khoản Admin cứng để kiểm thử:
* Tên đăng nhập (Username): `admin`
* Mật khẩu (Password): `Cep@123`
