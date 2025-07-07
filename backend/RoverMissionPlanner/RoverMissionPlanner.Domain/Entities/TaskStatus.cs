using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoverMissionPlanner.Domain.Entities
{
    public enum TaskStatus
    {
        Planned,
        InProgress,
        Completed,
        Aborted
    }
}
