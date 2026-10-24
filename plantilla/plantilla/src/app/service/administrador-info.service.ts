import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthGuard } from '../components/Autentificacion/auth.service';
@Injectable({
  providedIn: 'root',
})
export class AdministratorService {
  private apiBaseUrl = 'http://localhost:5164'; // Reemplaza con tu URL base

  // Define los encabezados con el token
    private headers: HttpHeaders;
  
    constructor(private http: HttpClient, private Authservice: AuthGuard) {
      const token = this.Authservice.getToken(); // Obtén el token del servicio AuthGuard
      this.headers = new HttpHeaders({
        Authorization: `Bearer ${token}`,
      });
    }

  // Método para crear un administrador
  createAdministrator(administrator: { adminName: string;adminSalary: number }): Observable<any> {
    return this.http.post(`${this.apiBaseUrl}/administrator/create`, administrator, {
        headers: this.headers, // Usa this.headers
      });
  }

  // Método para obtener la lista de administradores
  getAdministrators(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiBaseUrl}/administrator/list` ,{
        headers: this.headers, // Usa this.headers
      });
  }

  // Método para actualizar un administrador
  updateAdministrator(administrator: { id: number; administrator: { adminName: string; adminSalary: number } }): Observable<any> {
    return this.http.put(`${this.apiBaseUrl}/administrator/update`, administrator ,{
        headers: this.headers, // Usa this.headers
      });
  }

  // Método para eliminar un administrador
  deleteAdministrator(administratorId: number): Observable<any> {
    return this.http.delete(`${this.apiBaseUrl}/administrator/delete?administratorId=${administratorId}` ,{
        headers: this.headers, // Usa this.headers
      });
  }
}
