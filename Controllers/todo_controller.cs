using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TodoController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<TodoItem>> GetTodos()
    {
        return Ok(_context.TodoItems.ToList());
    }

    [HttpGet("{id}")]
    public ActionResult<TodoItem> GetTodo(int id)
    {
        var todo = _context.TodoItems.Find(id);
        if (todo == null)
            return NotFound();
        return todo;
    }

    [HttpPost]
    public ActionResult<TodoItem> CreateTodo(TodoItem todo)
    {
        todo.CreatedAt = DateTime.UtcNow;
        _context.TodoItems.Add(todo);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, todo);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateTodo(int id, TodoItem todo)
    {
        if (id != todo.Id)
            return BadRequest();

        _context.Entry(todo).State = EntityState.Modified;
        _context.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteTodo(int id)
    {
        var todo = _context.TodoItems.Find(id);
        if (todo == null)
            return NotFound();

        _context.TodoItems.Remove(todo);
        _context.SaveChanges();
        return NoContent();
    }
}