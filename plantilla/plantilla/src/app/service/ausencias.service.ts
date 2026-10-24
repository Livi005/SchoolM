import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { AuthGuard } from 'src/app/components/Autentificacion/auth.service';  // Asegúrate de tener el guard importado

export interface StudentSubject {
  idStudSub?: number; // ID opcional, generado automáticamente al crear
  idStud: number;
  idSub: number;
  njAbsents: number;
}

@Injectable({
  providedIn: 'root',
})
export class StudentSubjectService {
  private baseUrl = 'http://localhost:5164/studentSubject'; // URL base del backend

  constructor(private http: HttpClient, private authGuard: AuthGuard) {}

  // Método privado para obtener encabezados con token de autenticación
  private getAuthHeaders(): HttpHeaders {
    const token = this.authGuard.getToken();
    return new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
  }

  // Crear un StudentSubject
  createStudentSubject(studentSubject: StudentSubject): Observable<StudentSubject> {
    const headers = this.getAuthHeaders();
    return this.http.post<StudentSubject>(`${this.baseUrl}/create`, studentSubject, { headers }).pipe(
      tap((data) => console.log('StudentSubject creado:', data)),
      catchError((error) => {
        console.error('Error al crear StudentSubject:', error);
        throw error;
      })
    );
  }

  // Listar StudentSubjects
  listStudentSubjects(): Observable<StudentSubject[]> {
    const headers = this.getAuthHeaders();
    return this.http.get<StudentSubject[]>(`${this.baseUrl}/list`, { headers }).pipe(
      tap((data) => console.log('Datos recibidos (listStudentSubjects):', data)),
      catchError((error) => {
        console.error('Error al listar StudentSubjects:', error);
        throw error;
      })
    );
  }
}
