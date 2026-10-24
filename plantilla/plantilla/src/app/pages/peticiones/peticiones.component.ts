import { Component, OnInit } from '@angular/core';
import { ProfessorService } from 'src/app/service/peticiones .service';
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';

@Component({
  selector: 'app-peticiones',
  templateUrl: './peticiones.component.html',
  styleUrls: ['./peticiones.component.css']
})
export class PeticionesComponent implements OnInit {
  specs: string[] = []; // Almacena las especializaciones disponibles
  selectedSpec: string = ''; // Especialización seleccionada
  professors: any[] = []; // Lista de profesores según la especialización seleccionada
  selectedFunctionality = 'especializacionProfesores'; // Funcionalidad seleccionada en la UI

  maintenances: any = {}; // Almacena los datos de mantenimientos realizados
  totalMaintenances: number = 0; // Contador del total de mantenimientos
  goodProfesor: any[] = []; // Lista de profesores con buenas calificaciones
  badStudents: any[] = []; // Lista de estudiantes con bajo rendimiento
  punishedProfessors: any[] = []; // Lista de profesores sancionados
  maintenanceCosts: any[] = []; // Costos de mantenimiento por aula

  // Lista de funcionalidades disponibles en la UI
  functionalities = [
    { key: 'especializacionProfesores', label: 'Gestión de Profesores' },
    { key: 'promedioMantenimiento', label: 'Promedio de Mantenimiento' },
    { key: 'mantenimientosRealizados', label: 'Mantenimientos Realizados' },
    { key: 'ProfesorBuenaNota', label: 'Profesores Destacados' },
    { key: 'BadStudents', label: 'Estudiantes con Bajo Rendimiento' },
    { key: 'BadProfessor', label: 'Profesores Sancionados' }
  ];

  constructor(private professorService: ProfessorService) {}

  ngOnInit(): void {
    this.loadSpecs(); // Carga la lista de especializaciones al iniciar el componente
  }

  // Carga la lista de especializaciones desde el servicio
  loadSpecs(): void {
    this.professorService.getSpecs().subscribe((response) => {
      this.specs = response.spec;
    });
  }

  // Carga los profesores según la especialización seleccionada
  onSpecChange(): void {
    if (this.selectedSpec) {
      this.professorService.getProfessorsBySpec(this.selectedSpec).subscribe((response) => {
        this.professors = response;
      });
    }
  }

  // Maneja el cambio de funcionalidad seleccionada y carga los datos necesarios
  onFunctionalityChange(): void {
    if (this.selectedFunctionality === 'mantenimientosRealizados') {
      this.loadMaintenances();
    } else if (this.selectedFunctionality === 'ProfesorBuenaNota') {
      this.loadGoodProfesor();
    } else if (this.selectedFunctionality === 'BadProfessor') {
      this.loadBadProfesor();
    } else if (this.selectedFunctionality === 'BadStudents') {
      this.loadBadStudents();
    } else if (this.selectedFunctionality === 'promedioMantenimiento') {
      this.loadMaintenanceCosts();
    }
  }

  // Carga la información de mantenimientos realizados
  loadMaintenances(): void {
    this.professorService.getMaintenances().subscribe((response) => {
      this.maintenances = response.classRoomsAndMeans;
      this.totalMaintenances = response.ammountOfMaintenance2yo;
    });
  }

  // Carga la lista de profesores con buenas calificaciones
  loadGoodProfesor(): void {
    this.professorService.getGoodProfessors().subscribe(response => {
      this.goodProfesor = Object.entries(response.professorsAndSubjects).map(
        ([name, subjects]) => ({ name, subjects })
      );
    });
  }

  // Retorna las claves de un objeto (para iterar sobre ellas en la vista)
  objectKeys(obj: any): string[] {
    return Object.keys(obj);
  }

  // Carga la lista de estudiantes con bajo rendimiento
  loadBadStudents(): void {
    this.professorService.getBadStudents().subscribe(response => {
      this.badStudents = response;
    });
  }

  // Carga la lista de profesores sancionados
  loadBadProfesor(): void {
    this.professorService.getBadProfesor().subscribe(response => {
      this.punishedProfessors = response.map(professor => ({
        nameProf: professor.nameProf,
        punishmentDate: professor.punishmentDate,
        UseAuxMean: professor.UseAuxMean,
        evals: professor.evals.length > 0 ? professor.evals : [0, 0, 0] // Manejo de evaluaciones vacías
      }));
    });
  }

  // Carga los costos de mantenimiento por aula
  loadMaintenanceCosts(): void {
    this.professorService.getMaintenanceCosts().subscribe(response => {
      console.log("Datos de mantenimiento recibidos:", response);
      this.maintenanceCosts = Object.entries(response.classRoomAverageCost).map(([idClassR, amount]) => ({
        idClassR, // ID del aula
        amount    // Costo de mantenimiento
      }));
    });
  }

  // Exporta los datos a un archivo PDF según la funcionalidad seleccionada
  exportToPdf(selectedFunctionality: string) {
    const doc = new jsPDF();
    let title = '';
    let columns: string[] = [];
    let data: any[] = [];

    // Configura el título, columnas y datos según la funcionalidad seleccionada
    switch (selectedFunctionality) {
      case 'especializacionProfesores':
        title = 'Gestión de Profesores por Especialización';
        columns = ['Profesor', 'Especialización', 'Aulas y Medios'];
        data = this.professors.map(prof => [
          prof.nameProf,
          prof.spec,
          JSON.stringify(prof.classRoomsAndMeans) // Convertir a string para simplificar
        ]);
        break;

      case 'promedioMantenimiento':
        title = 'Promedio de Mantenimiento';
        columns = ['Aula', 'Costo de Mantenimiento'];
        data = this.maintenanceCosts.map(cost => [cost.idClassR, cost.amount]);
        break;

      case 'mantenimientosRealizados':
        title = 'Mantenimientos Realizados';
        columns = ['Aula', 'Medios Auxiliares', 'Medios Tecnológicos', 'Total'];
        data = Object.keys(this.maintenances).map(key => [
          key,
          this.maintenances[key].AuxiliaryMean,
          this.maintenances[key].TechnologicalMean,
          this.maintenances[key].AuxiliaryMean + this.maintenances[key].TechnologicalMean
        ]);
        break;

      case 'ProfesorBuenaNota':
        title = 'Profesores Destacados';
        columns = ['Profesor', 'Materias'];
        data = this.goodProfesor.map(prof => [prof.name, prof.subjects.join(', ')]);
        break;

      case 'BadStudents':
        title = 'Estudiantes con Bajo Rendimiento';
        columns = ['Curso', 'Estudiante', 'Evaluación Promedio'];
        data = this.badStudents.map(student => [student.curso, student.studentName, student.professorsAvarageEvaluation]);
        break;

      case 'BadProfessor':
        title = 'Profesores Sancionados';
        columns = ['Profesor', 'Fecha de Sanción', 'Usa Medios Auxiliares', 'Peores Evaluaciones'];
        data = this.punishedProfessors.map(prof => [
          prof.nameProf,
          prof.punishmentDate,
          prof.UseAuxMean ? 'Sí' : 'No',
          prof.evals.join(', ')
        ]);
        break;

      default:
        alert('Seleccione una funcionalidad válida');
        return;
    }

    // Agregar título al PDF
    doc.text(title, 14, 20);

    // Generar tabla con los datos
    autoTable(doc, {
      head: [columns],
      body: data,
      startY: 30
    });

    // Guardar el documento PDF con el título correspondiente
    doc.save(`${title}.pdf`);
  }
}
