using System.Collections.Generic;
using UniversityApp.Models;
using UniversityApp.DataAccess;

namespace UniversityApp.Services
{
    public class UniversityService
    {
        private readonly UniversityRepository _repository;
        public UniversityService(UniversityRepository repository) => _repository = repository;

        public void RegisterStudent(Student student, StudentProfile profile) => _repository.AddStudentWithProfile(student, profile);

        public void SetupDepartmentWithProfessor(Department dept, Professor prof)
        {
            _repository.AddDepartment(dept);
            prof.DepartmentId = dept.Id;
            _repository.AddProfessor(prof);
        }

        public void CreateCourse(Course course) => _repository.AddCourse(course);
        public List<Course> GetCatalog() => _repository.GetAllCourses();
        public Student? GetStudentInfo(int studentId) => _repository.GetStudentById(studentId);
    }

    public class EnrollmentService
    {
        private readonly UniversityRepository _repository;
        private const int MaxEctsCredits = 30;

        public EnrollmentService(UniversityRepository repository) => _repository = repository;

        public bool EnrollStudent(int studentId, int courseId, out string message)
        {
            var student = _repository.GetStudentById(studentId);
            var course = _repository.GetCourseById(courseId);

            if (student == null || course == null)
            {
                message = "Помилка: студента або навчальний курс не знайдено.";
                return false;
            }

            int currentCredits = GetStudentTotalCredits(studentId);
            if (currentCredits + course.Credits > MaxEctsCredits)
            {
                message = $"Відмовлено: перевищено ліміт навантаження ({currentCredits + course.Credits}/{MaxEctsCredits} ECTS).";
                return false;
            }

            _repository.SaveEnrollment(new Enrollment { StudentId = studentId, CourseId = courseId });
            message = $"Студента {student.FullName} успішно зараховано на курс \"{course.Title}\".";
            return true;
        }

        public void SetGrade(int studentId, int courseId, string grade) =>
            _repository.SaveEnrollment(new Enrollment { StudentId = studentId, CourseId = courseId, Grade = grade });

        public int GetStudentTotalCredits(int studentId)
        {
            int sum = 0;
            foreach (var item in _repository.GetEnrollmentsByStudent(studentId))
            {
                var course = _repository.GetCourseById(item.CourseId);
                if (course != null) sum += course.Credits;
            }
            return sum;
        }

        public List<(Course Course, string Grade)> GetAcademicCardData(int studentId)
        {
            var result = new List<(Course, string)>();
            foreach (var e in _repository.GetEnrollmentsByStudent(studentId))
            {
                var course = _repository.GetCourseById(e.CourseId);
                if (course != null) result.Add((course, e.Grade));
            }
            return result;
        }
    }
}