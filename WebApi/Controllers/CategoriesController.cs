using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryManager _category;

    public CategoriesController(ICategoryManager category)
    {
        _category = category;
    }

    [HttpGet]
    [Route("GetAllCategories")]
    public IActionResult GetAllCategories()
    {
        try
        {
            var result = _category.GetAll();
            if (result.Count == 0)
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
    [Route("GetCategoriesByName")]
    public IActionResult GetCategoriesByName(string name)
    {
        try
        {
            var result = _category.GetByName(name);
            if (result.Count == 0)
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
    [Route("GetCategoryById")]
    public IActionResult GetCategoryById(int id)
    {
        try
        {
            var result = _category.GetById(id);
            if (result == null)
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
    [Route("AddCategory")]
    public IActionResult AddCategory(Category category)
    {
        try
        {
            _category.Add(category);

            return new StatusCodeResult(201);
        }
        catch
        {
            // ignored
        }

        return BadRequest();
    }

    [HttpPut]
    [Route("UpdateCategory")]
    public IActionResult UpdateCategory(Category category)
    {
        try
        {
            _category.Update(category);

            return new StatusCodeResult(201);
        }
        catch
        {
            // ignored
        }

        return BadRequest();
    }

    [HttpDelete]
    [Route("DeleteCategory")]
    public IActionResult DeleteCategory(int id)
    {
        try
        {
            _category.Delete(id);

            return new StatusCodeResult(201);
        }
        catch
        {
            // ignored
        }

        return BadRequest();
    }
}