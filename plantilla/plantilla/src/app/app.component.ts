import { Component } from "@angular/core";
import { AuthGuard } from "./components/Autentificacion/auth.service";
import { Observable } from "rxjs";
@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.scss"],
})
export class AppComponent {
  title = "Gestion-Escolar";
  isAuthenticated: Observable<boolean>;
  constructor(public authService: AuthGuard) {
    this.isAuthenticated = this.authService.isAuthenticated$;
  }

  logout() {
    this.authService.logout();
  }
}
