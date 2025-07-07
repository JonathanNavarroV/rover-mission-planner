using RoverMissionPlanner.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoverMissionPlanner.Domain.Entities
{
    public class RoverTask
    {
        public Guid Id { get; set; }
        public string RoverName { get; set; } = string.Empty;
        public TaskType TaskType { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime StartsAt { get; set; } // UTC
        public int DurationMinutes { get; set; }
        public Enums.TaskStatus Status { get; set; }

        public DateTime EndsAt => StartsAt.AddMinutes(DurationMinutes);
    }
}
