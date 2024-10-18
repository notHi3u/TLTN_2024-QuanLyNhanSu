
using EMS.Domain.Models.Account;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

    namespace EMS.Domain.Models
    {
        public class Employee
        {
            public string Id { get; set; } = new Guid().ToString(); // Mã nhân viên (Primary Key)
            public string? LastName { get; set; }// Họ
            public string? FirstName { get; set; }// Tên
            public DateOnly DateOfBirth { get; set; }// Ngày sinh
            public required string Gender { get; set; }// Giới tính
            public required string Nationality { get; set; }// Quốc tịch
            public string? Address { get; set; }// Địa chỉ
            public string PhoneNumber { get; set; }// Số điện thoại 
            public DateOnly HireDate { get; set; }// Ngày vào làm
            public DateOnly? FiredDate { get; set; }// Ngày rời cty (nullable)
            public required string Position { get; set; }// Vị trí công việc của nhân viên
            public required string Status { get; set; }// Tình trạng hoạt động
            public string? MaritalStatus { get; set; }// Tình trạng hôn nhân
            public required string EducationLevel { get; set; }// Trình độ học vấn cao nhất
            public string? IdNumber { get; set; }// Mã số CCCD hoặc Passport
            public string? DepartmentId { get; set; }// Mã phòng ban (Foreign Key)
            public string? TaxId { get; set; }// Mã số thuế thu nhập cá nhân
            public string Email { get; set; }// Email

            public string? UserId { get; set; }
            public virtual User User { get; set; }
        }
    }
