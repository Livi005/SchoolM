import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthGuard } from '../components/Autentificacion/auth.service';

@Injectable({
  providedIn: 'root',
})
export class SolicitudService {
  private apiUrl = 'http://localhost:5164';
  private headers: HttpHeaders;

  constructor(private http: HttpClient, private Authservice: AuthGuard) {
    const token = this.Authservice.getToken();
    this.headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
      'Content-Type': 'application/json',
    });
  }

  checkAvailableClassroomsAndMeans(userName: string): Observable<any> {
    console.log(userName);
    return this.http.post(`${this.apiUrl}/classroommeans/checkAviableClassRoomsAndMeans`, {userName}, {
        headers: this.headers,
      });
  }
  getUser() {
    return this.Authservice.getUserRole();
  }
  reserveClassRoomAndMeans(requestPayload: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/classroommeans/reserveClassRoomAndMeans`, requestPayload, {
      headers: this.headers,
    });
  }
  NotcheckAvailableClassroomsAndMeans(userName: string): Observable<any> {
    console.log(userName);
    return this.http.post(`${this.apiUrl}/classroommeans/checkNotAviableClassRoomsAndMeans`, {userName}, {
        headers: this.headers,
      });
  }
}
