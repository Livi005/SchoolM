import { Component, OnInit } from '@angular/core';
import { CalificacionesService } from 'src/app/service/otergar.service';
import { AuthGuard } from 'src/app/components/Autentificacion/auth.service';
@Component({
  selector: 'app-evaluacion',
  templateUrl: './evaluacion.component.html',
  styleUrls: ['./evaluacion.component.scss']
})
export class EvaluacionComponent implements OnInit {
  calificaciones: any[] = [];

  constructor(private calificacionesService: CalificacionesService , private Autentificacion :AuthGuard) {}
  userName: string = this.Autentificacion.getUserName(); 
  ngOnInit() {
    this.cargarCalificaciones();
  }

  cargarCalificaciones() {
    this.calificacionesService.getCalificaciones(this.userName).subscribe(
      (response) => {
        console.log('Datos recibidos:', response);
        this.calificaciones = response || [];
      },
      (error) => {
        console.error('Error al cargar las calificaciones:', error);
      }
    );
  }

  showModal: boolean = false;
  selectedCalificacion: any = null;
  notaIngresada: number | null = null;
  
  abrirModal(calificacion: any) {
    this.selectedCalificacion = calificacion;
    this.notaIngresada = null; // Resetea la nota
    this.showModal = true;
  }
  
  cerrarModal() {
    this.showModal = false;
    this.selectedCalificacion = null;
    this.notaIngresada = null;
  }
  
  guardarNota() {
    if (this.notaIngresada !== null && !isNaN(Number(this.notaIngresada))) {
      const payload = {
        idProf: this.selectedCalificacion.idProf,
        idStud: this.selectedCalificacion.idStud,
        idSub: this.selectedCalificacion.idSub,
        idCourse: this.selectedCalificacion.idCourse,
        evaluation: Number(this.notaIngresada)
      };
  
      this.calificacionesService.asignarNota(payload).subscribe(
        () => {
          
          this.cerrarModal();
          this.cargarCalificaciones();
        },
        (error) => {
          console.error('❌ Error al asignar la nota:', error);
          alert('Error al asignar la nota');
        }
      );
    } else {
      alert('⚠ Debe ingresar un número válido');
    }
  }
}