import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthGuard } from '../components/Autentificacion/auth.service';
@Injectable({
  providedIn: 'root',
})
export class TechnologicalMeansService {
  private apiBaseUrl = 'http://localhost:5164'; // Cambia la URL base si es necesario

  constructor(private http: HttpClient , private AuthGuar : AuthGuard) {}

  // Crear un medio tecnológico
  createMean(mean: { nameMean: string; state: string }): Observable<any> {
    const token = this.getToken();
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });

    return this.http.post(`${this.apiBaseUrl}/auxiliarymeans/create`, mean, { headers });
  }

  // Obtener el token de autenticación
  private getToken(): string {
    // Lógica para obtener el token de tu servicio de autenticación
    return this.AuthGuar.getToken();
  }
}
