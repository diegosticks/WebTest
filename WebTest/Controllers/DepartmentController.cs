using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebTest.Data;
using WebTest.Models;

namespace WebTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public DepartmentController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Department>> GetDepartments()
        {
            return Ok(_db.Departments.ToList());
        }

        [HttpGet("{id:int}", Name ="GetDepartment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Department> GetDepartment(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var department = _db.Departments.FirstOrDefault(d => d.DepartmentId == id);
            if (department == null)
            {
                return NotFound();
            }

            return Ok(department);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Department> CreateDepartment([FromBody] Department dept)
        {
            if(_db.Departments.FirstOrDefault(d=>d.DepartmentName.ToLower()==dept.DepartmentName.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Department Already Exist!");
                return BadRequest(ModelState);
            }
            if(dept == null)
            {
                return BadRequest(dept);
            }
            if(dept.DepartmentId > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            Department department = new()
            {
                DepartmentId = dept.DepartmentId,
                DepartmentName = dept.DepartmentName
            };
            _db.Departments.Add(department);
            _db.SaveChanges();

            return CreatedAtRoute("GetDepartment", new { Id = dept.DepartmentId }, dept);
        }

        [HttpDelete("{id:int}", Name ="DeleteDepartment")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteDepartment(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var department = _db.Departments.FirstOrDefault(d => d.DepartmentId == id);
            if(department == null)
            {
                return NotFound();
            }
            _db.Departments.Remove(department);
            _db.SaveChanges();

            return NoContent();
        }

        [HttpPut("{id:int}", Name ="UpdateDepartment")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateDepartment(int id, [FromBody] Department dept)
        {
            if (dept == null || id != dept.DepartmentId)
            {
                return BadRequest();
            }

            Department department = new()
            {
                DepartmentId = dept.DepartmentId,
                DepartmentName = dept.DepartmentName,
            };
            _db.Departments.Update(department);
            _db.SaveChanges();

            return NoContent();
        }
    }
}
