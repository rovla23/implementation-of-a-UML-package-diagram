using System.Collections.Generic;
using System.Linq;
using UniversityApp.Models;

namespace UniversityApp.DataAccess
{
    public class DatabaseContext
    {
        public List<Student> Students { get; } = new();
        public List<StudentProfile> StudentProfiles { get; } = new();
        public List<Course> Courses { get; } = new();
        public List<Enrollment> Enrollments { get; } = new();
        public List<Department> Departments { get; } = new();
        public List<Professor> Professors { get; } = new();
    }

    public class UniversityRepository
    {
        private readonly DatabaseContext _context;

        public UniversityRepository(DatabaseContext context) => _context = context;

        public void AddStudentWithProfile(Student student, StudentProfile profile)
        {
            profile.StudentId = student.Id;
            student.Profile = profile;
            _context.Students.Add(student);
            _context.StudentProfiles.Add(profile);
        }

        public Student? GetStudentById(int id) => _context.Students.FirstOrDefault(s => s.Id == id);
        public void AddDepartment(Department department) => _context.Departments.Add(department);
        public void AddProfessor(Professor professor) => _context.Professors.Add(professor);
        public List<Professor> GetProfessorsByDepartment(int deptId) => _context.Professors.Where(p => p.DepartmentId == deptId).ToList();
        public void AddCourse(Course course) => _context.Courses.Add(course);
        public List<Course> GetAllCourses() => _context.Courses;
        public Course? GetCourseById(int id) => _context.Courses.FirstOrDefault(c => c.Id == id);

        public void SaveEnrollment(Enrollment enrollment)
        {
            var existing = _context.Enrollments.FirstOrDefault(e => e.StudentId == enrollment.StudentId && e.CourseId == enrollment.CourseId);
            if (existing != null) existing.Grade = enrollment.Grade;
            else _context.Enrollments.Add(enrollment);
        }

        public List<Enrollment> GetEnrollmentsByStudent(int studentId) => _context.Enrollments.Where(e => e.StudentId == studentId).ToList();
    }
}