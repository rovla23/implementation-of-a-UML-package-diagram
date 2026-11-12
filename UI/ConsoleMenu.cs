using System;
using UniversityApp.Models;
using UniversityApp.Services;

namespace UniversityApp.UI
{
    public class ConsoleMenu
    {
        private readonly UniversityService _universityService;
        private readonly EnrollmentService _enrollmentService;

        public ConsoleMenu(UniversityService universityService, EnrollmentService enrollmentService)
        {
            _universityService = universityService;
            _enrollmentService = enrollmentService;
        }

        public void ShowHeader()
        {
            Console.WriteLine("==================================================");
            Console.WriteLine(" СИСТЕМА АВТОМАТИЗАЦІЇ РОБОТИ УНІВЕРСИТЕТУ ");
            Console.WriteLine("==================================================\n");
        }

        public void DisplayCourses()
        {
            Console.WriteLine("--- Каталог доступних курсів ---");
            foreach (var c in _universityService.GetCatalog())
            {
                Console.WriteLine($"  #{c.Id} {c.Title} ({c.Credits} ECTS)");
            }
            Console.WriteLine();
        }

        public void DisplayAcademicCard(int studentId)
        {
            var student = _universityService.GetStudentInfo(studentId);
            if (student == null) return;

            var cardItems = _enrollmentService.GetAcademicCardData(studentId);
            int totalCredits = _enrollmentService.GetStudentTotalCredits(studentId);

            Console.WriteLine("\n--------------------------------------------------");
            Console.WriteLine($" АКАДЕМІЧНА КАРТКА СТУДЕНТА #{student.Id}");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"ПІБ:            {student.FullName}");
            Console.WriteLine($"Рік вступу:     {student.EnrollmentYear}");
            Console.WriteLine($"Телефон:        {student.Profile?.PhoneNumber ?? "Не вказано"}");
            Console.WriteLine("Вивчені дисципліни:");

            foreach (var item in cardItems)
            {
                Console.WriteLine($"  • {item.Course.Title} ({item.Course.Credits} ECTS) — Оцінка: {item.Grade}");
            }

            Console.WriteLine($"Загальна кількість кредитів: {totalCredits} ECTS");
            Console.WriteLine("--------------------------------------------------\n");
        }
    }
}