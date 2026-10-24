import { Component, OnInit } from '@angular/core';
import { CalificacionesService } from 'src/app/service/calificacionesPofesores.service';
@Component({
  selector: 'app-calificacion',
  templateUrl: './calificacion.component.html',
  styleUrls: ['./calificacion.component.scss']
})
export class CalificacionComponent implements OnInit {

  constructor(private calificacionesService : CalificacionesService) { }

  evaluaciones :any[] = [];
  
  ngOnInit(): void {
    this.cargarEvaluaciones()
  }
  cargarEvaluaciones(): void {
    console.log('Cargando las calificaciones...'); // Se ejecuta primero
    
    this.calificacionesService.listCalificaciones().subscribe(
      (response: any) => {
        if (response) {
          this.evaluaciones = response;
          console.log('siiiiiiiiiiiiiiu');
          console.log(this.evaluaciones); // Este se ejecuta después de recibir los datos
        } 
      }
    );

    // ❌ ESTE SE EJECUTA ANTES QUE LA RESPUESTA DE LA API
    // console.log(this.evaluaciones); 
}
}
