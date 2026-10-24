import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthGuard } from '../components/Autentificacion/auth.service';
@Injectable({
  providedIn: 'root',
})
export class StudentGradingService {
  private apiUrl = 'http://localhost:5164';
  private headers: HttpHeaders;

  constructor(private http: HttpClient , private Authent :AuthGuard) {
    const token = this.Authent.getToken(); // Replace with your token retrieval logic
    this.headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
      'Content-Type': 'application/json',
    });
  }

  getSubjects(userName: string): Observable<any[]> {
    const url = `${this.apiUrl}/professorStudentSubject/getsubjects`;
    const params = new HttpParams().set('userName', userName);
    return this.http.get<any[]>(url, { headers: this.headers, params });
  }

  getStudents(IdSub: number): Observable<any[]> {
    const url = `${this.apiUrl}/professorStudentSubject/getstudents`;
    const params = new HttpParams().set('IdSub', IdSub.toString());
    return this.http.get<any[]>(url, { headers: this.headers, params });
  }
  submitGrade(payload: any): Observable<void> {
    const url = `${this.apiUrl}/professorStudentSubject/update`;
    return this.http.post<void>(url, payload, { headers: this.headers });
  }
  assignGrade(payload: any): Observable<void> {
    const url = `${this.apiUrl}/professorStudentSubject/givenote`;
    return this.http.post<void>(url, payload, { headers: this.headers });
  }
  getNotes(UserName: string): Observable<any[]> {
    const url = `${this.apiUrl}/professorStudentSubject/getnotesByProfessor?UserName=${encodeURIComponent(UserName)}`;
    return this.http.get<any[]>(url, { headers: this.headers });
  }
  
  listResults(): Observable<any[]> {
    const url = `${this.apiUrl}/profstudsubcourse/list`;
    return this.http.get<any[]>(url, { headers: this.headers });
  }
  getRole ()
  {
    return this.Authent.getUserRole();
  }
}
