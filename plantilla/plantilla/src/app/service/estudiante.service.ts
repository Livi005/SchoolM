import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthGuard } from 'src/app/components/Autentificacion/auth.service';
@Injectable({
  providedIn: 'root',
})
export class EstudentService {
  private apiBaseUrl = 'http://localhost:5164'; 

  constructor(private http: HttpClient , private Authservice: AuthGuard) {}

  // Obtener la lista de estudiantes
  getStudents(): Observable<any[]> {
    const headers = this.getAuthHeaders();
    return this.http.get<any[]>(`${this.apiBaseUrl}/student/list`, { headers });
  }

  // Crear un estudiante
  createStudent(student: { nameStud: string; age: number; eActivity: boolean; idC: number }): Observable<any> {
    const headers = this.getAuthHeaders();
    return this.http.post(`${this.apiBaseUrl}/student/create`, student, { headers });
  }

  // Actualizar un estudiante
  updateStudent(studentUpdate: { id: number; idc: number; student: { nameStud: string; age: number; eActivity: boolean } }): Observable<any> {
    const headers = this.getAuthHeaders();
    return this.http.put(`${this.apiBaseUrl}/student/update`, studentUpdate, { headers });
  }
  
  // Eliminar un estudiante
  deleteStudent(studentId: number): Observable<any> {
    const headers = this.getAuthHeaders();
    return this.http.delete(`${this.apiBaseUrl}/student/delete?studentId=${studentId}`, { headers });
  }

  // Obtener los encabezados de autenticación
  private getAuthHeaders(): HttpHeaders {
    const token = this.Authservice.getToken(); // Reemplaza esto con la lógica para obtener el token dinámico
    return new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
  }
}
