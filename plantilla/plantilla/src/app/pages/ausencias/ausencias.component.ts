import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { StudentSubjectService, StudentSubject } from 'src/app/service/ausencias.service';

@Component({
  selector: 'app-student-subject',
  templateUrl: './ausencias.component.html',
  styleUrls: ['./ausencias.component.css'],
})
export class AusenciasComponent implements OnInit {
  studentSubjects: StudentSubject[] = [];
  studentSubjectForm: FormGroup;
  isModalVisible = false;

  constructor(
    private fb: FormBuilder,
    private studentSubjectService: StudentSubjectService
  ) {}

  ngOnInit(): void {
    // Inicialización del formulario
    this.studentSubjectForm = this.fb.group({
      idStud: [0, Validators.required],
      idSub: [0, Validators.required],
      njAbsents: [0, [Validators.required, Validators.min(0)]],
    });

    // Cargar los studentSubjects al iniciar el componente
    this.loadStudentSubjects();
  }

  loadStudentSubjects(): void {
    this.studentSubjectService.listStudentSubjects().subscribe(
      (response) => {
        this.studentSubjects = response;
console.log(response)
      },
      (error) => {
        console.error('Error al cargar los studentSubjects:', error);
      }
    );
  }

  onSubmit(): void {
    if (this.studentSubjectForm.valid) {
      const newStudentSubject = {
        idStud: this.studentSubjectForm.get('idStud')?.value,
        idSub: this.studentSubjectForm.get('idSub')?.value,
        njAbsents: this.studentSubjectForm.get('njAbsents')?.value,
      };

      this.studentSubjectService.createStudentSubject(newStudentSubject).subscribe(
        (response) => {
          console.log('StudentSubject creado con éxito:', response);
          this.studentSubjects.push(response); // Añadir el nuevo studentSubject a la lista
          this.toggleModal(); // Cierra el modal
        },
        (error) => {
          console.error('Error al crear el StudentSubject:', error);
        }
      );
    } else {
      console.warn('Formulario inválido');
    }
  }

  toggleModal(): void {
    this.isModalVisible = !this.isModalVisible;
    if (!this.isModalVisible) {
      this.studentSubjectForm.reset(); // Resetea el formulario al cerrar el modal
    }
  }

  closeModal(event: MouseEvent): void {
    const target = event.target as HTMLElement;
    if (target.classList.contains('modal')) {
      this.toggleModal();
    }
  }
}
