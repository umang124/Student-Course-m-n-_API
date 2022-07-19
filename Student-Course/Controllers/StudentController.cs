using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer;
using ServiceLayer.DTOs;

namespace Student_Course.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            var students = await _studentService.GetStudents();
            return Ok(students);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetStudent(int id)
        {
            var student = await _studentService.GetStudent(id);
            return Ok(student);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            await _studentService.DeleteStudent(id);
            return Ok("Deleted");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStudent(int id, StudentDTO student)
        {
            await _studentService.UpdateStudent(id, student);
            return Ok("Updated");
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent(StudentDTO student)
        {
            await _studentService.AddStudent(student);
            return Ok("Added");
        }
    }
}
