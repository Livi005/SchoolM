import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TechnologicalMeansService, TechnologicalMeans } from 'src/app/service/mediosTecnologico.service';

@Component({
  selector: 'app-medio-tecnologico',
  templateUrl: './medios-tecnologicos.component.html',
  styleUrls: ['./medio-tecnologicos.css'],
})
export class MediosTecnologicosComponent implements OnInit {
  medios: TechnologicalMeans[] = []; // Lista de medios tecnológicos disponibles
  medioForm: FormGroup; // Formulario para la gestión de medios tecnológicos
  isModalVisible = false; // Controla la visibilidad del modal de creación
  isEditModalVisible = false; // Controla la visibilidad del modal de edición
  selectedMedio: TechnologicalMeans | null = null; // Medio tecnológico seleccionado para edición

  constructor(private fb: FormBuilder, private medioService: TechnologicalMeansService) {}

  /**
   * Se ejecuta al iniciar el componente.
   * Inicializa el formulario y carga la lista de medios tecnológicos.
   */
  ngOnInit(): void {
    // Configuración del formulario con validaciones
    this.medioForm = this.fb.group({
      nameMean: ['', Validators.required], // Nombre del medio (obligatorio)
      state: ['', Validators.required], // Estado del medio (obligatorio)
    });

    // Cargar los medios tecnológicos existentes
    this.loadMedios();
  }

  /**
   * Obtiene la lista de medios tecnológicos desde el servicio y los almacena en `medios`.
   */
  loadMedios(): void {
    this.medioService.listTechnologicalMeans().subscribe(
      (response) => {
        this.medios = response;
      },
      (error) => {
        console.error('Error al cargar los medios:', error);
      }
    );
  }

  /**
   * Envía los datos del formulario para crear un nuevo medio tecnológico.
   * Si el formulario es válido, se llama al servicio para guardarlo en la base de datos.
   */
  onSubmit(): void {
    if (this.medioForm.valid) {
      const nuevoMedio: TechnologicalMeans = {
        nameMean: this.medioForm.get('nameMean')?.value,
        state: this.medioForm.get('state')?.value,
      };

      this.medioService.createTechnologicalMeans(nuevoMedio).subscribe(
        (response) => {
          console.log('Medio creado con éxito:', response);
          this.medios.push(response); // Agregar el nuevo medio a la lista local
          this.toggleModal(); // Cerrar el modal de creación
        },
        (error) => {
          console.error('Error al crear el medio:', error);
        }
      );
    } else {
      console.warn('Formulario inválido');
    }
  }

  /**
   * Elimina un medio tecnológico de la lista y de la base de datos.
   * @param medio Medio tecnológico a eliminar
   */
  deleteMedio(medio: TechnologicalMeans): void {
    if (medio.idMean) {
      const confirmacion = confirm(`¿Estás seguro de eliminar el medio "${medio.nameMean}"?`);
      if (confirmacion) {
        this.medioService.deleteTechnologicalMeans(medio.idMean).subscribe(
          () => {
            console.log(`Medio "${medio.nameMean}" eliminado con éxito.`);
            // Filtrar la lista local eliminando el medio eliminado
            this.medios = this.medios.filter((m) => m.idMean !== medio.idMean);
          },
          (error) => {
            console.error('Error al eliminar el medio:', error);
            alert('No se pudo eliminar el medio. Intenta nuevamente.');
          }
        );
      }
    }
  }

  /**
   * Prepara los datos del medio seleccionado para edición y muestra el modal de edición.
   * @param medio Medio tecnológico a editar
   */
  editMedio(medio: TechnologicalMeans): void {
    this.selectedMedio = { ...medio }; // Clonar el objeto seleccionado
    this.isEditModalVisible = true;

    // Llenar el formulario con los datos del medio seleccionado
    this.medioForm.patchValue({
      nameMean: medio.nameMean,
      state: medio.state,
    });
  }

  /**
   * Actualiza un medio tecnológico con los datos modificados.
   * Si el formulario es válido, se envía la actualización al servicio.
   */
  updateMedio(): void {
    if (this.selectedMedio && this.medioForm.valid) {
      // Actualizar los valores del medio seleccionado con los datos del formulario
      this.selectedMedio.nameMean = this.medioForm.get('nameMean')?.value;
      this.selectedMedio.state = this.medioForm.get('state')?.value;

      this.medioService.updateTechnologicalMeans(this.selectedMedio).subscribe(
        (updatedMedio) => {
          console.log('Medio actualizado:', updatedMedio);
          // Buscar y actualizar el medio en la lista local
          const index = this.medios.findIndex((m) => m.idMean === updatedMedio.idMean);
          if (index !== -1) {
            this.medios[index] = updatedMedio;
          }
          this.closeEditModal(); // Cerrar el modal de edición
        },
        (error) => {
          console.error('Error al actualizar el medio:', error);
        }
      );
    }
  }

  /**
   * Alterna la visibilidad del modal de creación de medios tecnológicos.
   * Si se cierra, el formulario se reinicia.
   */
  toggleModal(): void {
    this.isModalVisible = !this.isModalVisible;
    if (!this.isModalVisible) {
      this.medioForm.reset(); // Restablecer formulario al cerrar el modal
    }
  }

  /**
   * Cierra el modal si se hace clic fuera de él.
   * @param event Evento de clic
   */
  closeModal(event: MouseEvent): void {
    const target = event.target as HTMLElement;
    if (target.classList.contains('modal')) {
      this.toggleModal();
    }
  }

  /**
   * Alterna la visibilidad del modal de edición de medios tecnológicos.
   */
  toggleEditModal(): void {
    this.isEditModalVisible = !this.isEditModalVisible;
  }

  /**
   * Cierra el modal de edición si se hace clic fuera de él y restablece el formulario.
   * @param event (Opcional) Evento de clic para detectar si se hizo clic fuera del modal
   */
  closeEditModal(event?: MouseEvent): void {
    if (event) {
      const target = event.target as HTMLElement;
      if (!target.classList.contains('modal')) {
        return;
      }
    }
    this.isEditModalVisible = false;
    this.selectedMedio = null; // Limpiar el medio seleccionado
    this.medioForm.reset(); // Restablecer el formulario
  }
}
