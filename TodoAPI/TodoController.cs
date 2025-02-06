using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoAPI.Database;
using TodoAPI.Dtos;
using TodoAPI.Entity;
using TodoAPI.Repository;

namespace TodoAPI.API
{

    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoRepository _repository;
        private readonly ILogger<TodoController> _logger;
        private readonly IMapper _mapper;
        public TodoController(ITodoRepository repository, ILogger<TodoController> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<TodoDto>>> GetTodos()
        {
            var todos = await _repository.GetAllAsync();
            return Ok(_mapper.Map<List<TodoDto>>(todos));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoDto>> GetTodo(int id)
        {
            var todo = await _repository.GetByIdAsync(id);

            if (todo is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<TodoDto>(todo));
        }

        [HttpPost]
        public async Task<ActionResult<TodoDto>> PostTodo(TodoDto todo)
        {
            // controller cant access directly to entity
            // have to seperate services
            await _repository.AddAsync(_mapper.Map<TodoItem>(todo));
            return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, todo);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutTodo(int id, TodoDto todo)
        {
            if (id != todo.Id)
            {
                return BadRequest();
            }

            try
            {
                await _repository.UpdateAsync(_mapper.Map<TodoItem>(todo));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _repository.ExistsAsync(todo.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }

}