using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesWebApi.Models;
using SalesWebApi.Services;

namespace SalesWebApi.Controllers
{
    [ApiController]
    [Route("/departments")]
    public class DepartmentsController : ControllerBase
    {
        private readonly DepartmentService _departmentService;

        public DepartmentsController(DepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Index()
        {
            var list = await _departmentService.FindAllAsync();

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

            var department = await _departmentService.FindAsync(id.Value);

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

            await _departmentService.InsertAsync(department);

            // Um objeto Department é fornecido no corpo da resposta, juntamente com um cabeçalho de resposta Location contendo a URL do produto recém-criado.
            return CreatedAtAction(nameof(Details), new { id = department.Id }, department);
            // return Created(new Uri(Url.Link("Details", new { id = department.Id })), department);
            // return Created(department.Id.ToString(), department);
            // return Ok(department);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Edit(int id, [FromBody] Department department)
        {
            if (id != department.Id)
            {
                return NotFound(new { message = "Id mismatch" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                await _departmentService.UpdateAsync(department);

                return Ok(department);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_departmentService.DepartmentExists(department.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _departmentService.RemoveAsync(id);

                return Ok(new { message = "Successfully deleted" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

    }
}
