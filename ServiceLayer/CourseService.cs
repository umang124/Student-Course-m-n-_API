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
    public interface ICourseService
    {
        Task AddCourse(CourseDTO course);
        Task<ICollection<ViewCourse>> GetCourses();
        Task<ViewCourse> GetCourse(int id);
        Task UpdateCourse(int id, CourseDTO course);
        Task DeleteCourse(int id);

    }
    public class CourseService : ICourseService
    {
        private readonly StudentCourseDbContext _dbContext;

        public CourseService(StudentCourseDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddCourse(CourseDTO course)
        {
            Course courseMap = new();
            courseMap.Name = course.Name;

            await _dbContext.Course.AddAsync(courseMap);
            await _dbContext.SaveChangesAsync();

        }

        public async Task DeleteCourse(int id)
        {
            var course = await _dbContext.Course.FindAsync(id);
            if (course != null)
            {
                _dbContext.Remove(course);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<ViewCourse> GetCourse(int id)
        {
            var course = await _dbContext.Course.Select(x => new ViewCourse
            {
                Id = x.Id,
                Name = x.Name
            }).FirstOrDefaultAsync(x => x.Id == id);
            
            if (course != null)
            {
                return course;
            }
            return new ViewCourse();
        }

        public async Task<ICollection<ViewCourse>> GetCourses()
        {
            var courses = await _dbContext.Course.Select(x => new ViewCourse
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();

            return courses;
        }

        public async Task UpdateCourse(int id, CourseDTO course)
        {
            var getCourse = await _dbContext.Course.FindAsync(id);
            if (getCourse != null)
            {
                getCourse.Name = course.Name;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
