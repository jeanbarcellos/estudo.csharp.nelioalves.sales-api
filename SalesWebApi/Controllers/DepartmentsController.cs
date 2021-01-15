using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesWebApi.Data;
using SalesWebApi.Models;

namespace SalesWebApi.Controllers
{
    [ApiController]
    [Route("/departments")]
    public class DepartmentsController : ControllerBase
    {
        private readonly SalesWebApiContext _context;

        public DepartmentsController(SalesWebApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Index()
        {
            var list = await _context.Department.ToListAsync();

            return Ok(list);
        }

        [HttpGet]
        [Route("{id:int}", Name = "Details")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest(new { message = "Id not provided" });
            }

            var department = await _context.Department
                .FirstOrDefaultAsync(m => m.Id == id);

            if (department == null)
            {
                return NotFound(new { message = "Id not found" });
            }

            return Ok(department);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create([FromBody] Department department)
        {
            // Obtém o ModelStateDictionary que contém o estado do modelo e da validação de vinculação do modelo.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Add(department);
            await _context.SaveChangesAsync();

            // Um objeto Department é fornecido no corpo da resposta, juntamente com um cabeçalho de resposta Location contendo a URL do produto recém-criado.
            return CreatedAtAction(nameof(Details), new { id = department.Id }, department);
            // return Created(new Uri(Url.Link("Details", new { id = department.Id })), department);
            // return Created(department.Id.ToString(), department);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Edit(int id, [FromBody] Department department)
        {
            if (id != department.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                _context.Update(department);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(department.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(department);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var department = await _context.Department.FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            _context.Department.Remove(department);
            await _context.SaveChangesAsync();

            return Ok(department);
        }

        private bool DepartmentExists(int id)
        {
            return _context.Department.Any(e => e.Id == id);
        }
    }
}
