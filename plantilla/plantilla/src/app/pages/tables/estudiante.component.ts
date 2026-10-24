import { Component, OnInit } from '@angular/core';
import { EstudentService } from '../../service/estudiante.service';

@Component({
  selector: 'app-estudiante',
  templateUrl: './estudiante.component.html',
  styleUrls: ['./estudiante.component.css'],
})
export class TablesComponent implements OnInit {
  students: Array<{  id: number; idc: number; student: { nameStud: string; age: number; eActivity: boolean } }> = [];
  newStudent = { nameStud: '', age: 0, eActivity: false, idC: 0 };
  editingStudent: { id: number; idc: number; student: { nameStud: string; age: number; eActivity: boolean }  } | null = null;
  isAddStudentModalOpen = false;
  constructor(private estudianteService: EstudentService) {}

  ngOnInit() {
    this.loadStudents();
  }

  // Cargar estudiantes
  loadStudents() {
    this.estudianteService.getStudents().subscribe(
      (data) => {
        this.students = data;
console.log(this.students)
      },
      (error) => {
        console.error('Error al cargar estudiantes:', error);
      }
    );
  }

  openAddStudentModal() {
    this.isAddStudentModalOpen = true;
  }

  closeAddStudentModal() {
    this.isAddStudentModalOpen = false;
  }

  addStudent() {
    if (this.newStudent.nameStud && this.newStudent.age > 0 && this.newStudent.idC > 0) {
      this.estudianteService.createStudent(this.newStudent).subscribe(
        (response) => {
          this.loadStudents(); // Recargar la lista
          this.newStudent = { nameStud: '', age: 0, eActivity: false, idC: 0 }; // Limpiar formulario
          this.isAddStudentModalOpen = false; // Cerrar el modal
        },
        (error) => {
          console.error('Error al agregar el estudiante:', error);
        }
      );
    } else {
      alert('Por favor, completa todos los campos correctamente.');
    }
  }

  // Editar estudiante
  editStudent(student: { id: number; idc: number; student: { nameStud: string; age: number; eActivity: boolean }  }) {
    this.editingStudent = { ...student }; // Clonar el objeto para evitar cambios inmediatos
  }

  // Guardar cambios del estudiante
  saveStudent() {
    if (this.editingStudent) {
      // Construir el cuerpo de la petición
      const studentUpdate = {
        id: this.editingStudent.id,
        idc: this.editingStudent.idc, // Incluye el idc
        student: {
          nameStud: this.editingStudent.student.nameStud,
          age: this.editingStudent.student.age,
          eActivity: this.editingStudent.student.eActivity,
        },
      };

      // Llamar al servicio para actualizar el estudiante
      this.estudianteService.updateStudent(studentUpdate).subscribe(
        (response) => {
          alert('Estudiante actualizado con éxito.');
          this.editingStudent = null; // Salir del modo edición
          this.loadStudents(); // Recargar la lista
        },
        (error) => {
          console.error('Error al actualizar estudiante:', error);
        }
      );
    }
  }


  // Cancelar edición
  cancelEdit() {
    this.editingStudent = null; // Salir del modo edición sin guardar
  }

  // Eliminar estudiante
  deleteStudent(studentId: number) {
    if (confirm('¿Estás seguro de que deseas eliminar este estudiante?')) {
      this.estudianteService.deleteStudent(studentId).subscribe(
        (response) => {
          this.loadStudents(); // Recargar la lista
        },
        (error) => {
          console.error('Error al eliminar estudiante:', error);
        }
      );
    }

  }
}
