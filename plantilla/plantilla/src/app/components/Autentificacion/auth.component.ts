// auth.component.ts
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthGuard } from './auth.service';
import { Router } from '@angular/router';
@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css'],
})

export class AuthComponent {
  loginForm: FormGroup;
  userData = {userName : '', password: '' };
  rememberMe = false;
  constructor(private fb: FormBuilder, private authGuard: AuthGuard ,private router: Router,) {
  }

  ngOnInit() {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }
  // Título de la aplicación
  title = 'Escuela';

  // Estilos para controlar la visibilidad de los formularios
  loginStyles = { left: '4px', opacity: '1' };
  registerStyles = { right: '-520px', opacity: '0' };

  // Estado del menú responsivo
  isMenuResponsive = false;

  // Función para mostrar el formulario de inicio de sesión
  showLogin() {
    this.loginStyles = { left: '4px', opacity: '1' };
    this.registerStyles = { right: '-520px', opacity: '0' };
  }

  // Función para alternar el menú
  toggleMenu() {
    this.isMenuResponsive = !this.isMenuResponsive;
  }
  onSubmit() {
    if (this.loginForm.valid) {
      const username = this.loginForm.get('username');
      const password = this.loginForm.get('password');
      console.log('enviando');
      if (username && password) {
        this.userData = {
          userName: username.value,
          password: password.value
        };
  
  
        this.authGuard.login(this.userData).subscribe(
          response => console.log(response),
          error => {
            console.error('Error al iniciar sesión:', error);
            alert('Ha ocurrido un error al iniciar sesión. Por favor, inténtelo nuevamente.');
          }
        );
      } else {
        console.error('No se pudo obtener los controles del formulario');
        alert('Ha habido un error al procesar su solicitud.');
      }
      this.router.navigate(['/inicio']); // Redirige después del login
    } else {
      console.log('Form is invalid:', this.loginForm.errors);
      alert('Por favor, complete todos los campos obligatorios.');
    }
  }
 
}
