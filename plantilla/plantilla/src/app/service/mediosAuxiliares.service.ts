import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators'; // Importar el operador tap
import { AuthGuard } from 'src/app/components/Autentificacion/auth.service';

export interface AuxiliaryMeans {
  idMean?: number;
  nameMean: string;
  state: string;
  type: string;
}

@Injectable({
  providedIn: 'root',
})
export class AuxiliaryMeansService {
  private baseUrl = 'http://localhost:5164/auxiliarymeans'; // Contexto correcto

  constructor(private http: HttpClient, private Authservice: AuthGuard) {}

  // Crear medio auxiliar
  createAuxiliaryMeans(auxiliaryMeans: AuxiliaryMeans): Observable<AuxiliaryMeans> {
    const headers = this.getAuthHeaders();
    console.log('Datos enviados al backend (createAuxiliaryMeans):', auxiliaryMeans);

    return this.http.post<AuxiliaryMeans>(`${this.baseUrl}/create`, auxiliaryMeans, { headers });
  }

  // Listar medios auxiliares con log
  listAuxiliaryMeans(): Observable<AuxiliaryMeans[]> {
    const headers = this.getAuthHeaders();
    return this.http.get<AuxiliaryMeans[]>(`${this.baseUrl}/list`, { headers }).pipe(
      tap((data) => {
        console.log('Datos recibidos del backend (listAuxiliaryMeans):', data);
      })
    );
  }

  // Actualizar medio auxiliar
  updateAuxiliaryMeans(auxiliaryMeans: AuxiliaryMeans): Observable<AuxiliaryMeans> {
    const headers = this.getAuthHeaders();
    return this.http.put<AuxiliaryMeans>(`${this.baseUrl}/update`, auxiliaryMeans, { headers });
  }

  // Eliminar medio auxiliar
  deleteAuxiliaryMeans(id: number): Observable<any> {
    const headers = this.getAuthHeaders();
    return this.http.delete<any>(`${this.baseUrl}/delete?auxiliaryMeansId=${id}`, { headers });
  }

  // Obtener los encabezados de autenticación
  private getAuthHeaders(): HttpHeaders {
    const token = this.Authservice.getToken(); // Obtiene el token desde AuthGuard
    return new HttpHeaders({
      Authorization: `Bearer ${token}`, // Incluye el token en los encabezados
    });
  }
}
