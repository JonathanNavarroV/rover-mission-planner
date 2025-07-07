import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { RoverTask } from '../models/rover-task.model';

@Injectable({
  providedIn: 'root',
})
export class RoverTaskService {
  // URL Base de la API
  private baseUrl = 'https://localhost:7190/rovers';

  private readonly http = inject(HttpClient);

  /**
   * Obtiene las tareas de un rover en una fecha específica
   * @param roverId ID del rover
   * @param date Fecha en formato YYYY-MM-DD
   * @returns Observable con lista de tareas
   */
  getTasksByDate(roverId: string, date: string): Observable<RoverTask[]> {
    return this.http.get<RoverTask[]>(
      `${this.baseUrl}/${roverId}/tasks?date=${date}`
    );
  }
}
