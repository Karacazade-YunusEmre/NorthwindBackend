using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize()]
public class ProductsController : ControllerBase
{
    private readonly IProductService _product;

    public ProductsController(IProductService product)
    {
        _product = product;
    }

    [HttpGet]
    [Route("getallproduct")]
    public IActionResult GetAllProduct()
    {
        try
        {
            var result = _product.GetAll();
            if (!result.Success)
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
    [Route("getproductbyname")]
    public IActionResult GetProductsByName(string name)
    {
        try
        {
            var result = _product.GetByName(name);
            if (!result.Success)
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
    [Route("getproductbyid")]
    public IActionResult GetProductById(int id)
    {
        try
        {
            var result = _product.GetById(id);
            if (!result.Success)
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
    [Route("add")]
    [Authorize(Roles = "User, Manager, Admin")]
    public IActionResult AddProduct(Product product)
    {
        try
        {
            var result = _product.Add(product);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return new StatusCodeResult(201);
        }
        catch
        {
            // ignored
        }

        return BadRequest();
    }

    [Authorize(Roles = "Manager, Admin")]
    [HttpPut]
    [Route("update")]
    public IActionResult UpdateProduct(Product product)
    {
        try
        {
            var result = _product.Update(product);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return new StatusCodeResult(201);
        }
        catch
        {
            // ignored
        }

        return BadRequest();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete]
    [Route("delete")]
    public IActionResult DeleteProduct(int id)
    {
        try
        {
            var result = _product.Delete(id);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return new StatusCodeResult(201);
        }
        catch
        {
            // ignored
        }

        return BadRequest();
    }
}