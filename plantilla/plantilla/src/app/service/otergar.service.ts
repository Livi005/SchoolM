import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CalificacionesService {
  private apiUrl = 'http://localhost:5164'; // Reemplaza con la URL real
  private headers = new HttpHeaders({ 'Content-Type': 'application/json' });

  constructor(private http: HttpClient) {}

  // Obtener la lista de profesores, asignaturas y cursos
  getCalificaciones(userName: string): Observable<any[]> {
    const url = `${this.apiUrl}/profstudsubcourse/listProfessorsByStudent`;
    const params = new HttpParams().set('userName', userName);
    return this.http.get<any[]>(url, { headers: this.headers, params });
  }

  // Asignar una nota al estudiante
  asignarNota(payload: any): Observable<void> {
    const url = `${this.apiUrl}/profstudsubcourse/create`;
    return this.http.post<void>(url, payload, { headers: this.headers });
  }
}
