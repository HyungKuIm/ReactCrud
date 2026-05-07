using Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PeopleController : ControllerBase
{
    private readonly AppDbContext _context;

    public PeopleController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> AddPerson(Person person)
    {
        try
        {
            _context.People.Add(person);
            await _context.SaveChangesAsync();
            return CreatedAtRoute("GetPerson", new { id = person.Id }, person); // 201 Created status code + person object in response body
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            // 500 Internal Server Error status code + exception message in response body
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetPeople()
    {
        try
        {
            var people = await _context.People.ToListAsync();
            return Ok(people); // 200 Ok status code + list of people in response body
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            // 500 Internal Server Error status code + exception message in response body
        }
    }

    [HttpGet("{id:int}", Name = "GetPerson")]
    public async Task<IActionResult> GetPerson(int id)
    {
        try
        {
            var person = await _context.People.FindAsync(id);
            if (person is null)
            {
                return NotFound(); // 404 Not Found status code
            }
            return Ok(person); // 200 Ok status code + person object in response body
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            // 500 Internal Server Error status code + exception message in response body
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdatePerson(int id, [FromBody] Person person)
    {
        try
        {
            if (id != person.Id)
            {
                return BadRequest("ID in URL does not match ID in request body."); // 400 Bad Request status code + error message in response body
            }

            if (!await _context.People.AnyAsync(p => p.Id == id))
            {
                return NotFound(); // 404 Not Found status code
            }


            _context.People.Update(person);
            await _context.SaveChangesAsync();
            return NoContent(); // 204 No Content status code
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            // 500 Internal Server Error status code + exception message in response body
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePerson(int id)
    {
        try
        {
            
            var person = await _context.People.FindAsync(id);
            if (person is null)
            {
                return NotFound(); // 404 Not Found status code
            }

            _context.People.Remove(person);
            await _context.SaveChangesAsync();
            return NoContent(); // 204 No Content status code
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            // 500 Internal Server Error status code + exception message in response body
        }
    }
    
}

