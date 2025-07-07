using FluentAssertions;
using Moq;
using RoverMissionPlanner.Application.Services;
using RoverMissionPlanner.Domain.Entities;
using RoverMissionPlanner.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoverMissionPlanner.Tests.Application.Services
{
    public class RoverTaskServiceTests
    {
        // Verifica que no se permita crear una tarea que se solapa con otra
        [Fact]
        public async Task CreateTaskAsync_ShouldReturnFalse_WhenTaskOverlaps()
        {
            var service = new RoverTaskService();
            var roverId = Guid.NewGuid();

            var existingTask = new RoverTask
            {
                RoverName = "Curiosity",
                TaskType = TaskType.Drill,
                Latitude = 0,
                Longitude = 0,
                StartsAt = new DateTime(2025, 07, 07, 10, 00, 00, DateTimeKind.Utc),
                DurationMinutes = 60,
                Status = Domain.Enums.TaskStatus.Planned
            };

            // Primero se agrega la tarea existente al servicio real
            await service.CreateTaskAsync(roverId, existingTask);

            var newTask = new RoverTask
            {
                RoverName = "Curiosity",
                TaskType = TaskType.Sample,
                Latitude = 1,
                Longitude = 1,
                StartsAt = new DateTime(2025, 07, 07, 10, 30, 00, DateTimeKind.Utc), // Solapado
                DurationMinutes = 60,
                Status = Domain.Enums.TaskStatus.Planned
            };

            var result = await service.CreateTaskAsync(roverId, newTask);

            result.Should().BeFalse("Porque la nueva tarea se solapa con una existente.");
        }

        // Crear tarea para otro rover
        [Fact]
        public async Task CreateTaskAsync_ShouldAllowSameTimeForDifferentRovers()
        {
            var service = new RoverTaskService();
            var rover1 = Guid.NewGuid();
            var rover2 = Guid.NewGuid();

            var taskForRover1 = new RoverTask
            {
                RoverName = "Curiosity",
                TaskType = TaskType.Drill,
                Latitude = 0,
                Longitude = 0,
                StartsAt = new DateTime(2025, 07, 07, 10, 00, 00, DateTimeKind.Utc),
                DurationMinutes = 60,
                Status = Domain.Enums.TaskStatus.Planned
            };

            var taskForRover2 = new RoverTask
            {
                RoverName = "Perseverance",
                TaskType = TaskType.Photo,
                Latitude = 0,
                Longitude = 0,
                StartsAt = new DateTime(2025, 07, 07, 10, 00, 00, DateTimeKind.Utc), // Mismo horario
                DurationMinutes = 60,
                Status = Domain.Enums.TaskStatus.Planned
            };

            var result1 = await service.CreateTaskAsync(rover1, taskForRover1);
            var result2 = await service.CreateTaskAsync(rover2, taskForRover2);

            result1.Should().BeTrue();
            result2.Should().BeTrue("Porque las tareas pertenecen a rovers distintos.");
        }

        // Crear tareas para otro día
        [Fact]
        public async Task CreateTaskAsync_ShouldAllowTaskOnDifferentDay()
        {
            var service = new RoverTaskService();
            var roverId = Guid.NewGuid();

            var day1Task = new RoverTask
            {
                RoverName = "Curiosity",
                TaskType = TaskType.Sample,
                Latitude = 0,
                Longitude = 0,
                StartsAt = new DateTime(2025, 07, 07, 10, 00, 00, DateTimeKind.Utc),
                DurationMinutes = 60,
                Status = Domain.Enums.TaskStatus.Planned
            };

            var day2Task = new RoverTask
            {
                RoverName = "Curiosity",
                TaskType = TaskType.Drill,
                Latitude = 1,
                Longitude = 1,
                StartsAt = new DateTime(2025, 07, 08, 10, 00, 00, DateTimeKind.Utc),
                DurationMinutes = 60,
                Status = Domain.Enums.TaskStatus.Planned
            };

            var result1 = await service.CreateTaskAsync(roverId, day1Task);
            var result2 = await service.CreateTaskAsync(roverId, day2Task);

            result1.Should().BeTrue();
            result2.Should().BeTrue("Porque las tareas están en días distintos.");
        }

        // Crear tareas adyacentes
        [Fact]
        public async Task CreateTaskAsync_ShouldAllowAdjacentTasks()
        {
            var service = new RoverTaskService();
            var roverId = Guid.NewGuid();

            var firstTask = new RoverTask
            {
                RoverName = "Curiosity",
                TaskType = TaskType.Sample,
                Latitude = 0,
                Longitude = 0,
                StartsAt = new DateTime(2025, 07, 07, 10, 00, 00, DateTimeKind.Utc),
                DurationMinutes = 60,
                Status = Domain.Enums.TaskStatus.Planned
            };

            var adjacentTask = new RoverTask
            {
                RoverName = "Curiosity",
                TaskType = TaskType.Photo,
                Latitude = 1,
                Longitude = 1,
                StartsAt = new DateTime(2025, 07, 07, 11, 00, 00, DateTimeKind.Utc), // justo al terminar la anterior
                DurationMinutes = 30,
                Status = Domain.Enums.TaskStatus.Planned
            };

            var result1 = await service.CreateTaskAsync(roverId, firstTask);
            var result2 = await service.CreateTaskAsync(roverId, adjacentTask);

            result1.Should().BeTrue();
            result2.Should().BeTrue("Porque las tareas son contiguas y no se solapan");
        }

        // Verifica que solo se devuelven las tareas correspondientes a una fecha específica
        [Fact]
        public async Task GetTasksByDateAsync_ShouldReturnOnlyTasksForTheSpecifiedDate()
        {
            var service = new RoverTaskService();
            var roverId = Guid.NewGuid();

            var taskToday = new RoverTask
            {
                RoverName = "Curiosity",
                TaskType = TaskType.Sample,
                Latitude = 0,
                Longitude = 0,
                StartsAt = new DateTime(2025, 07, 07, 10, 00, 00, DateTimeKind.Utc),
                DurationMinutes = 60,
                Status = Domain.Enums.TaskStatus.Planned
            };

            var taskTomorrow = new RoverTask
            {
                RoverName = "Curiosity",
                TaskType = TaskType.Photo,
                Latitude = 1,
                Longitude = 1,
                StartsAt = new DateTime(2025, 07, 08, 10, 00, 00, DateTimeKind.Utc),
                DurationMinutes = 30,
                Status = Domain.Enums.TaskStatus.Planned
            };

            await service.CreateTaskAsync(roverId, taskToday);
            await service.CreateTaskAsync(roverId, taskTomorrow);

            var tasks = await service.GetTasksByDateAsync(roverId, new DateTime(2025, 07, 07));

            tasks.Should().HaveCount(1, "Porque solo hay una tarea el 07 de julio");
            tasks.First().StartsAt.Should().Be(taskToday.StartsAt);
        }

        // Verifica que se calcule correctamente el porcentaje de minutos utilizados en el día
        [Fact]
        public async Task GetUtilizationByDateAsync_ShouldReturnCorrectPercentage()
        {
            var service = new RoverTaskService();
            var roverId = Guid.NewGuid();

            var task1 = new RoverTask
            {
                RoverName = "Curiosity",
                TaskType = TaskType.Drill,
                Latitude = 0,
                Longitude = 0,
                StartsAt = new DateTime(2025, 07, 07, 08, 00, 00, DateTimeKind.Utc),
                DurationMinutes = 60,
                Status = Domain.Enums.TaskStatus.Planned
            };

            var task2 = new RoverTask
            {
                RoverName = "Curiosity",
                TaskType = TaskType.Sample,
                Latitude = 0,
                Longitude = 0,
                StartsAt = new DateTime(2025, 07, 07, 10, 00, 00, DateTimeKind.Utc),
                DurationMinutes = 120,
                Status = Domain.Enums.TaskStatus.Planned
            };

            await service.CreateTaskAsync(roverId, task1);
            await service.CreateTaskAsync(roverId, task2);

            var utilization = await service.GetUtilizationByDateAsync(roverId, new DateTime(2025, 07, 07));

            utilization.Should().BeApproximately((180.0 / 1440.0) * 100, 0.01,
                "Porque se utilizaron 180 minutos de un total de 1440 en el día");
        }
    }
}
