import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';
import { AuthGuard } from 'src/app/components/Autentificacion/auth.service';

export interface TechnologicalMeans {
  idMean?: number; // ID opcional, ya que es generado automáticamente al crear
  nameMean: string;
  state: string;
}

@Injectable({
  providedIn: 'root',
})
export class TechnologicalMeansService {
  private baseUrl = 'http://localhost:5164/technologicalmeans'; // URL base del backend

  constructor(private http: HttpClient, private Authservice: AuthGuard) {}

  // Crear un medio tecnológico
  createTechnologicalMeans(technologicalMeans: TechnologicalMeans): Observable<TechnologicalMeans> {
    const headers = this.getAuthHeaders();
    return this.http.post<TechnologicalMeans>(`${this.baseUrl}/create`, technologicalMeans, { headers }).pipe(
      tap((data) => console.log('Medio creado:', data)),
      catchError((error) => {
        console.error('Error al crear medio:', error);
        throw error;
      })
    );
  }

  // Listar medios tecnológicos
  listTechnologicalMeans(): Observable<TechnologicalMeans[]> {
    const headers = this.getAuthHeaders();
    return this.http.get<TechnologicalMeans[]>(`${this.baseUrl}/list`, { headers }).pipe(
      tap((data) => console.log('Datos recibidos (listTechnologicalMeans):', data)),
      catchError((error) => {
        console.error('Error al listar medios:', error);
        throw error;
      })
    );
  }

  // Eliminar medio tecnológico por ID
  deleteTechnologicalMeans(id: number): Observable<any> {
    const headers = this.getAuthHeaders();
    return this.http.delete<any>(`${this.baseUrl}/delete?technologicalMeansId=${id}`, { headers }).pipe(
      tap(() => console.log('Medio eliminado con ID:', id)),
      catchError((error) => {
        console.error('Error al eliminar medio:', error);
        throw error;
      })
    );
  }

  // Actualizar medio tecnológico por ID
  updateTechnologicalMeans(technologicalMeans: TechnologicalMeans): Observable<TechnologicalMeans> {
    const headers = this.getAuthHeaders();
    return this.http.put<TechnologicalMeans>(`${this.baseUrl}/update`, technologicalMeans, { headers }).pipe(
      tap((data) => console.log('Medio actualizado:', data)),
      catchError((error) => {
        console.error('Error al actualizar medio:', error);
        throw error;
      })
    );
  }

  // Método privado para obtener encabezados con token de autenticación
  private getAuthHeaders(): HttpHeaders {
    const token = this.Authservice.getToken();
    return new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
  }
}
