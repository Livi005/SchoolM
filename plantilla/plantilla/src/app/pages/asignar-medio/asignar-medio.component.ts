import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { SolicitudService } from 'src/app/service/asignar-medio.service';

@Component({
  selector: 'app-solicitud',
  templateUrl: './asignar-medio.component.html',
  styleUrls: ['./asignar-medio.component.css'],
})
export class AsignarComponent implements OnInit {
  solicitudForm: FormGroup; // Formulario reactivo para la solicitud
  aulas: any[] = []; // Lista de aulas disponibles
  mediosDisponibles: any[] = []; // Lista de medios tecnológicos disponibles
  selectedClassroom: any = null; // Aula seleccionada por el usuario
  selectedTechnologicalMeans: any[] = []; // Lista de medios seleccionados
  assignedTechMeans: any[] = []; // Lista de medios ya asignados
  showAssignedMeans = false; // Indica si se muestran los medios asignados

  constructor(private fb: FormBuilder, private http: HttpClient, private solicitud: SolicitudService) {
    // Se inicializa el formulario con los campos necesarios
    this.solicitudForm = this.fb.group({
      aula: [''], // Campo para el aula seleccionada
      medios: this.fb.array([]), // Lista de medios tecnológicos
    });
  }

  ngOnInit(): void {
    this.cargarAulas(); // Al iniciar el componente, se cargan las aulas disponibles
  }

  /**
   * Obtiene la lista de aulas disponibles desde el servicio y las almacena en el array 'aulas'.
   */
  cargarAulas() {
    this.solicitud.getAvailableClassrooms().subscribe(
      (response) => {
        this.aulas = response || []; // Si la respuesta es null o undefined, se asigna un array vacío
      },
      (error) => {
        console.error('Error al cargar aulas:', error);
      }
    );
  }

  /**
   * Selecciona un aula y carga los medios tecnológicos disponibles para esa aula.
   * @param aulaId - ID del aula seleccionada.
   */
  seleccionarAula(aulaId: string) {
    if (!aulaId) {
      // Si no se selecciona un aula, se limpian las variables relacionadas
      this.selectedClassroom = null;
      this.mediosDisponibles = [];
      return;
    }

    const aulaIdNumber = Number(aulaId);
    this.selectedClassroom = this.aulas.find(aula => aula.idClassR === aulaIdNumber);

    // Obtiene los medios disponibles para el aula seleccionada
    this.solicitud.getTechnologicalMeansForClassroom(aulaIdNumber).subscribe(
      (response) => {
        const todosLosMedios = response || [];

        // Obtiene la lista de medios ya asignados
        this.solicitud.getAssignedTechMeans().subscribe(
          (assignedResponse) => {
            this.assignedTechMeans = assignedResponse || [];

            // Filtra los medios para mostrar solo los que no han sido asignados aún a esta aula
            this.mediosDisponibles = todosLosMedios.filter(medio =>
              !this.assignedTechMeans.some(asignado =>
                asignado.idTechMean === medio.idMean && asignado.idClassRoom === aulaIdNumber
              )
            );
          },
          (error) => {
            console.error('Error al cargar medios asignados:', error);
          }
        );
      },
      (error) => {
        console.error('Error al obtener medios tecnológicos:', error);
      }
    );
  }

  /**
   * Asigna un medio tecnológico a un aula seleccionada.
   * @param medioId - ID del medio a asignar.
   */
  asignarMedio(medioId: number) {
    if (!this.selectedClassroom) {
      alert('Debe seleccionar un aula antes de asignar un medio');
      return;
    }

    const data = {
      idClassRoom: this.selectedClassroom.idClassR, // ID del aula seleccionada
      idTechMean: medioId, // ID del medio a asignar
    };

    this.solicitud.assignTechnologicalMeanToClassroom(data).subscribe(
      (response) => {
        console.log('Medio asignado exitosamente', response);
        // Se elimina el medio de la lista disponible tras la asignación
        this.mediosDisponibles = this.mediosDisponibles.filter(m => m.idMean !== medioId);
      },
      (error) => {
        console.error('Error al asignar el medio:', error);
      }
    );
  }

  /**
   * Carga la lista de medios tecnológicos que ya han sido asignados a aulas.
   */
  cargarMediosAsignados() {
    this.solicitud.getAssignedTechMeans().subscribe(
      (response) => {
        this.assignedTechMeans = response || []; // Almacena los medios asignados en la variable correspondiente
      },
      (error) => {
        console.error('Error al cargar medios asignados:', error);
      }
    );
  }

  /**
   * Elimina la asignación de un medio tecnológico en un aula específica.
   * @param idClassRoomTech - ID de la asignación a eliminar.
   */
  eliminarAsignacion(idClassRoomTech: number) {
    this.solicitud.deleteAssignedTechMean(idClassRoomTech).subscribe(
      (response) => {
        console.log('Asignación eliminada exitosamente', response);
        // Se actualiza la lista eliminando el medio que fue desasignado
        this.assignedTechMeans = this.assignedTechMeans.filter(m => m.idClassRoomTech !== idClassRoomTech);
      },
      (error) => {
        console.error('Error al eliminar asignación:', error);
      }
    );
  }

  /**
   * Alterna la visualización entre la lista de aulas y la lista de medios asignados.
   * Si se activan los medios asignados, se cargan los datos desde el servicio.
   */
  toggleAssignedMeans() {
    this.showAssignedMeans = !this.showAssignedMeans; // Alterna entre mostrar u ocultar medios asignados
    this.selectedClassroom = null; // Limpia la selección actual de aula
    if (this.showAssignedMeans) {
      this.cargarMediosAsignados(); // Si se activó la vista, carga los medios asignados
    }
  }

  /**
   * Vuelve a la vista de selección de aulas, limpiando las selecciones actuales.
   */
  volverAulas() {
    this.selectedClassroom = null; // Restablece la selección de aula
    this.showAssignedMeans = false; // Oculta la lista de medios asignados
    this.mediosDisponibles = []; // Limpia la lista de medios disponibles
  }
}
