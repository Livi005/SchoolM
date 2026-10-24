import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthGuard } from '../components/Autentificacion/auth.service';

@Injectable({
  providedIn: 'root',
})
export class SecretaryService {
  private apiBaseUrl = 'http://localhost:5164'; // Reemplaza con tu URL base
  
  // Define los encabezados con el token
  private headers: HttpHeaders;

  constructor(private http: HttpClient, private Authservice: AuthGuard) {
    const token = this.Authservice.getToken(); // Obtén el token del servicio AuthGuard
    this.headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
  }
  // Método para crear una secretaria
  createSecretary(secretary: { nameS: string; salaryS: number }): Observable<any> {
    return this.http.post(`${this.apiBaseUrl}/secretary/create`, secretary, {
      headers: this.headers, // Usa this.headers
    });
  }
  updateSecretary(secretary: { id: number; secretary: { nameS: string; salaryS: number } }): Observable<any> {
  
    return this.http.put(`${this.apiBaseUrl}/secretary/update`, secretary, { headers :this.headers });
  }
  
  // Método para obtener la lista de secretarias
  getSecretaries(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiBaseUrl}/secretary/list`, { 
        headers :this.headers ,
     });
     
  }updateStudentNote(profStudSubId: number, studentGrade: number): Observable<void> {
    const url = `${this.apiBaseUrl}/professorStudentSubject/updateStudentNote?profStudSubId=${profStudSubId}&studentGrade=${studentGrade}`;
    return this.http.post<void>(url, null, { headers: this.headers });
  }
  
  
  deleteSecretary(secretaryId: number): Observable<any> {
    return this.http.delete(`${this.apiBaseUrl}/secretary/delete?secretaryId=${secretaryId}`, { headers:this.headers });
  }
  getsubject ()
  {
    return this.http.get(`${this.apiBaseUrl}/subject/list` , {
      headers :this.headers ,
    })
  }
  getSecretNotas(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiBaseUrl}/professorStudentSubject/listnotes`, { 
        headers :this.headers ,
     });
  }
}
