import { Component, OnInit } from '@angular/core';
import { ProfesorService } from 'src/app/service/profesor-info.service';

@Component({
  selector: 'app-profesor-info',
  templateUrl: './profesor-info.component.html',
  styleUrls: ['./profesor-info.component.css']
})
export class ProfesorComponent implements OnInit {
  profesores: any[] = [];
  globalSearchQuery: string = '';
  showModal: boolean = false;
  newProfesor: any = { nameProf: '', contract: '', spec: '', salary: 0, laboralExperience: 0 };

  constructor(private profesorService: ProfesorService) {}

  ngOnInit(): void {
    this.loadProfesores();
  }

  loadProfesores(): void {
    this.profesorService.listProfesores().subscribe(
      (data) => {
        this.profesores = data.map((profesor: any) => ({
          ...profesor,
          isEditing: false,
          spec: profesor.professor.spec
        }));
      },
      (error) => {
        console.error('Error al listar profesores:', error);
      }
    );
  }

  openModal(): void {
    this.showModal = true;
    this.newProfesor = { nameProf: '', contract: '', spec: '', salary: 0, laboralExperience: 0 };
  }

  closeModal(): void {
    this.showModal = false;
  }

  saveProfesor(): void {
    this.profesorService.createProfesor(this.newProfesor).subscribe(
      (response) => {
        console.log('Profesor agregado:', response);
        this.loadProfesores();
        this.closeModal();
      },
      (error) => {
        console.error('Error al agregar profesor:', error);
      }
    );
  }

  toggleEdit(profesor: any): void {
    profesor.isEditing = !profesor.isEditing;
  }

  saveChanges(profesor: any): void {
    if (!profesor.id) {
      this.profesorService.createProfesor(profesor.professor).subscribe(
        (response) => {
          console.log('Profesor agregado:', response);
          this.loadProfesores();
        },
        (error) => {
          console.error('Error al agregar profesor:', error);
        }
      );
    } else {
      const updatedProfesor = {
        id: profesor.id,
        professor: {
          nameProf: profesor.professor.nameProf,
          contract: profesor.professor.contract,
          spec: profesor.professor.spec,
          salary: profesor.professor.salary,
          laboralExperience: profesor.professor.laboralExperience
        }
      };

      this.profesorService.updateProfesor(updatedProfesor).subscribe(
        (response) => {
          console.log('Profesor actualizado:', response);
          profesor.isEditing = false;
        },
        (error) => {
          console.error('Error al actualizar profesor:', error);
        }
      );
    }
  }

  deleteProfesor(id: number): void {
    this.profesorService.deleteProfesor(id).subscribe(
      (response) => {
        console.log('Profesor eliminado:', response);
        this.loadProfesores();
      },
      (error) => {
        console.error('Error al eliminar profesor:', error);
      }
    );
  }

  updateDisplayProfesor(): void {
    this.profesores = this.profesores.filter(profesor =>
      profesor.professor.nameProf.toLowerCase().includes(this.globalSearchQuery.toLowerCase()) ||
      profesor.professor.spec.toLowerCase().includes(this.globalSearchQuery.toLowerCase())
    );
  }
}
