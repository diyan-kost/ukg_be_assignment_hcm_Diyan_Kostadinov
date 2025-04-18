﻿using HCM.Core.Models.Salary;

namespace HCM.Core.Models.Employee
{
    public class EmployeeDetailsDto
    {
        public string FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string? LastName { get; set; }

        public string NationalIdNumber { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string CurrentAddress { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Gender { get; set; }

        public DateTime HiredAt { get; set; }

        public string? ManagerName { get; set; }

        public int? ManagerId { get; set; }

        public string? Username { get; set; }

        public string? Role { get; set; }

        public List<SalaryDto> Salaries { get; set; }
    }
}
