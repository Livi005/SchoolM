import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AsignaturaPComponent } from './asignatura-profesor.component';

describe('AsignaturaProfesorComponent', () => {
  let component: AsignaturaPComponent;
  let fixture: ComponentFixture<AsignaturaPComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AsignaturaPComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AsignaturaPComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
