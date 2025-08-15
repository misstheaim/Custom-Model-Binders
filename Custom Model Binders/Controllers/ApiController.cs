using Custom_Model_Binders.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;

namespace Custom_Model_Binders.Controllers;

public class ApiController : Controller
{
    [HttpGet]
    public IActionResult Location([FromQuery(Name = "coords")]Point point)
    {
        if (point == null)
        {
            if (!ModelState.IsValid)
            {
                if (ModelState.TryGetValue("coords", out var pointEntity))
                {
                    return BadRequest(pointEntity.Errors.First().ErrorMessage);
                }
            }
            return BadRequest("Incorrect coords input.");
        }
        return Json(point, new JsonSerializerOptions()
        {
            WriteIndented = true,
        });
    }

    [HttpGet]
    public IActionResult Person([FromRoute(Name = "id")]Person person)
    {
        if (person == null)
        {
            if (!ModelState.IsValid)
            {
                if (ModelState.TryGetValue("id", out var pointEntity))
                {
                    return BadRequest(pointEntity.Errors.First().ErrorMessage);
                }
            }
            return NotFound();
        }
        return Json(person, new JsonSerializerOptions()
        {
            WriteIndented = true,
        });
    }

    [HttpPost]
    public IActionResult Person(string name, int age)
    {
        if (string.IsNullOrEmpty(name) || age == 0)
        {
            return BadRequest("Properties should not be empty.");
        }
        Guid id = Guid.NewGuid();
        var person = new Person() { Id = id, Name = name, Age = age};
        DataBase.People.Add(person with { });

        string encodedId = Convert.ToBase64String(id.ToByteArray());
        person.Id = Guid.Empty;

        return Json(new { person, encodedId }, new JsonSerializerOptions()
        {
            WriteIndented = true,
        });
    }
}
