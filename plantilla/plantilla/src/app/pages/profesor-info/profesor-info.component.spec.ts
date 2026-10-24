import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { By } from '@angular/platform-browser';
import { ProfesorComponent } from './profesor-info.component';

describe('ProfesorInfoComponent', () => {
  let component: ProfesorComponent;
  let fixture: ComponentFixture<ProfesorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ProfesorComponent],
      imports: [FormsModule],
    }).compileComponents();

    fixture = TestBed.createComponent(ProfesorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should open and close the modal', () => {
    component.openModal();
    expect(component.showModal).toBeTrue();

    component.closeModal();
    expect(component.showModal).toBeFalse();
  });

  it('should add a new professor', () => {
    component.newProfesor = {
      nameProf: 'Dr. Smith',
      contract: 'Full-Time',
      salary: 50000,
      laboralExperience: 10,
    };
    component.saveProfesor();

    expect(component.profesores.length).toBe(1);
    expect(component.profesores[0].professor.nameProf).toBe('Dr. Smith');
  });

  it('should edit an existing professor', () => {
    component.profesores = [
      {
        id: 1,
        professor: {
          nameProf: 'Dr. Jones',
          contract: 'Part-Time',
          salary: 30000,
          laboralExperience: 5,
        },
        isEditing: false,
      },
    ];

    const professorToEdit = component.profesores[0];
    component.toggleEdit(professorToEdit);

    expect(professorToEdit.isEditing).toBeTrue();

    professorToEdit.professor.nameProf = 'Dr. Updated';
    component.saveChanges(professorToEdit);

    expect(professorToEdit.professor.nameProf).toBe('Dr. Updated');
    expect(professorToEdit.isEditing).toBeFalse();
  });

  it('should delete a professor', () => {
    component.profesores = [
      {
        id: 1,
        professor: {
          nameProf: 'Dr. Smith',
          contract: 'Full-Time',
          salary: 50000,
          laboralExperience: 10,
        },
        isEditing: false,
      },
    ];

    component.deleteProfesor(1);
    expect(component.profesores.length).toBe(0);
  });

  it('should filter professors based on search query', () => {
    component.profesores = [
      {
        id: 1,
        professor: {
          nameProf: 'Dr. Smith',
          contract: 'Full-Time',
          salary: 50000,
          laboralExperience: 10,
        },
        isEditing: false,
      },
      {
        id: 2,
        professor: {
          nameProf: 'Dr. Jones',
          contract: 'Part-Time',
          salary: 30000,
          laboralExperience: 5,
        },
        isEditing: false,
      },
    ];

    component.globalSearchQuery = 'Jones';
    component.updateDisplayProfesor();
    expect(component.profesores.length).toBe(1);
    expect(component.profesores[0].professor.nameProf).toBe('Dr. Jones');
  });
});
