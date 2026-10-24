import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthGuard } from '../components/Autentificacion/auth.service';

interface Classroom {
  idClassR: number;
  location: string;
}

@Injectable({
  providedIn: 'root'
})
export class ClassroomService {
  private apiUrl = 'http://localhost:5164/classroom';
  private headers: HttpHeaders
  private getAuthHeaders(): HttpHeaders {
    const token = this.Authservice.getToken(); 
    console.log('Token de autenticación:', token);
    return new HttpHeaders({
      Authorization: `Bearer ${token}`, 
    });
  }
  constructor(private http: HttpClient,private Authservice: AuthGuard) {}

  getClassrooms(): Observable<Classroom[]> {
    return this.http.get<Classroom[]>(`${this.apiUrl}/list`, { headers: this.headers });
  }

  createClassroom(classroom: Classroom): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/create`, classroom ,{ headers: this.headers });
  }

  updateClassroom(classroom: Classroom): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/update`, classroom , { headers: this.headers });
  }

  deleteClassroom(idClassR: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/delete?classRoomId=${idClassR}` ,{ headers: this.headers });
  }
 
}
