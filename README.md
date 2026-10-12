# BÁO CÁO GIỮA KỲ: HỆ THỐNG QUẢN LÝ & LUYỆN THI TOEIC READING

> **ĐỒ ÁN GIỮA KỲ - PHÁT TRIỂN ỨNG DỤNG DOANH NGHIỆP & DI ĐỘNG**  
> **Kiến trúc N-Layer với ASP.NET Boilerplate (.NET 9) & Flutter**

---

## 👨‍🎓 THÔNG TIN SINH VIÊN

* **Họ và tên**: **Phan Tấn Phát**
* **Mã số sinh viên (MSSV)**: **2280602319**
* **Lớp**: **23DTHC7**
* **Link Thiết kế Giao diện Figma**: [Xem Thiết kế Figma tại đây](https://www.figma.com/design/HYWMDWnYFypKKMnjnileHJ/Untitled?node-id=0-1&t=Fto0y1a4s9yoCBeR-1)

---

## 📁 CẤU TRÚC THƯ MỤC DỰ ÁN

```text
Midterm_ToeicApp/
├── Backend/                                # Hệ thống Backend ASP.NET Boilerplate (.NET 9)
│   └── 10.2.0/aspnet-core/
│       ├── src/
│       │   ├── ToeicReadingApp.Core/       # Domain Entities (Exam, ExamPart, QuestionGroup, Question, ExamAttempt...)
│       │   ├── ToeicReadingApp.EntityFrameworkCore/ # EF Core DbContext, Migrations (Database: ToeicReadingAppDb)
│       │   ├── ToeicReadingApp.Application/ # Application Services & DTOs
│       │   ├── ToeicReadingApp.Web.Core/   # Web Core, Authentication & Token Auth
│       │   └── ToeicReadingApp.Web.Mvc/    # Portal Quản trị viên (Upload Word, Preview, AJAX Edit)
│       └── ToeicReadingApp.sln             # Solution Visual Studio
│
├── Frontend/                               # Ứng dụng Di động & Web Client (Flutter)
│   ├── lib/
│   │   ├── models/                         # Data Models (Exam, Question, Submit, Result)
│   │   ├── screens/                        # ExamListScreen, ExamTakingScreen, ExamResultScreen
│   │   ├── services/                       # ApiService (kết nối Backend localhost:5000), MockData
│   │   ├── theme/                          # Modern AppTheme
│   │   └── widgets/                        # QuestionCard, PassageCard, QuestionGridDialog...
│   └── pubspec.yaml                        # Quản lý dependencies (http, provider...)
│
├── videos/                                 # Video quay thực tế các chức năng hoàn chỉnh
│   ├── Giao diện Admin Import đề thi 1.mp4 # Video demo Upload file Word đề thi chuẩn
│   ├── Giao diện Admin Import đề thi 2.mp4 # Video demo Báo lỗi định dạng (All-or-Nothing)
│   ├── Giao diện Admin Import đề thi 3.mp4 # Video demo Xem trước & Sửa câu hỏi/bài đọc
│   ├── Giao diện Users.mp4                 # Video demo Luyện thi và nộp bài trên Flutter
│   └── Sơ đồ database.mp4                  # Video thuyết minh Sơ đồ Cơ sở Dữ liệu
│
├── BaoCao-PhanTanPhat-2280602319.docx       # File Word Báo cáo đồ án giữa kỳ chính thức
├── Database_ER_Diagram.html                # Sơ đồ Cơ sở Dữ liệu Quan hệ ER Diagram (Tương tác SVG)
├── DeThi_TOEIC_Reading_Test05_Tagged.docx  # File Word đề thi Test 05 chuẩn 100 câu có gắn thẻ Tags
├── DeThi_TOEIC_Loi_DinhDang.docx           # File Word mẫu cố tình sai định dạng để test kiểm duyệt
└── .gitignore                              # Cấu hình Git an toàn (loại bỏ thư mục build/cache)
```

---

## 🌟 TÍNH NĂNG NỔI BẬT CỦA HỆ THỐNG

### 1. Phân hệ Quản trị viên Backend (ASP.NET Core MVC)
* **Cơ chế Import All-or-Nothing**: Kiểm duyệt toàn vẹn cấu trúc file Word (.docx) 100 câu bằng thư viện `DocumentFormat.OpenXml`. Nếu phát hiện bất kỳ lỗi định dạng nào (thiếu thẻ, sai số thứ tự, thiếu đáp án đúng...), quy trình hủy bỏ hoàn toàn trong bộ nhớ (In-Memory), không lưu rác vào CSDL SQL Server.
* **Hỗ trợ trọn vẹn 3 phần TOEIC Reading**:
  * **Part 5 (Câu 101 - 130)**: 30 câu hỏi điền từ độc lập.
  * **Part 6 (Câu 131 - 146)**: 4 nhóm bài đọc hoàn thành đoạn văn.
  * **Part 7 (Câu 147 - 200)**: 54 câu hỏi đọc hiểu kèm văn bản, email, bảng biểu và trích xuất hình ảnh.
* **Giao diện Quản trị Chuyên nghiệp**:
  * Chuẩn **AdminLTE 3.2**, **Bootstrap 4.6**, **FontAwesome 6**, **Google Fonts Inter**.
  * Chức năng **Xem trước & Chỉnh sửa trực quan** câu hỏi, đoạn văn, hình ảnh qua Modal AJAX không cần tải lại trang.

### 2. Ứng dụng Luyện thi Di động (Flutter Client)
* **Màn hình Danh sách Đề thi (Mục 5)**: Tìm kiếm đề thi theo tên, lọc theo trạng thái, hiển thị thời gian và số câu hỏi.
* **Màn hình Làm bài thi (Mục 6)**:
  * Đồng hồ đếm ngược 75:00 tự động nộp bài khi hết giờ.
  * Tab điều hướng mượt mà giữa Part 5, Part 6 và Part 7.
  * **Lưới câu hỏi (Question Grid Sheet)**: 100 câu hỏi trực quan, đánh dấu trạng thái đã làm / chưa làm và nhảy nhanh đến câu hỏi bất kỳ.
* **Màn hình Kết quả & Chấm điểm (Mục 7)**:
  * Điểm số quy đổi chuẩn TOEIC Reading (thang điểm tối đa **495**).
  * Thống kê tỷ lệ phần trăm và số lượng câu Đúng / Sai / Bỏ qua.
  * Bảng **Answer Review** chi tiết từng câu (đáp án thí sinh chọn vs đáp án đúng của hệ thống).

---

## 🚀 HƯỚNG DẪN KHỞI CHẠY DỰ ÁN

### 1. Cấu hình Cơ sở dữ liệu:
* Đảm bảo SQL Server đang hoạt động tại `localhost`.
* Connection string mặc định trong `appsettings.json`:
  ```json
  "ConnectionStrings": {
    "Default": "Server=localhost; Database=ToeicReadingAppDb; Trusted_Connection=True; TrustServerCertificate=True;"
  }
  ```
* Chạy cập nhật CSDL (nếu cần):
  ```bash
  cd Backend/10.2.0/aspnet-core
  dotnet ef database update --project src/ToeicReadingApp.EntityFrameworkCore --startup-project src/ToeicReadingApp.Web.Mvc
  ```

### 2. Khởi chạy Backend Web Admin:
```bash
cd Backend/10.2.0/aspnet-core
dotnet run --project src/ToeicReadingApp.Web.Mvc/ToeicReadingApp.Web.Mvc.csproj
```
* **Địa chỉ Quản trị**: `http://localhost:5000/Exams`
* **Tài khoản đăng nhập**: `admin`
* **Mật khẩu**: `123qwe`

### 3. Khởi chạy Ứng dụng Flutter:
```bash
cd Frontend
flutter run -d chrome --web-port 3000
```
* **Địa chỉ Client**: `http://localhost:3000` (hoặc chạy trên Windows Desktop / Android Emulator kết nối trực tiếp vào `http://localhost:5000`).
