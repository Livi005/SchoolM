import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthGuard } from '../components/Autentificacion/auth.service';

@Injectable({
  providedIn: 'root',
})
export class ProfessorService {
  private baseUrl = 'http://localhost:5164';

  constructor(private http: HttpClient, private Authtoken: AuthGuard) {}

  private token = this.Authtoken.getToken();

  private getHeaders(): HttpHeaders {
    return new HttpHeaders({
      Authorization: `Bearer ${this.token}`,
      'Content-Type': 'application/json',
    });
  }

  getSpecs(): Observable<{ spec: string[] }> {
    return this.http.get<{ spec: string[] }>(`${this.baseUrl}/professorClassRoom/getSpecs`, { headers: this.getHeaders() });
  }

  getProfessorsBySpec(spec: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/professorClassRoom/getProfessorsBySpec?spec=${spec}`, { headers: this.getHeaders() });
  }

  getMaintenances(): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/classroom/getMaintenances`, { headers: this.getHeaders() });
  }
  getMaintenanceCosts(): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/classroom/getMaintenancesAverageCosts`, { headers: this.getHeaders() });
  }
  getGoodProfessors () : Observable<any>
  {
    return this.http.get<any>(`${this.baseUrl}/professor/getGoodProfessors` , { headers: this.getHeaders() });
  }
  getBadStudents ():Observable<any>
  {
    return this.http.get<any>(`${this.baseUrl}/student/getBadStudents` , { headers: this.getHeaders() })
  }
  getBadProfesor() :Observable<any>
  {
    return this.http.get<any>(`${this.baseUrl}/professor/getBadProfessors` , { headers: this.getHeaders() })
  }
}
