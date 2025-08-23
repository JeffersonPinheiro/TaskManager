using GerenciamentoDeTarefas.src.Application.DTOs;
using GerenciamentoDeTarefas.src.Application.UseCases.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoDeTarefas.src.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly CreateTaskUseCase _createTask;
        private readonly EditTaskUseCase _editTask;
        private readonly DeleteTaskUseCase _deleteTask;
        private readonly ListTaskUseCase _listTasks;
        private readonly DashboardUseCase _dashboard;
        private readonly GetTaskUseCase _getTask;
        public TasksController(
            CreateTaskUseCase createTask,
            EditTaskUseCase editTask,
            DeleteTaskUseCase deleteTask,
            ListTaskUseCase listTasks,
            DashboardUseCase dashboard,
            GetTaskUseCase getTask)
        {
            _createTask = createTask;
            _editTask = editTask;
            _deleteTask = deleteTask;
            _listTasks = listTasks;
            _dashboard = dashboard;
            _getTask = getTask;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskRequest request)
        {
            var task = await _createTask.ExecuteAsync(request);
            return CreatedAtAction(nameof(Get), new { id = task.Id }, task);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] EditTaskRequest request)
        {
            var task = await _editTask.ExecuteAsync(id, request);
            return Ok(task);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _deleteTask.ExecuteAsync(id);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] TaskFilter filter)
        {
            var tasks = await _listTasks.ExecuteAsync(filter);
            return Ok(tasks);
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            var dashboard = await _dashboard.ExecuteAsync();
            return Ok(dashboard);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var task = await _getTask.ExecuteAsync(id);
            return Ok(task);
        }
    }
}
