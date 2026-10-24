// profesor.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthGuard } from '../components/Autentificacion/auth.service';
@Injectable({
  providedIn: 'root',
})
export class ProfesorService {
  private baseUrl = 'http://localhost:5164';


  constructor(private http: HttpClient , private Authtoken :AuthGuard) {}
  private token = this.Authtoken.getToken(); 
  private getHeaders(): HttpHeaders {
    return new HttpHeaders({
      Authorization: `Bearer ${this.token}`,
      'Content-Type': 'application/json'
    });
  }

  listProfesores(): Observable<any> {
    return this.http.get(`${this.baseUrl}/professor/list`, { headers: this.getHeaders() });
  }

  createProfesor(profesor: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/professor/create`, profesor, { headers: this.getHeaders() });
  }

  updateProfesor(profesor: any): Observable<any> {
    return this.http.put(`${this.baseUrl}/professor/update`, profesor, { headers: this.getHeaders() });
  }

  deleteProfesor(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/professor/delete?professorId=${id}`, { headers: this.getHeaders() });
  }
}
