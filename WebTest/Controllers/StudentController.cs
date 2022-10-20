using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebTest.Data;
using WebTest.Models;
using WebTest.Models.Dto;

namespace WebTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public StudentController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<StudentCreate>> GetStudents()
        {
            return Ok(_db.Students.ToList());
        }

        [HttpGet("{id:int}", Name ="GetStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<StudentCreate> GetStudent(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var student = _db.Students.FirstOrDefault(x => x.Id == id);    
            if(student == null)
            {
                return NotFound();
            } 
            
            return Ok(student);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentCreate> CreateStudent([FromBody] StudentCreate screate)
        {
            if (_db.Students.FirstOrDefault
                (s => s.FirstName.ToLower() == screate.FirstName.ToLower()
                && s.LastName.ToLower() == screate.LastName.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Student Already Exist");
                return BadRequest(ModelState);
            }
            if (screate == null)
            {
                return BadRequest(screate);
            }
            if (screate.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            Student student = new()
            {
                Id = screate.Id,
                FirstName = screate.FirstName,
                LastName = screate.LastName,
                Enrolled = screate.Enrolled
            };
            _db.Students.Add(student);
            _db.SaveChanges();

            return CreatedAtRoute("GetStudent", new { Id = screate.Id }, screate);
        }

        [HttpDelete("{id:int}", Name = "DeleteStudent")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteStudent(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var student = _db.Students.FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            _db.Students.Remove(student);
            _db.SaveChanges();

            return NoContent();
        }

        [HttpPut("{id:int}", Name ="UpdateStudent")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateStudent(int id, [FromBody] StudentUpdate supdate)
        {
            if (supdate == null || id != supdate.Id)
            {
                return BadRequest();
            }

            Student student = new()
            {
                Id = id,
                FirstName = supdate.FirstName,
                LastName = supdate.LastName,
                Enrolled = supdate.Enrolled,
                Updated = supdate.Updated = DateTime.Now
            };
            _db.Students.Update(student);
            _db.SaveChanges();

            return NoContent();
        }
    }
}
