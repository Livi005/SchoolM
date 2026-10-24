import { Component, OnInit } from '@angular/core';
import { AdministratorService } from 'src/app/service/administrador-info.service';

@Component({
  selector: 'app-info-administrador',
  templateUrl: './info-administrador.component.html',
  styleUrls: ['./info-administrador.component.scss'],
})
export class InfoAdministradorComponent implements OnInit {
  administrators: Array<{ id: number; administrator: { adminName: string; adminSalary: number } }> = [];
  newAdministrator = { adminName: '', adminSalary: 0 };
  editingAdministrator: { id: number; administrator: { adminName: string; adminSalary: number } } | null = null;

  constructor(private administratorService: AdministratorService) {}

  ngOnInit() {
    this.loadAdministrators();
  }

  // Cargar administradores
  loadAdministrators() {
    this.administratorService.getAdministrators().subscribe(
      (data) => {
        this.administrators = data;
      },
      (error) => {
        console.error('Error al cargar los administradores:', error);
      }
    );
  }

  // Agregar administrador
  addAdministrator() {
    if (this.newAdministrator.adminName && this.newAdministrator.adminSalary > 0) {
      this.administratorService.createAdministrator(this.newAdministrator).subscribe(
        (response) => {
          this.loadAdministrators(); // Recargar la lista
          this.newAdministrator = { adminName: '', adminSalary: 0 }; // Limpiar el formulario
        },
        (error) => {
          console.error('Error al crear el administrador:', error);
        }
      );
    } else {
      alert('Por favor, ingrese un nombre y un salario válidos.');
    }
  }

  // Editar administrador
  editAdministrator(administrator: { id: number; administrator: { adminName: string; adminSalary: number } }) {
    this.editingAdministrator = { ...administrator }; // Clonar el objeto para evitar cambios inmediatos
  }

  // Guardar cambios
  saveAdministrator() {
    if (this.editingAdministrator) {
      this.administratorService.updateAdministrator(this.editingAdministrator).subscribe(
        (response) => {
          this.editingAdministrator = null; // Salir del modo edición
          this.loadAdministrators(); // Recargar la lista
        },
        (error) => {
          console.error('Error al actualizar el administrador:', error);
        }
      );
    }
  }

  // Cancelar edición
  cancelEdit() {
    this.editingAdministrator = null; // Salir del modo edición sin guardar
  }

  // Eliminar administrador
  deleteAdministrator(administratorId: number) {
    if (confirm('¿Estás seguro de que deseas eliminar este administrador?')) {
      this.administratorService.deleteAdministrator(administratorId).subscribe(
        (response) => {
          this.loadAdministrators(); // Recargar la lista
        },
        (error) => {
          console.error('Error al eliminar el administrador:', error);
        }
      );
    }
  }
}
