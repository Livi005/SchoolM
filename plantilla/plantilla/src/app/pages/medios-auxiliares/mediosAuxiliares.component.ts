import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuxiliaryMeansService, AuxiliaryMeans } from 'src/app/service/mediosAuxiliares.service';

@Component({
  selector: 'app-medio-auxiliar',
  templateUrl: './mediosAuxiliares.component.html',
  styleUrls: ['./mediosAuxiliares.component.css'],
})
export class MedioAuxiliarComponent implements OnInit {
  medios: AuxiliaryMeans[] = [];
  medioForm: FormGroup;
  isEditModalVisible = false; // Controla la visibilidad del modal de edición
  isModalVisible = false; // Controla la visibilidad del modal
  selectedMedio: AuxiliaryMeans | null = null; // Almacena el medio seleccionado para editar

  constructor(private fb: FormBuilder, private medioService: AuxiliaryMeansService) {}

  ngOnInit(): void {
    this.medioForm = this.fb.group({
      nameMean: ['', Validators.required],
      state: ['', Validators.required],
      type: ['', Validators.required],
    });

    this.loadMedios();
  }

  loadMedios(): void {
    this.medioService.listAuxiliaryMeans().subscribe(
      (response) => {
        this.medios = response;
      },
      (error) => {
        console.error('Error al cargar los medios:', error);
      }
    );
  }

  onSubmit(): void {
    if (this.medioForm.valid) {
      // Mapear las propiedades del formulario a las que espera la API
      const nuevoMedio: AuxiliaryMeans = {
        idMean: undefined, // La API generará el ID automáticamente
        nameMean: this.medioForm.get('nameMean')?.value,
        state: this.medioForm.get('state')?.value,
        type: this.medioForm.get('type')?.value,
      };
      console.log('Datos del formulario (raw values):', this.medioForm.value);
      console.log('Nuevo medio (mapeado):', nuevoMedio);

      this.medioService.createAuxiliaryMeans(nuevoMedio).subscribe(
        (response) => {
          console.log('Medio creado con éxito:', response);
          // Actualiza la lista de medios con la respuesta del servidor
          this.medios.push(response);
          this.toggleModal(); // Cierra el modal tras guardar
        },
        (error) => {
          console.error('Error al crear el medio:', error);
        }
      );
    } else {
      console.warn('El formulario no es válido');
    }
  }

  deleteMedio(medio: AuxiliaryMeans): void {
    if (medio.idMean) {
      if (confirm(`¿Estás seguro de que deseas eliminar el medio "${medio.nameMean}"?`)) {
        this.medioService.deleteAuxiliaryMeans(medio.idMean).subscribe(
          () => {
            console.log(`Medio eliminado con éxito: ${medio.nameMean}`);
            // Actualiza la lista de medios excluyendo el medio eliminado
            this.medios = this.medios.filter((m) => m.idMean !== medio.idMean);
          },
          (error) => {
            console.error('Error al eliminar el medio:', error);
            alert('No se pudo eliminar el medio. Por favor, inténtalo de nuevo.');
          }
        );
      }
    } else {
      console.warn('El medio no tiene un ID válido y no se puede eliminar:', medio);
    }
  }

  toggleModal(): void {
    this.isModalVisible = !this.isModalVisible;
    if (!this.isModalVisible) {
      this.medioForm.reset(); // Resetea el formulario al cerrar el modal
    }
  }

  closeModal(event: MouseEvent): void {
    const target = event.target as HTMLElement;
    if (target.classList.contains('modal')) {
      this.toggleModal(); // Cierra el modal si se hace clic fuera del contenido
    }
  }
  openEditModal(medio: AuxiliaryMeans): void {
    this.selectedMedio = medio;
    this.isEditModalVisible = true;

    // Precarga los datos del medio seleccionado en el formulario
    this.medioForm.patchValue({
      nameMean: medio.nameMean,
      state: medio.state,
      type: medio.type,
    });
  }

  toggleEditModal(): void {
    this.isEditModalVisible = !this.isEditModalVisible;
    if (!this.isEditModalVisible) {
      this.medioForm.reset();
    }
  }

  closeEditModal(event: MouseEvent): void {
    const target = event.target as HTMLElement;
    if (target.classList.contains('modal')) {
      this.toggleEditModal();
    }
  }

  updateMedio(): void {
    if (this.selectedMedio && this.medioForm.valid) {
      // Actualiza los datos del medio seleccionado
      this.selectedMedio.nameMean = this.medioForm.get('nameMean')?.value;
      this.selectedMedio.state = this.medioForm.get('state')?.value;
      this.selectedMedio.type = this.medioForm.get('type')?.value;

      this.medioService.updateAuxiliaryMeans(this.selectedMedio).subscribe(
        (updatedMedio) => {
          console.log('Medio actualizado:', updatedMedio);
          // Actualiza la lista local de medios con los cambios
          const index = this.medios.findIndex((m) => m.idMean === updatedMedio.idMean);
          if (index !== -1) {
            this.medios[index] = updatedMedio;
          }
          this.toggleEditModal(); // Cierra el modal tras guardar
        },
        (error) => {
          console.error('Error al actualizar el medio:', error);
          alert('No se pudo actualizar el medio. Por favor, inténtalo de nuevo.');
        }
      );
    } else {
      console.warn('El formulario no es válido o no hay un medio seleccionado.');
    }
  }

}
