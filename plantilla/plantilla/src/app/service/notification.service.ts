import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, interval } from 'rxjs';
import { Observable } from 'rxjs';
import { AuthGuard } from '../components/Autentificacion/auth.service';
import { switchMap, map } from 'rxjs/operators';
@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private apiUrl = 'http://localhost:5164/notifiction/get';
  private notificationsSubject = new BehaviorSubject<string[]>([]);
  public notifications$ = this.notificationsSubject.asObservable();
  private lastNotificationCount = 0;
  private headers: HttpHeaders;
  constructor(private http: HttpClient ,private authService: AuthGuard,) {
    
     const token = this.authService.getToken(); // Obtén el token del servicio AuthGuard
        this.headers = new HttpHeaders({
          Authorization: `Bearer ${token}`,
        });
  }
  userId = this.authService.getUserName();

  
   getNotifications()  {
    const params = new HttpParams().set('UserId', this.userId);
    return this.http.get<any>(this.apiUrl, { params  ,headers:this.headers}).pipe(
      map(response => {
        console.log (response);
      })
    );
  }
  fetchNotifications() {
    this.http.get<string[]>(this.apiUrl).subscribe(
      (data) => this.notificationsSubject.next(data),
      (error) => console.error('❌ Error obteniendo notificaciones:', error)
    );
  }

  private showToast(message: string) {
    console.log(`TOAST: ${message}`);
  }
}
