import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthGuard } from '../components/Autentificacion/auth.service';

@Injectable({
  providedIn: 'root',
})
export class MantenimientoService {
  private baseUrl = 'http://localhost:5164'; // URL base del backend

  constructor(private http: HttpClient, private authToken: AuthGuard) {}

  // Obtiene el token de autenticación del servicio AuthGuard
  private token = this.authToken.getToken();

  // Método para obtener los encabezados de las peticiones HTTP, incluyendo el token de autenticación
  private getHeaders(): HttpHeaders {
    return new HttpHeaders({
      Authorization: `Bearer ${this.token}`, // Encabezado para autenticación con JWT
      'Content-Type': 'application/json' // Indica que los datos enviados son en formato JSON
    });
  }

  // Obtiene la lista de mantenimientos desde el backend
  listMantenimientos(): Observable<any> {
    return this.http.get(`${this.baseUrl}/maintenance/list`, { headers: this.getHeaders() });
  }

  // Crea un nuevo mantenimiento enviando los datos al backend
  createMantenimiento(mantenimiento: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/maintenance/create`, mantenimiento, { headers: this.getHeaders() });
  }

  // Actualiza un mantenimiento existente
  updateMantenimiento(mantenimiento: any): Observable<any> {
    return this.http.put(`${this.baseUrl}/mantenimiento/update`, mantenimiento, { headers: this.getHeaders() });
  }

  // Elimina un mantenimiento por su ID
  deleteMantenimiento(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/mantenimiento/delete?idM=${id}`, { headers: this.getHeaders() });
  }
}
