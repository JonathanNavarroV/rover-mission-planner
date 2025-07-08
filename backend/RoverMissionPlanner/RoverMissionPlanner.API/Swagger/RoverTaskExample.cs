using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using RoverMissionPlanner.Domain.Entities;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace RoverMissionPlanner.API.Swagger
{
    public class RoverTaskExample:ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(RoverTask))
            {
                schema.Example = new OpenApiObject
                {
                    ["roverName"] = new OpenApiString("Curiosity"),
                    ["taskType"] = new OpenApiString("Drill"),
                    ["latitude"] = new OpenApiDouble(-4.5895),
                    ["longitude"] = new OpenApiDouble(137.4417),
                    ["startsAt"] = new OpenApiString("2025-07-07T10:00:00Z"),
                    ["durationMinutes"] = new OpenApiInteger(60),
                    ["status"] = new OpenApiString("Planned")
                };
            }
        }
    }
}
