import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Solicitud } from 'src/app/service/asignatura-profesor.service';

@Component({
  selector: 'app-solicitud',
  templateUrl: './asignatura-profesor.component.html',
  styleUrls: ['./asignatura-profesor.component.css'],
})
export class AsignaturaPComponent implements OnInit {
  solicitudForm: FormGroup;
  subjects: any[] = []; // Lista de asignaturas disponibles
  professors: any[] = []; // Lista de profesores disponibles
  selectedSubject: any = null; // Asignatura seleccionada
  professorSubjectAssignments: any[] = []; // Lista de asignaciones de profesores a asignaturas
  showAssignedSubjects = false; // Controla la vista de asignaciones
  filteredSubjects: any[] = []; // Lista filtrada de asignaturas
  searchTerm: string = ''; // Término de búsqueda para filtrar asignaturas

  constructor(private fb: FormBuilder, private http: HttpClient, private solicitud: Solicitud) {
    this.solicitudForm = this.fb.group({
      subject: [''],
    });
  }

  /**
   * Se ejecuta al iniciar el componente.
   * Carga las asignaturas disponibles.
   */
  ngOnInit(): void {
    this.cargarAsignaturas();
  }

  /**
   * Obtiene la lista de asignaturas desde el servicio y las almacena en `subjects` y `filteredSubjects`.
   */
  cargarAsignaturas() {
    this.solicitud.getSubjects().subscribe(
      (response) => {
        this.subjects = response || [];
        this.filteredSubjects = this.subjects; // Inicializa la lista filtrada con todas las asignaturas
      },
      (error) => {
        console.error('Error al cargar asignaturas:', error);
      }
    );
  }

  /**
   * Filtra la lista de asignaturas en función del término de búsqueda ingresado por el usuario.
   */
  filterSubjects() {
    this.filteredSubjects = this.subjects.filter(subject =>
      subject.nameSub.toLowerCase().includes(this.searchTerm.toLowerCase())
    );
  }

  /**
   * Selecciona una asignatura y obtiene los profesores disponibles para asignarla.
   * @param subjectId ID de la asignatura seleccionada
   */
  seleccionarAsignatura(subjectId: string) {
    if (!subjectId) {
      this.selectedSubject = null;
      this.professors = [];
      return;
    }

    const subjectIdNumber = Number(subjectId);
    this.selectedSubject = this.subjects.find(subject => subject.idSub === subjectIdNumber);

    // Obtiene la lista de profesores disponibles para la asignatura seleccionada
    this.solicitud.getProfessorsForSubject(subjectIdNumber).subscribe(
      (response) => {
        // Filtra los profesores ya asignados para que no aparezcan en la lista de selección
        this.professors = response.filter(professor =>
          !this.professorSubjectAssignments.some(assignment =>
            assignment.professorId === professor.id && assignment.subjectId === subjectIdNumber
          ));
      },
      (error) => {
        console.error('Error al obtener profesores:', error);
      }
    );
  }

  /**
   * Asigna un profesor a una asignatura.
   * @param professorId ID del profesor a asignar
   */
  asignarProfesor(professorId: number) {
    if (!this.selectedSubject) {
      alert('Debe seleccionar una asignatura antes de asignar un profesor');
      return;
    }

    const data = {
      idProf: professorId,
      idSub: this.selectedSubject.idSub,
    };

    this.solicitud.assignProfessorToSubject(data).subscribe(
      (response) => {
        console.log('Profesor asignado exitosamente', response);

        // Agrega la nueva asignación a la lista de asignaciones locales
        this.professorSubjectAssignments.push({
          professorId: professorId,
          subjectId: this.selectedSubject.idSub,
          professorName: response.professorName,
          subjectName: this.selectedSubject.nameSub
        });

        // Recargar la lista de profesores disponibles
        this.seleccionarAsignatura(this.selectedSubject.idSub);
      },
      (error) => {
        console.error('Error al asignar el profesor:', error);
      }
    );
  }

  /**
   * Carga todas las asignaciones de profesores a asignaturas.
   */
  cargarAsignaciones() {
    this.solicitud.getProfessorSubjectAssignments().subscribe(
      (response) => {
        this.professorSubjectAssignments = response || [];
      },
      (error) => {
        console.error('Error al cargar asignaciones:', error);
      }
    );
  }

  /**
   * Elimina una asignación de profesor a una asignatura.
   * @param idProfSub ID de la asignación a eliminar
   */
  eliminarAsignacion(idProfSub: number) {
    this.solicitud.deleteProfessorSubjectAssignment(idProfSub).subscribe(
      (response) => {
        console.log('Asignación eliminada exitosamente', response);

        // Elimina la asignación de la lista local
        this.professorSubjectAssignments = this.professorSubjectAssignments.filter(
          (assignment) => assignment.idProfSub !== idProfSub
        );

        // Recargar la lista de profesores disponibles
        this.seleccionarAsignatura(this.selectedSubject.idSub);
      },
      (error) => {
        console.error('Error al eliminar asignación:', error);
      }
    );
  }

  /**
   * Alterna la visualización de la lista de asignaciones.
   * Si se activa, se cargan las asignaciones existentes.
   */
  toggleAssignedSubjects() {
    this.showAssignedSubjects = !this.showAssignedSubjects;
    this.selectedSubject = null;
    if (this.showAssignedSubjects) {
      this.cargarAsignaciones();
    }
  }
}
