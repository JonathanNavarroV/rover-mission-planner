using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentValidation;
using RoverMissionPlanner.Domain.Entities;

namespace RoverMissionPlanner.API.Validators
{
    public class RoverTaskValidator : AbstractValidator<RoverTask>
    {
        public RoverTaskValidator() {
            RuleFor(x => x.RoverName).NotEmpty();
            RuleFor(x => x.TaskType).IsInEnum();
            RuleFor(x => x.Latitude).InclusiveBetween(-90, 90);
            RuleFor(x => x.Longitude).InclusiveBetween(-180, 180);
            RuleFor(x => x.StartsAt).NotEmpty()
                .Must(dt => dt.Kind == DateTimeKind.Utc).WithMessage("StartsAt debe ser UTC.");
            RuleFor(x => x.DurationMinutes).GreaterThan(0);
            RuleFor(x => x.Status).IsInEnum();
        }
    }
}
