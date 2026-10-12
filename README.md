# Hệ Thống Quản Lý & Luyện Thi TOEIC Reading

Dự án hoàn chỉnh phân hệ Backend & Ứng dụng Di động / Web Luyện thi TOEIC Reading theo kiến trúc đa tầng chuẩn Doanh nghiệp (**N-Layer Architecture**) sử dụng **ASP.NET Boilerplate (ABP Framework v10.2.0, .NET 9)** và **Flutter**.

---

## 📁 Cấu Trúc Repository

```text
Midterm_ToeicApp/
├── Backend/                                # Hệ thống Backend ASP.NET Boilerplate (.NET 9)
│   └── 10.2.0/aspnet-core/
│       ├── src/
│       │   ├── ToeicReadingApp.Core/       # Domain Entities (Exam, ExamPart, QuestionGroup, Question, ExamAttempt...)
│       │   ├── ToeicReadingApp.EntityFrameworkCore/ # EF Core DbContext, Migrations
│       │   ├── ToeicReadingApp.Application/ # Application Services & DTOs
│       │   ├── ToeicReadingApp.Web.Core/   # Web Core & Auth Config
│       │   └── ToeicReadingApp.Web.Mvc/    # Admin Web MVC Portal (Upload, Preview, Edit)
│       └── ...
├── Frontend/                               # Ứng dụng Flutter Client (Web & Mobile)
│   ├── lib/
│   │   ├── models/                         # Data Models
│   │   ├── screens/                        # ExamListScreen, ExamScreen, ResultScreen
│   │   ├── services/                       # ApiService (kết nối Backend localhost:5000)
│   │   └── widgets/                        # QuestionCard, PassageCard, QuestionGridSheet
│   └── pubspec.yaml
├── videos/                                 # Video quay thực tế các chức năng hoàn chỉnh
│   ├── Giao diện Admin Import đề thi 1.mp4
│   ├── Giao diện Admin Import đề thi 2.mp4
│   ├── Giao diện Admin Import đề thi 3.mp4
│   ├── Giao diện Users.mp4
│   └── Sơ đồ database.mp4
├── Database_ER_Diagram.html                # Sơ đồ Cơ sở Dữ liệu Quan hệ tương tác trực quan
├── DeThi_TOEIC_Reading_Test05_Tagged.docx  # File Word đề thi Test 05 chuẩn 100 câu có gắn thẻ
├── DeThi_TOEIC_Loi_DinhDang.docx           # File Word mẫu test cơ chế All-or-Nothing
└── .gitignore                              # File cấu hình bỏ qua file build & cache
```

---

## 🚀 Các Tính Năng Nổi Bật

### 1. Backend & Admin Portal (ASP.NET Core MVC)
* **Cơ chế Import All-or-Nothing**: Kiểm duyệt toàn vẹn cấu trúc file Word (.docx) 100 câu bằng `DocumentFormat.OpenXml`. Nếu phát hiện bất kỳ lỗi định dạng nào, quy trình hủy bỏ hoàn toàn trong bộ nhớ (In-Memory), không lưu rác vào CSDL.
* **Hỗ trợ đầy đủ 3 phần TOEIC Reading**:
  * **Part 5**: 30 câu đơn (101 - 130).
  * **Part 6**: 4 nhóm câu gắn với đoạn văn hoàn chỉnh (131 - 146).
  * **Part 7**: 54 câu gắn với đoạn văn/email/thông báo/hình ảnh biểu đồ (147 - 200).
* **Giao diện Quản trị viên**:
  * Tích hợp AdminLTE 3, Bootstrap 4.6/5, FontAwesome 6, Google Fonts Inter.
  * Xem trước, lọc theo tab Part, chỉnh sửa câu hỏi & đoạn văn/hình ảnh trực tiếp qua AJAX modal.

### 2. Frontend Ứng dụng Luyện thi (Flutter)
* **Màn hình danh sách đề thi (Mục 5)**: Tìm kiếm, lọc đề thi, thời gian làm bài, số câu hỏi.
* **Màn hình làm bài thi (Mục 6)**:
  * Đồng hồ đếm ngược 75:00 tự động nộp bài khi hết giờ.
  * Hiển thị câu hỏi trực quan, phân chia tab Part 5, 6, 7.
  * Lưới câu hỏi (Question Grid) 100 câu giúp thí sinh theo dõi tiến độ và nhảy nhanh đến câu chưa làm.
* **Màn hình kết quả & chấm điểm (Mục 7)**:
  * Quy đổi điểm TOEIC Reading chuẩn (thang điểm tối đa 495).
  * Thống kê số câu Đúng / Sai / Bỏ qua và thời gian làm bài.
  * Bảng Answer Review chi tiết từng câu.

---

## 🛠 Hướng Dẫn Khởi Chạy

### 1. Khởi chạy Backend:
```bash
cd Backend/10.2.0/aspnet-core
dotnet run --project src/ToeicReadingApp.Web.Mvc/ToeicReadingApp.Web.Mvc.csproj
```
* **Địa chỉ Admin**: `http://localhost:5000/Exams`
* **Tài khoản**: `admin`
* **Mật khẩu**: `123qwe`

### 2. Khởi chạy Frontend Flutter:
```bash
cd Frontend
flutter run -d chrome --web-port 3000
```
* **Địa chỉ Client**: `http://localhost:3000`
