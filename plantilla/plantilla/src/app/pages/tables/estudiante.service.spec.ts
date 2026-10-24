import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { By } from '@angular/platform-browser';
import { TablesComponent } from './estudiante.component';


// Mock Data
const mockStudents = [
  { id: 1, student: { nameStud: 'Juan', age: 15, eActivity: true }, idc: 101 },
  { id: 2, student: { nameStud: 'Ana', age: 16, eActivity: false }, idc: 102 },
];

describe('EstudianteComponent', () => {
  let component: TablesComponent;
  let fixture: ComponentFixture<TablesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TablesComponent],
      imports: [FormsModule],
    }).compileComponents();

    fixture = TestBed.createComponent(TablesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should open the add student modal', () => {
    component.openAddStudentModal();
    expect(component.isAddStudentModalOpen).toBeTrue();
  });

  it('should close the add student modal', () => {
    component.isAddStudentModalOpen = true;
    component.closeAddStudentModal();
    expect(component.isAddStudentModalOpen).toBeFalse();
  });

  it('should add a new student', () => {
    component.newStudent = { nameStud: 'Pedro', age: 17, eActivity: true, idC: 103 };
    component.students = [...mockStudents];
    component.addStudent();

    expect(component.students.length).toBe(3);
    expect(component.students[2].student.nameStud).toBe('Pedro');
  });

  it('should edit an existing student', () => {
    component.students = [...mockStudents];
    component.editStudent(mockStudents[0]);
    expect(component.editingStudent).toEqual(mockStudents[0]);

    component.editingStudent.student.nameStud = 'Updated Name';
    component.saveStudent();

    expect(component.students[0].student.nameStud).toBe('Updated Name');
    expect(component.editingStudent).toBeNull();
  });

  it('should delete a student', () => {
    component.students = [...mockStudents];
    component.deleteStudent(mockStudents[0].id);

    expect(component.students.length).toBe(1);
    expect(component.students[0].id).toBe(2);
  });

  it('should cancel editing a student', () => {
    component.students = [...mockStudents];
    component.editStudent(mockStudents[0]);
    component.cancelEdit();

    expect(component.editingStudent).toBeNull();
  });

  it('should display the list of students', () => {
    component.students = [...mockStudents];
    fixture.detectChanges();

    const studentRows = fixture.debugElement.queryAll(By.css('tbody tr'));
    expect(studentRows.length).toBe(2);

    const firstRowCells = studentRows[0].queryAll(By.css('td'));
    expect(firstRowCells[0].nativeElement.textContent).toContain('Juan');
  });
});
