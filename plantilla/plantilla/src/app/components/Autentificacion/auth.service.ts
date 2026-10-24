import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { tap } from 'rxjs/operators';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  private isLoggedIn = new BehaviorSubject<boolean>(this.hasSession()); // Detecta si hay sesión guardada
  isAuthenticated$ = this.isLoggedIn.asObservable(); // Observable para detectar cambios de sesión
  
  constructor(private http: HttpClient, private router: Router) {}

  private hasSession(): boolean {
    return !!localStorage.getItem('userSession');
  }
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    const userRole = localStorage.getItem('role');
    const expectedRole = route.data['role'];
    const token  = localStorage.getItem('token');
    if (userRole === expectedRole) {
      return true;
    } else {
      this.router.navigate(['/login']);
      return false;
    }
  }


  login(credentials: { userName: string; password: string }) {
    console.log("Intentando iniciar sesión...");
    return this.http.post('http://localhost:5164/identity/login', credentials).pipe(
      tap(response => {
        console.log("Respuesta completa del servidor:");
        console.dir(response, { depth: null });
      }),
      map(response => {
        console.log("Respuesta mapeada:");
        console.dir(response, { depth: null });
        return response as { verificstion: boolean, role: string , token: string };
      }),
      tap((response) => {
        console.log("Respuesta en tap:");
        console.dir(response, { depth: null });

        const role = response.role;
        const token = response.token;
        localStorage.setItem('role', role);
       
        localStorage.setItem('userName', credentials.userName);
        localStorage.setItem('token',token);
        localStorage.setItem('userSession', credentials.userName);
         this.isLoggedIn.next(true); // Notifica que se ha iniciado sesión
        

      }),
     )
    ;
  }

  logout() {
    localStorage.removeItem('userSession');
    this.isLoggedIn.next(false); // Notifica que se ha cerrado sesión
  }

  isAuthenticated(): boolean {
    return this.isLoggedIn.getValue();
  }

  getToken () : string | null{
    return localStorage.getItem('token');
  }
  getUserRole(): string | null {
    
    return localStorage.getItem('role');
  }
  getUserName(): string | null {
    
    return localStorage.getItem('userName');
  }
}
