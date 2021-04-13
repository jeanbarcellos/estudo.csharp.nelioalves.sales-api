using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWebApi.Models;
using SalesWebApi.Models.ViewModels;
using SalesWebApi.Services;
using SalesWebApi.Services.Exceptions;

namespace SalesWebApi.Controllers
{
    [ApiController]
    [Route("/sellers")]
    public class SellersController : ControllerBase
    {
        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;

        public SellersController(SellerService sellerService, DepartmentService departmentService)
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Index()
        {
            var list = await _sellerService.FindAllAsync();

            return Ok(list);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest(new { message = "Id not provided" });
            }

            var obj = await _sellerService.FindByIdAsync(id.Value);

            if (obj == null)
            {
                return NotFound(new { message = "Id not found" });
            }

            var viewModel = new SellerViewModel
            {
                Id = obj.Id,
                Name = obj.Name,
                Email = obj.Email,
                BirthDate = obj.BirthDate,
                BaseSalary = obj.BaseSalary,
                DepartmentId = obj.Department.Id
            };

            return Ok(viewModel);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create([FromBody] Seller seller)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Seller is invalid" });
            }

            await _sellerService.InsertAsync(seller);

            // Um objeto Seller é fornecido no corpo da resposta, juntamente com um cabeçalho de resposta Location contendo a URL do produto recém-criado.
            return CreatedAtAction(nameof(Details), new { id = seller.Id }, seller);
            // return Created(new Uri(Url.Link("Details", new { id = department.Id })), department);
            // return Created(seller.Id.ToString(), seller);
            // return Ok(seller);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Edit(int id, [FromBody] Seller seller)
        {

            if (id != seller.Id)
            {
                return BadRequest(new { message = "Id mismatch" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Seller is invalid" });
            }

            try
            {
                await _sellerService.UpdateAsync(seller);

                return Ok(seller);
            }
            catch (ApplicationException e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _sellerService.RemoveAsync(id);

                return Ok(new { message = "Successfully deleted" });
            }
            catch (IntegrityException e)
            {
                return BadRequest(new { message = e.Message });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

    }
}
