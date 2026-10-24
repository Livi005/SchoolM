import { Component, OnInit } from '@angular/core';
import { ClassroomService } from 'src/app/service/aula.component.service';

interface Classroom {
  idClassR: number;
  location: string;
}

@Component({
  selector: 'app-classroom',
  templateUrl: './aulas.component.html',
  styleUrls: ['./aulas.component.scss']
})
export class ClassroomComponent implements OnInit {
  classrooms: Classroom[] = [];
  newClassroom: Classroom = { idClassR: 0, location: '' };
  editingClassroom: Classroom | null = null;
  isAddClassroomModalOpen = false;

  constructor(private classroomService: ClassroomService) {}

  ngOnInit() {
    this.loadClassrooms();
  }

  loadClassrooms() {
    this.classroomService.getClassrooms().subscribe((data) => {
      this.classrooms = data;
    });
  }

  openAddClassroomModal() {
    this.isAddClassroomModalOpen = true;
  }

  closeAddClassroomModal() {
    this.isAddClassroomModalOpen = false;
  }

  addClassroom() {
    this.classroomService.createClassroom(this.newClassroom).subscribe(() => {
      this.loadClassrooms();
      this.closeAddClassroomModal();
      this.newClassroom = { idClassR: 0, location: '' };
    });
  }

  editClassroom(classroom: Classroom) {
    this.editingClassroom = { ...classroom };
  }

  cancelEdit() {
    this.editingClassroom = null;
  }

  saveClassroom() {
    if (this.editingClassroom) {
      this.classroomService.updateClassroom(this.editingClassroom).subscribe(() => {
        this.loadClassrooms();
        this.editingClassroom = null;
      });
    }
  }

  deleteClassroom(idClassR: number) {
    this.classroomService.deleteClassroom(idClassR).subscribe(() => {
      this.loadClassrooms();
    });
  }
}
