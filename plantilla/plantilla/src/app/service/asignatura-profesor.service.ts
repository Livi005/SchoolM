import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthGuard } from 'src/app/components/Autentificacion/auth.service'; // Importa tu servicio de autenticación
@Injectable({
  providedIn: 'root',
})
export class Solicitud {
  private apiUrl = 'http://localhost:5164';
  private headers: HttpHeaders;

  constructor(private http: HttpClient, private Authservice: AuthGuard) {
    const token = this.Authservice.getToken();
    this.headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
      'Content-Type': 'application/json',
    });
  }

  // Obtener asignaturas disponibles
  getSubjects(): Observable<any> {
    return this.http.get(`${this.apiUrl}/subject/list`, { headers: this.headers });
  }

  // Obtener profesores disponibles para una asignatura
  getProfessorsForSubject(subjectId: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/professor/list`, { headers: this.headers });
  }

  // Asignar profesor a una asignatura
  assignProfessorToSubject(data: { idProf: number, idSub: number }): Observable<any> {
    return this.http.post(`${this.apiUrl}/professorSubject/create`, data, { headers: this.headers });
  }

  // Obtener asignaciones de profesores a asignaturas
  getProfessorSubjectAssignments(): Observable<any> {
    return this.http.get(`${this.apiUrl}/professorSubject/list`, { headers: this.headers });
  }

  // Eliminar asignación de profesor
  deleteProfessorSubjectAssignment(idProfSub: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/professorSubject/delete?idProfSub=${idProfSub}`, { headers: this.headers });
  }
}
