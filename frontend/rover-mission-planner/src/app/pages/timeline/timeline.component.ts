import { CommonModule } from '@angular/common';
import { Component, Input, OnChanges, OnInit } from '@angular/core';
import { TimelineModule } from 'primeng/timeline';
import { RoverTask } from '../../core/models/rover-task.model';
import { RoverTaskService } from '../../core/services/rover-task.service';

@Component({
  selector: 'app-timeline',
  standalone: true,
  imports: [CommonModule, TimelineModule],
  templateUrl: './timeline.component.html',
  styleUrl: './timeline.component.scss',
})
export class TimelineComponent implements OnChanges {
  // ID de ejemplo para el rover a consultar
  @Input() roverId!: string;

  // Fecha seleccionada en formato YYYY-MM-DD
  @Input() date!: string;

  // Array para guardar lad tareas obtenias
  tasks: RoverTask[] = [];

  constructor(private roverTaskService: RoverTaskService) {}

  /**
   * - Carga las tareas para la fecha actual
   */
  ngOnChanges(): void {
    this.loadTasks();
  }

  /**
   * Método que llama al servicio para obtener las tareas
   * y actualiza el array `tasks` con los datos recibidos.
   */
  loadTasks() {
    this.roverTaskService
      .getTasksByDate(this.roverId, this.date)
      .subscribe((tasks) => (this.tasks = tasks));
  }
}
