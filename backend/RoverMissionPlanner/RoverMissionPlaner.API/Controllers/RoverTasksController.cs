using Microsoft.AspNetCore.Mvc;
using RoverMissionPlanner.Application.Services;
using RoverMissionPlanner.Domain.Entities;

namespace RoverMissionPlanner.API.Controllers
{
    [ApiController]
    [Route("rovers/{roverId:guid}/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly IRoverTaskService _roverTaskService;

        // Se inyecta el servicio que maneja la lógica de tareas
        public TasksController(IRoverTaskService roverTaskService)
        {
            _roverTaskService = roverTaskService;
        }

        // POST /rovers/{roverId}/tasks -> Crear tarea
        [HttpPost]
        public async Task<IActionResult> CreateTask(Guid roverId, [FromBody] RoverTask task)
        {
            throw new Exception("Error forzado para probar middleware");
            task.Id = Guid.NewGuid();

            // Se intenta crear usando el servicio
            var created = await _roverTaskService.CreateTaskAsync(roverId, task);

            if (!created)
            {
                return Conflict("La tarea se solapa con otra existente");
            }

            // Devuelve un 201 con la tarea creada
            return CreatedAtAction(nameof(GetTasks), new { roverId, date = task.StartsAt.Date.ToString("yyyy-MM-dd") }, task);
        }

        // GET /rovers/{roverId}/tasks?date=YYYY-MM-DD -> Lista todas las tareas del rover para el día indicado
        [HttpGet]
        public async Task<IActionResult> GetTasks(Guid roverId, [FromQuery] DateTime date)
        {
            var tasks = await _roverTaskService.GetTasksByDateAsync(roverId, date);

            // Devuelva la lista ordenada de tareas
            return Ok(tasks);
        }

        // GET /rovers/{roverId}/utilization?date=YYYY-MM-DD -> Devuelve el porcentaje de utilización diaria del rover
        [HttpGet("/rovers/{roverId:guid}/utilization")]
        public async Task<IActionResult> GetUtilization(Guid roverId, [FromQuery] DateTime date)
        {
            var usage = await _roverTaskService.GetUtilizationByDateAsync(roverId, date);

            // Devuelve un objeto con el porcentaje de uso
            return Ok(new { utilizationPercent = usage });
        }
    }
}
