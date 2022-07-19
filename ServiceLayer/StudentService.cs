using DataLayer.Data;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.DTOs;
using ServiceLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public interface IStudentService
    {
        Task AddStudent(StudentDTO student);
        Task<ICollection<ViewStudentWithCourse>> GetStudents();
        Task<ViewStudentWithCourse> GetStudent(int id);
        Task UpdateStudent(int id, StudentDTO student);
        Task DeleteStudent(int id);
    }
    public class StudentService : IStudentService
    {
        private readonly StudentCourseDbContext _dbContext;
        public StudentService(StudentCourseDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddStudent(StudentDTO student)

        {
            Student studentMap = new();
            studentMap.Name = student.Name;
            studentMap.Address = student.Address;

            await _dbContext.Student.AddAsync(studentMap);
            await _dbContext.SaveChangesAsync();

            int studentId = studentMap.Id;

            if (student.CourseIds.Any())
            {
                foreach (var courseId in student.CourseIds)
                {
                    StudentCourse studentCourse = new();
                    studentCourse.StudentId = studentId;
                    studentCourse.CourseId = courseId;

                    await _dbContext.StudentCourse.AddAsync(studentCourse);
                    await _dbContext.SaveChangesAsync();
                }
                
            }
        }

        public async Task DeleteStudent(int id)
        {
            var student = await _dbContext.Student.FindAsync(id);
            if (student != null)
            {
                _dbContext.Remove(student);
                await _dbContext.SaveChangesAsync();
            }
            var studentCourse = await _dbContext.StudentCourse.Where(x => x.StudentId == id).ToListAsync();
            if (studentCourse.Any())
            {
                _dbContext.RemoveRange(studentCourse);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<ViewStudentWithCourse> GetStudent(int id)
        {
            var student = await _dbContext.Student
                            .Include(s => s.StudentCourses)
                            .ThenInclude(c => c.Course)
                            .Select(d => new ViewStudentWithCourse
                            {
                                Name = d.Name,
                                Id = d.Id,
                                Address = d.Address,
                                CourseIds = d.StudentCourses.Select(s => s.CourseId).ToList(),
                                CourseNames = d.StudentCourses.Select(s => s.Course.Name).ToList()
                            }).FirstOrDefaultAsync(x => x.Id == id);
            if (student != null)
            {
                return student;
            }
            return new ViewStudentWithCourse();
        }

        public async Task<ICollection<ViewStudentWithCourse>> GetStudents()
        {
            var students = await _dbContext.Student
                                .Include(s => s.StudentCourses)
                                .ThenInclude(c => c.Course)
                                .Select(d => new ViewStudentWithCourse
                                {
                                    Id = d.Id,
                                    Name = d.Name,
                                    Address = d.Address,
                                    CourseIds = d.StudentCourses.Select(c => c.CourseId).ToList(),
                                    CourseNames = d.StudentCourses.Select(c => c.Course.Name).ToList()
                                }).ToListAsync();

            return students;
        }

        public async Task UpdateStudent(int id, StudentDTO student)
        {
            var getStudent = await _dbContext.Student.FindAsync(id);
            if (getStudent != null)
            {
                getStudent.Name = student.Name;
                getStudent.Address = student.Address;

                await _dbContext.SaveChangesAsync();
            }

            var getStudentCourse = await _dbContext.StudentCourse.Where(x => x.StudentId == id).ToListAsync();
            if (getStudentCourse.Any())
            {
                _dbContext.RemoveRange(getStudentCourse);
                await _dbContext.SaveChangesAsync();
            }

            if (student.CourseIds.Any())
            {
                foreach(var courseId in student.CourseIds)
                {
                    StudentCourse studentCourse = new();
                    studentCourse.StudentId = id;
                    studentCourse.CourseId = courseId;
                    await _dbContext.StudentCourse.AddAsync(studentCourse);
                    await _dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
