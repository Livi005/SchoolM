import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MantenimientoComponent } from './mantenimiento.component';
import { MantenimientoService } from 'src/app/service/mantenimiento.service';
import { of, throwError } from 'rxjs';

class MockMantenimientoService {
  listMantenimientos() {
    return of([
      { maintenanceDate: '2023-01-01', cost: 100, idMean: 1, typeOfMean: 'Tecnológico' },
      { maintenanceDate: '2023-02-01', cost: 200, idMean: 2, typeOfMean: 'Auxiliar' }
    ]);
  }

  createMantenimiento(newMantenimiento: any) {
    return of(newMantenimiento);
  }
}

describe('MantenimientoComponent', () => {
  let component: MantenimientoComponent;
  let fixture: ComponentFixture<MantenimientoComponent>;
  let mockMantenimientoService: MockMantenimientoService;

  beforeEach(() => {
    mockMantenimientoService = new MockMantenimientoService();

    TestBed.configureTestingModule({
      declarations: [MantenimientoComponent],
      providers: [
        { provide: MantenimientoService, useValue: mockMantenimientoService }
      ]
    });

    fixture = TestBed.createComponent(MantenimientoComponent);
    component = fixture.componentInstance;
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should load mantenimientos on init', () => {
    component.ngOnInit();
    fixture.detectChanges();
    expect(component.mantenimientos.length).toBe(2); // Esperamos 2 mantenimientos cargados
    expect(component.mantenimientos[0].maintenanceDate).toBe('2023-01-01');
  });

  it('should add a new mantenimiento', () => {
    // Mock de los datos para el nuevo mantenimiento
    const newMantenimiento = {
      maintenanceDate: '2023-03-01',
      cost: 300,
      idMean: 3,
      typeOfMean: 1,
      idTechMean: 3,
      idAuxMean: 0
    };

    spyOn(mockMantenimientoService, 'createMantenimiento').and.returnValue(of(newMantenimiento));

    component.newMantenimiento = newMantenimiento;
    component.addMantenimiento();

    expect(mockMantenimientoService.createMantenimiento).toHaveBeenCalledWith(newMantenimiento);
    expect(component.mantenimientos.length).toBe(3); // Después de agregar, deberían haber 3 mantenimientos
  });

  it('should handle error when adding mantenimiento', () => {
    const errorResponse = 'Error al agregar mantenimiento';
    spyOn(mockMantenimientoService, 'createMantenimiento').and.returnValue(throwError(() => new Error(errorResponse)));
    spyOn(window, 'alert'); // Mock de alert

    component.newMantenimiento = {
      maintenanceDate: '2023-03-01',
      cost: 300,
      idMean: 3,
      typeOfMean: 1,
      idTechMean: 3,
      idAuxMean: 0
    };
    component.addMantenimiento();

    expect(window.alert).toHaveBeenCalledWith('Error al agregar mantenimiento.');
  });

  it('should update display filter correctly', () => {
    component.globalSearchQuery = 'Tecnológico';
    component.updateDisplayMantenimientos();
    expect(component.mantenimientos.length).toBe(1); // Debería filtrar por "Tecnológico"
    expect(component.mantenimientos[0].typeOfMean).toBe('Tecnológico');
  });
});
