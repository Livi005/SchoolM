
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { InfoEstudiantesComponent } from './info-estudiantes.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { By } from '@angular/platform-browser';

describe('InfoEstudiantesComponent', () => {
  let component: InfoEstudiantesComponent;
  let fixture: ComponentFixture<InfoEstudiantesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [InfoEstudiantesComponent],
      imports: [FormsModule, ReactiveFormsModule],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InfoEstudiantesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('debería crear el componente', () => {
    expect(component).toBeTruthy();
  });

  it('debería cambiar el modo de vista', () => {
    expect(component.viewMode).toBe('assign');
    component.toggleViewMode();
    expect(component.viewMode).toBe('list');
    component.toggleViewMode();
    expect(component.viewMode).toBe('assign');
  });

  it('debería actualizar la lista de estudiantes mostrados al buscar', () => {
    component.globalSearchQuery = 'Juan';
    component.displayedStudents = [
      { nameStud: 'Juan Pérez', age: 20, eActivity: true, course: { courseName: 'Matemáticas' }, grade: 8 },
      { nameStud: 'Ana López', age: 22, eActivity: false, course: { courseName: 'Historia' }, grade: 7 }
    ];
    component.updateDisplayedStudents();
    expect(component.displayedStudents.length).toBeGreaterThan(0);
  });

  it('debería asignar una nota correctamente', () => {
    const student = { nameStud: 'Juan Pérez', grade: null };
    component.assignGrade(student, 0);
    expect(student.grade).not.toBeNull();
  });

  it('debería filtrar asignaturas correctamente', () => {
    component.subjects = [
      { idSub: '1', nameSub: 'Matemáticas' },
      { idSub: '2', nameSub: 'Historia' }
    ];
    component.onSubjectChange('1');
    expect(component.selectedSubject).toBe('1');
  });
});