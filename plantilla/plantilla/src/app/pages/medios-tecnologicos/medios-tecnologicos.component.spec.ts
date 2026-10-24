import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MediosTecnologicosComponent } from './medios-tecnologicos.component';
import { ReactiveFormsModule } from '@angular/forms';
import { By } from '@angular/platform-browser';

describe('MediosTecnologicosComponent', () => {
  let component: MediosTecnologicosComponent;
  let fixture: ComponentFixture<MediosTecnologicosComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MediosTecnologicosComponent ],
      imports: [ReactiveFormsModule],
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MediosTecnologicosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('debería crear el componente', () => {
    expect(component).toBeTruthy();
  });

  it('debería abrir y cerrar el modal de agregar medio tecnológico', () => {
    expect(component.isModalVisible).toBeFalse();
    component.toggleModal();
    expect(component.isModalVisible).toBeTrue();
    component.toggleModal();
    expect(component.isModalVisible).toBeFalse();
  });

  it('debería agregar un medio tecnológico correctamente', () => {
    component.medioForm.setValue({
      nameMean: 'Nuevo Medio Tecnológico',
      state: 'funcionando'
    });
    
    component.onSubmit();
    expect(component.medios.length).toBe(1);
    expect(component.medios[0].nameMean).toBe('Nuevo Medio Tecnológico');
  });

  it('debería eliminar un medio tecnológico correctamente', () => {
    const medio = { nameMean: 'Medio de prueba', state: 'funcionando' };
    component.medios.push(medio);
    expect(component.medios.length).toBe(1);
    
    component.deleteMedio(medio);
    expect(component.medios.length).toBe(0);
  });

  it('debería abrir el modal de edición con los datos correctos', () => {
    const medio = { nameMean: 'Medio Editar', state: 'Mantenimiento' };
    component.editMedio(medio);
    
    expect(component.isEditModalVisible).toBeTrue();
    expect(component.medioForm.value.nameMean).toBe('Medio Editar');
  });
});
