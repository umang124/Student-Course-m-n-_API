using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer;
using ServiceLayer.DTOs;

namespace Student_Course.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCourses()
        {
            var courses = await _courseService.GetCourses();
            return Ok(courses);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCourse(int id)
        {
            var course = await _courseService.GetCourse(id);
            return Ok(course);
        }

        [HttpPost]
        public async Task<IActionResult> AddCourse(CourseDTO course)
        {
            await _courseService.AddCourse(course);
            return Ok("Added");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCourse(int id, CourseDTO course)
        {
            await _courseService.UpdateCourse(id, course);
            return Ok("Updated");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            await _courseService.DeleteCourse(id);
            return Ok("Deleted");
        }
    }
}
