using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _category;

    public CategoriesController(ICategoryService category)
    {
        _category = category;
    }

    [HttpGet]
    [Route("getallcategory")]
    public IActionResult GetAllCategory()
    {
        try
        {
            var result = _category.GetAll();
            if (!result.Success)
            {
                return NotFound("There is no any Category item");
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
    [Route("getcategorybyname")]
    public IActionResult GetCategoryByName(string name)
    {
        try
        {
            var result = _category.GetByName(name);
            if (!result.Success)
            {
                return NotFound($"There is no any Category item with name contain: {name}");
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
    [Route("getcategorybyid")]
    public IActionResult GetCategoryById(int id)
    {
        try
        {
            var result = _category.GetById(id);
            if (!result.Success)
            {
                return NotFound($"There is no any Category item with id: {id}");
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
    public IActionResult AddCategory(Category category)
    {
        try
        {
            var result = _category.Add(category);
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

    [HttpPut]
    [Route("update")]
    public IActionResult UpdateCategory(Category category)
    {
        try
        {
            var result = _category.Update(category);
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

    [HttpDelete]
    [Route("delete")]
    public IActionResult DeleteCategory(int id)
    {
        try
        {
            var result = _category.Delete(id);
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