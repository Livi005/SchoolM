import { Component, OnInit } from '@angular/core';
import { SecretaryService } from 'src/app/service/secretaria-info.service';
@Component({
  selector: 'app-info-secretaria',
  templateUrl: './info-secretaria.component.html',
  styleUrls: ['./info-secretaria.component.scss']
})
export class InfoSecretariaComponent implements OnInit {
 // Lista de secretarias
 secretaries: Array<{ id: number; secretary: { nameS: string; salaryS: number } }> = [];
  
 // Nuevo objeto para la secretaria
 newSecretary = { nameS: '', salaryS: 0 };
 editingSecretary: { id: number; secretary: { nameS: string; salaryS: number } } | null = null;
 constructor(private secretaryService: SecretaryService) {}

 ngOnInit() {
   this.loadSecretaries();
 }

 // Método para cargar la lista de secretarias
 loadSecretaries() {
   this.secretaryService.getSecretaries().subscribe(
     (data) => {
       this.secretaries = data;
     },
     (error) => {
       console.error('Error al cargar las secretarias:', error);
     }
   );
 }

 // Método para agregar una nueva secretaria
 addSecretary() {
   if (this.newSecretary.nameS && this.newSecretary.salaryS > 0) {
     this.secretaryService.createSecretary(this.newSecretary).subscribe(
       (response) => {
         // Agregar la nueva secretaria a la lista (opcional)
         this.loadSecretaries(); // Actualizar la lista desde el servidor
         this.newSecretary = { nameS: '', salaryS: 0 }; // Limpiar formulario
       },
       (error) => {
         console.error('Error al crear la secretaria:', error);
       }
     );
   } else {
     alert('Por favor, ingrese un nombre y un salario válidos.');
   }
 }
// Eliminar secretaria
deleteSecretary(secretaryId: number) {
  if (confirm('¿Estás seguro de que deseas eliminar esta secretaria?')) {
    this.secretaryService.deleteSecretary(secretaryId).subscribe(
      (response) => {
       
        this.loadSecretaries(); // Recargar la lista
      },
      (error) => {
        console.error('Error al eliminar la secretaria:', error);
      }
    );
  }
}

// Entrar en modo edición
editSecretary(secretary: { id: number; secretary: { nameS: string; salaryS: number } }) {
  this.editingSecretary = { ...secretary }; // Clonar el objeto para evitar cambios inmediatos
}

// Guardar cambios
saveSecretary() {
  if (this.editingSecretary) {
    this.secretaryService.updateSecretary(this.editingSecretary).subscribe(
      (response) => {
        alert('Secretaria actualizada con éxito.');
        this.editingSecretary = null; // Salir del modo edición
        this.loadSecretaries(); // Recargar la lista
      },
      (error) => {
        console.error('Error al actualizar la secretaria:', error);
      }
    );
  }
}

// Cancelar la edición
cancelEdit() {
  this.editingSecretary = null; // Salir del modo edición sin guardar
}
}