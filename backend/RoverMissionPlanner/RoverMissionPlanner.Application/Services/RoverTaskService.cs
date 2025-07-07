using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoverMissionPlanner.Domain.Entities;

namespace RoverMissionPlanner.Application.Services
{
    public class RoverTaskService : IRoverTaskService
    {
        // Diccionario en memoria para guardar tareas por rover
        private readonly ConcurrentDictionary<Guid, List<RoverTask>> _tasks = new();

        // Crea tarea validando solapamientos
        public async Task<bool> CreateTaskAsync(Guid roverId, RoverTask task)
        {
            // Si ya hay tareas registradas
            if (_tasks.TryGetValue(roverId, out var existingTasks))
            {
                // Verifica si la nueva tarea se solapa con alguna existente
                bool overlaps = existingTasks.Any(t => (task.StartsAt < t.EndsAt) && (t.StartsAt < task.EndsAt));

                if (overlaps)
                {
                    return false; // Conflicto por solapamiento
                }
            } else
            {
                // Si es la primera vez que se asigna tarea al rover
                existingTasks = new List<RoverTask>();
                _tasks[roverId] = existingTasks;
            }

            existingTasks.Add(task); // Agrega tarea nueva
            return await Task.FromResult(true);
        }

        // Obtiene tareas ordenadas por hora de inicio para un día
        public async Task<IList<RoverTask>> GetTasksByDateAsync(Guid roverId, DateTime date)
        {
            if (_tasks.TryGetValue(roverId, out var existingTasks))
            {
                var tasksForDay = existingTasks
                    .Where(t => t.StartsAt.Date == date.Date)
                    .OrderBy(t => t.StartsAt)
                    .ToList();

                return await Task.FromResult(tasksForDay);
            }

            // Retorna una lista vacía si no hay tareas
            return await Task.FromResult(new List<RoverTask>());
        }

        // Calcula el porcentaje de minutos ocupados en el día (1440 minutos)
        public async Task<double> GetUtilizationByDateAsync(Guid roverId, DateTime date)
        {
            if (_tasks.TryGetValue(roverId, out var existingTasks))
            {
                var tasksForDay = existingTasks.Where(t => t.StartsAt.Date == date.Date);
                int totalMinutes = tasksForDay.Sum(t => t.DurationMinutes);

                // Retorna el porcentaje del dia ocupado
                return await Task.FromResult((double)totalMinutes / 1440 * 100);
            }

            // Si no hay tareas retorna 0% de uso
            return await Task.FromResult(0);
        }
    }
}
