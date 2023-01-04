using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductManager _product;

    public ProductsController(IProductManager product)
    {
        _product = product;
    }

    [HttpGet]
    [Route("GetAllProduct")]
    public IActionResult GetAllProduct()
    {
        try
        {
            var result = _product.GetAll();
            if (result.Count == 0)
            {
                return NotFound("There is no any Product item");
            }

            return Ok(result);
        }
        catch
        {
            // ignored
        }

        return BadRequest();
    }

    [HttpGet]
    [Route("GetProductsByName")]
    public IActionResult GetProductsByName(string name)
    {
        try
        {
            var result = _product.GetByName(name);
            if (result.Count == 0)
            {
                return NotFound($"There is no any Product item with name contain: {name}");
            }

            return Ok(result);
        }
        catch
        {
            // ignored
        }

        return BadRequest();
    }

    [HttpGet]
    [Route("GetProductById")]
    public IActionResult GetProductById(int id)
    {
        try
        {
            var result = _product.GetById(id);
            if (result == null)
            {
                return NotFound($"There is no any Product item with id: {id}");
            }

            return Ok(result);
        }
        catch
        {
            // ignored
        }

        return BadRequest();
    }

    [HttpPost]
    [Route("AddProduct")]
    public IActionResult AddProduct(Product product)
    {
        try
        {
            _product.Add(product);

            return new StatusCodeResult(201);
        }
        catch
        {
            // ignored
        }

        return BadRequest();
    }

    [HttpPut]
    [Route("UpdateProduct")]
    public IActionResult UpdateProduct(Product product)
    {
        try
        {
            _product.Update(product);

            return new StatusCodeResult(201);
        }
        catch
        {
            // ignored
        }

        return BadRequest();
    }

    [HttpDelete]
    [Route("DeleteProduct")]
    public IActionResult DeleteProduct(int id)
    {
        try
        {
            _product.Delete(id);

            return new StatusCodeResult(201);
        }
        catch
        {
            // ignored
        }

        return BadRequest();
    }
}