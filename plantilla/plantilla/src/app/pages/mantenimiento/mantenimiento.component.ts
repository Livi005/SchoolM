import { Component, OnInit } from '@angular/core';
import { MantenimientoService } from 'src/app/service/mantenimiento.service';

@Component({
  selector: 'app-mantenimiento',
  templateUrl: './mantenimiento.component.html',
  styleUrls: ['./mantenimiento.component.css']
})
export class MantenimientoComponent implements OnInit {
  mantenimientos: any[] = [];
  globalSearchQuery: string = '';
  showModal: boolean = false;

  // Objeto para nuevo mantenimiento
  newMantenimiento = {
    maintenanceDate: '',
    cost: 0,
    idMean: 0,
    typeOfMean: 0,

    idAuxMean: 0,
    idTechMean: 0
     // Agregar esta propiedad para evitar el error
  };


  typeOfMean: string = '0'; // 0 = Tecnológico, 1 = Auxiliar

  constructor(private mantenimientoService: MantenimientoService) {}

  ngOnInit(): void {
    this.loadMantenimientos();
  }

  loadMantenimientos(): void {
    this.mantenimientoService.listMantenimientos().subscribe(
      (data) => {
        this.mantenimientos = data;
      },
      (error) => {
        console.error('Error al listar mantenimientos:', error);
      }
    );
  }

  updateDisplayMantenimientos(): void {
    this.mantenimientos = this.mantenimientos.filter(mantenimiento =>
      mantenimiento.typeOfMean.toLowerCase().includes(this.globalSearchQuery.toLowerCase())
    );
  }

  openModal(): void {
    this.showModal = true;
  }

  closeModal(): void {
    this.showModal = false;
  }

  onTipoMedioChange(): void {
    // Dependiendo del tipo de medio, ajustamos los valores de ID
    if (this.typeOfMean === '0') {
      this.newMantenimiento.idTechMean = this.newMantenimiento.idMean;
      this.newMantenimiento.idAuxMean = 0;
      this.newMantenimiento.idMean = 0;


    } else {
      this.newMantenimiento.idTechMean = 0;
      this.newMantenimiento.idAuxMean = this.newMantenimiento.idMean;
      this.newMantenimiento.idMean = 0;
    }
  }

  addMantenimiento(): void {

    // Asegúrate de que la fecha esté en formato YYYY-MM-DD
    if (this.newMantenimiento.maintenanceDate) {
      let fecha = new Date(this.newMantenimiento.maintenanceDate); // Convierte a un objeto Date
      this.newMantenimiento.maintenanceDate = fecha.toISOString().split('T')[0]; // Convierte a YYYY-MM-DD
    }

    // Verifica si hay otros campos requeridos
    if (!this.newMantenimiento.maintenanceDate || !this.newMantenimiento.cost || !this.newMantenimiento.idMean) {
      console.error('Faltan datos para agregar el mantenimiento');
      return; // No se envía la solicitud si hay datos faltantes
    }
    this.onTipoMedioChange()
    console.log("Datos enviados:", JSON.stringify(this.newMantenimiento, null, 2)); // Mostrar los datos
    this.mantenimientoService.createMantenimiento(this.newMantenimiento).subscribe(
      (response) => {
        console.log('Mantenimiento agregado:', response);
        this.loadMantenimientos(); // Recargar la lista
        this.closeModal(); // Cerrar el modal
      },
      (error) => {
        console.error('Error al agregar mantenimiento:', error);
      }
    );
  }
}
