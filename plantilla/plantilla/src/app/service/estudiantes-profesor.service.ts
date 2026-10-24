import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators'; // Importar el operador tap
import { AuthGuard } from 'src/app/components/Autentificacion/auth.service';

@Injectable({
  providedIn: 'root',
})
export class EstudentService {
  private apiBaseUrl = 'http://localhost:5164'; // URL base del API

  constructor(private http: HttpClient, private authService: AuthGuard) {}

  getStudents(): Observable<any[]> {
    const headers = this.getAuthHeaders();
    return this.http.get<any[]>(`${this.apiBaseUrl}/student/list`, { headers });
  }


  // Obtener los encabezados de autenticación
  private getAuthHeaders(): HttpHeaders {
    const token = this.authService.getToken(); // Obtener el token del servicio de autenticación
    if (!token) {
      console.error('Token no encontrado. Redirigiendo al login...');
      this.authService.logout();
      throw new Error('Token no encontrado');
    }
    return new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
  }
}
