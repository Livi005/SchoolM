import { Component, OnInit } from '@angular/core';
import { StudentGradingService } from 'src/app/service/generaStud.service';
import { SecretaryService } from 'src/app/service/secretaria-info.service';
@Component({
  selector: 'app-info-estudiantes',
  templateUrl: './info-estudiantes.component.html',
  styleUrls: ['./info-estudiantes.component.css']
})
export class InfoEstudiantesComponent implements OnInit {
  subjects: any[] = [];
  students: any[] = [];
  results: any[] = [];
  role : string ='';
  displayedStudents = [];
  globalSearchQuery = '';
  selectedSubject = '';
  editMode: boolean[] = [];
  viewMode: string = 'assign'; // Alterna entre 'assign' y 'list'
  notes: any[] = []; // Almacena las notas obtenidas de la API
  constructor(private gradingService: StudentGradingService , private secretaria :SecretaryService) {}
 
  ngOnInit(): void {
    this.role = this.gradingService.getRole();
    this.getSubjects(); // Replace 'currentUser' with the actual username logic
    
  }
  getStudents(subjectId: string): void {
    console.log(subjectId);
    this.gradingService.getStudents(+subjectId).subscribe(
      (response: any) => {
        console.log('Respuesta del servidor para estudiantes:', response);
  
        // Manejo flexible de la respuesta
        if (Array.isArray(response)) {
          this.students = response;
        } else if (response.students && Array.isArray(response.students)) {
          this.students = response.students;
        } else {
          console.error('La respuesta no contiene un array válido:', response);
          this.students = [];
        }
  
        this.updateDisplayedStudents();
      },
      (error) => console.error('Error fetching students:', error)
    );
  }
  updateStudentNote(note: any, index: number): void {
    const profStudSubId: number = note.idProfStudSub;
    const studentGrade: number = note.studentGrades;
  
    this.secretaria.updateStudentNote(profStudSubId, studentGrade).subscribe(
      () => {
        console.log('Nota actualizada:', { profStudSubId, studentGrade });
        this.editMode[index] = false; // Salir del modo edición
        alert('Nota actualizada correctamente');
      },
      (error) => {
        console.error('Error al actualizar la nota:', error);
        alert('Error al actualizar la nota');
      }
    );
  }
  
  updateDisplayedStudents(): void {
    if (!Array.isArray(this.students)) {
      console.error('this.students no es un array:', this.students);
      return;
    }
  
    this.displayedStudents = this.students.filter((student) => {
      const matchesSearch =
        !this.globalSearchQuery ||
        student.nameStud.toLowerCase().includes(this.globalSearchQuery.toLowerCase());
      return matchesSearch;
    });
  }
  assignGrade(student: any , index: number): void {
    const payload = {
      userName: this.role, // Reemplazar con la lógica de usuario actual
      idSub: this.selectedSubject,
      idStud: student.idStud,
      studentGrades: student.grade
    };

    this.gradingService.assignGrade(payload).subscribe(
      () => {
        console.log('Nota asignada correctamente:', payload);
  
        // Actualiza la tabla después de guardar
        this.displayedStudents[index].grade = student.grade;
  
        // Desactiva el modo de edición
        this.editMode[index] = false;
  
      },
      (error) => {
        console.error('Error asignando la nota:', error);
        alert('Ocurrió un error al asignar la nota');
      }
    );
  }
  getSubjects() {
    if (!this.role) {
      console.error('Role is undefined or invalid');
      return;
    }
  
    if (this.role === 'Secretary') {
      this.secretaria.getsubject().subscribe(
        (response: any) => {
          if (Array.isArray(response)) {
            this.subjects = response; // Asigna directamente el array
            if (this.subjects.length > 0) {
              this.selectedSubject = this.subjects[0].idSub; // Usa la propiedad idSub
              this.getStudents(this.selectedSubject); // Pasa el idSub de la primera asignatura
            } else {
              this.selectedSubject = ''; // Deja en blanco si no hay asignaturas
            }
          } else {
            console.error('Response is not an array:', response);
            this.subjects = [];
            this.selectedSubject = '';
          }
        },
        (error) => console.error('Error fetching subjects:', error)
      );
    } else {
      this.gradingService.getSubjects(this.role).subscribe(
        (response: any) => {
          if (response && response.subjects) {
            this.subjects = response.subjects || [];
            if (this.subjects.length > 0) {
              this.selectedSubject = this.subjects[0].idSub;
              this.getStudents(this.selectedSubject);
            } else {
              this.selectedSubject = '';
            }
          } else {
            console.error('Response is invalid or does not contain subjects:', response);
            this.subjects = [];
            this.selectedSubject = '';
          }
        },
        (error) => console.error('Error fetching subjects:', error)
      );
    }
  }
  
  onSubjectChange(subjectId: string): void {
    this.selectedSubject = subjectId;
    this.getStudents(subjectId);
  }

  toggleEditMode(index: number): void {
    this.editMode[index] = !this.editMode[index];
    if (!this.editMode[index]) {
      console.log('Guardando datos:', this.displayedStudents[index]);
    }
    if (this.role === 'secretaria') {
      this.editMode[index] = !this.editMode[index];
    }
  }
  toggleViewMode(): void {
    this.viewMode = this.viewMode === 'assign' ? 'list' : 'assign';
    if (this.viewMode === 'list') {
      this.getNotes();
    }
  }

  getNotes(): void {
    
    if (this.role === 'Secretary')
    {
      console.log('SECRETARIAAA')
      this.secretaria.getSecretNotas().subscribe(
        (notes: any[]) => {
          this.notes = notes;
        },
        (error) => console.error('Error fetching notes:', error)
      );
    }
    else
    {
    this.gradingService.getNotes(this.role).subscribe(
      (notes: any[]) => {
        this.notes = notes;
      },
      (error) => console.error('Error fetching notes:', error)
    );
  }
}
}
