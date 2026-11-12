using System;
using UniversityApp.Models;
using UniversityApp.Services;

namespace UniversityApp.UI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var dbContext = new UniversityApp.DataAccess.DatabaseContext();
            var repository = new UniversityApp.DataAccess.UniversityRepository(dbContext);

            var universityService = new UniversityService(repository);
            var enrollmentService = new EnrollmentService(repository);
            var menu = new ConsoleMenu(universityService, enrollmentService);

            menu.ShowHeader();

            universityService.SetupDepartmentWithProfessor(
                new Department { Id = 1, Name = "Кафедра програмної інженерії", Building = "Корпус 1" },
                new Professor { Id = 1, FirstName = "Олександр", LastName = "Іваненко", Email = "ivanenko@univ.edu" }
            );

            universityService.CreateCourse(new Course { Id = 101, Title = "Проектування інформаційних систем", Credits = 5 });
            universityService.CreateCourse(new Course { Id = 102, Title = "Бази даних та знань", Credits = 4 });

            var student = new Student { Id = 1, FirstName = "Владислава", LastName = "Романчук", EnrollmentYear = 2023 };
            var profile = new StudentProfile { PassportNumber = "КВ123456", PhoneNumber = "+380991234567", Address = "м. Київ" };
            universityService.RegisterStudent(student, profile);

            menu.DisplayCourses();

            if (enrollmentService.EnrollStudent(1, 101, out string msg1))
                Console.WriteLine($"[Інфо] {msg1}");

            if (enrollmentService.EnrollStudent(1, 102, out string msg2))
                Console.WriteLine($"[Інфо] {msg2}");

            enrollmentService.SetGrade(1, 101, "A");
            enrollmentService.SetGrade(1, 102, "B");

            menu.DisplayAcademicCard(1);
        }
    }
}