using RoverMissionPlanner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RoverMissionPlanner.Application.Services
{
    public interface IRoverTaskService
    {
        // Crea una tarea para un rover, devuelve false si hay solapamiento
        Task<bool> CreateTaskAsync(Guid roverId, RoverTask task);

        // Obtiene lista de tareas de un rover para un día específico
        Task<IList<RoverTask>> GetTasksByDateAsync(Guid roverId, DateTime date);

        // Calcula porcentaje de uso diario
        Task<double> GetUtilizationByDateAsync(Guid roverId, DateTime date);
    }
}
