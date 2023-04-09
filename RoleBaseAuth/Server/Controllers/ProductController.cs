using Microsoft.AspNetCore.Mvc;
using RoleBaseAuth.Server.Interfaces;
using RoleBaseAuth.Shared.Model;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RoleBaseAuth.Server
{
    [Authorize(Roles = "Administrator")]
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Route("CreateProduct")]
        public IActionResult CreateProduct(Product product)
        {
            var result = _unitOfWork.Products.Add(product);
            _unitOfWork.Complete();
            if (result is not null) return Ok("Product Created");
            else return BadRequest("Error in Creating the Product");
        }

        [HttpPut]
        [Route("UpdateProduct")]
        public IActionResult UpdateProduct(Product product)
        {
            _unitOfWork.Products.Update(product);
            _unitOfWork.Complete();
            return Ok("Product Updated");
        }

      

        [HttpGet]
        public async Task<List<Product>> Get()
        {
            return await _unitOfWork.Products.GetAll();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Product product = await _unitOfWork.Products.Get(id);
            if (product != null)
            {
                return Ok(product);
            }
            return NotFound();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Product product = await _unitOfWork.Products.Get(id);

            _unitOfWork.Products.Delete(product);
            _unitOfWork.Complete();
            return Ok();
        }
    }
}
