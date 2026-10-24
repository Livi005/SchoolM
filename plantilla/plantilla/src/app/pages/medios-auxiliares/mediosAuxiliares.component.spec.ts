import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MedioAuxiliarComponent } from './mediosAuxiliares.component';
import { ReactiveFormsModule } from '@angular/forms';
import { By } from '@angular/platform-browser';

describe('MediosAuxiliaresComponent', () => {
  let component: MedioAuxiliarComponent;
  let fixture: ComponentFixture<MedioAuxiliarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MedioAuxiliarComponent ],
      imports: [ReactiveFormsModule],
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MedioAuxiliarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('debería crear el componente', () => {
    expect(component).toBeTruthy();
  });

  it('debería abrir y cerrar el modal de agregar medio', () => {
    expect(component.isModalVisible).toBeFalse();
    component.toggleModal();
    expect(component.isModalVisible).toBeTrue();
    component.toggleModal();
    expect(component.isModalVisible).toBeFalse();
  });

  it('debería agregar un medio correctamente', () => {
    component.medioForm.setValue({
      nameMean: 'Nuevo Medio',
      state: 'funcionando',
      type: 'Tipo 1'
    });
    
    component.onSubmit();
    expect(component.medios.length).toBe(1);
    expect(component.medios[0].nameMean).toBe('Nuevo Medio');
  });

  it('debería eliminar un medio correctamente', () => {
    const medio = { nameMean: 'Medio de prueba', state: 'funcionando', type: 'Tipo 1' };
    component.medios.push(medio);
    expect(component.medios.length).toBe(1);
    
    component.deleteMedio(medio);
    expect(component.medios.length).toBe(0);
  });

  it('debería abrir el modal de edición con los datos correctos', () => {
    const medio = { nameMean: 'Medio Editar', state: 'enMantenimiento', type: 'Tipo 2' };
    component.openEditModal(medio);
    
    expect(component.isEditModalVisible).toBeTrue();
    expect(component.medioForm.value.nameMean).toBe('Medio Editar');
  });
});
