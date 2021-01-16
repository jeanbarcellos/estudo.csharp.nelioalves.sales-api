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

            return Ok(obj);
        }

        [HttpGet]
        [Route("create")]
        public async Task<IActionResult> Create()
        {
            var departments = await _departmentService.FindAllAsync();

            var viewModel = new SellerFormViewModel { Departments = departments };

            return Ok(viewModel);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create([FromBody] Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.FindAllAsync();

                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };

                return BadRequest(viewModel);
            }

            await _sellerService.InsertAsync(seller);

            // Um objeto Seller é fornecido no corpo da resposta, juntamente com um cabeçalho de resposta Location contendo a URL do produto recém-criado.
            return CreatedAtAction(nameof(Details), new { id = seller.Id }, seller);
            // return Created(new Uri(Url.Link("Details", new { id = department.Id })), department);
            // return Created(seller.Id.ToString(), seller);
            // return Ok(seller);
        }


        [HttpGet]
        [Route("{id:int}/edit")]
        public async Task<IActionResult> Edit(int? id)
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

            List<Department> departments = await _departmentService.FindAllAsync();

            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = departments };

            return Ok(viewModel);
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
                var departments = await _departmentService.FindAllAsync();

                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };

                return BadRequest(viewModel);
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
