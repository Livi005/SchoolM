import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { SolicitudComponent } from './solicitud.component';
import { SolicitudService } from 'src/app/service/solicitud.service';
import { of } from 'rxjs';

describe('SolicitudComponent', () => {
  let component: SolicitudComponent;
  let fixture: ComponentFixture<SolicitudComponent>;
  let solicitudService: SolicitudService;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [SolicitudComponent],
      imports: [FormsModule, ReactiveFormsModule, HttpClientTestingModule],
      providers: [SolicitudService]
    }).compileComponents();

    fixture = TestBed.createComponent(SolicitudComponent);
    component = fixture.componentInstance;
    solicitudService = TestBed.inject(SolicitudService);
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize the form with default values', () => {
    expect(component.solicitudForm).toBeTruthy();
    expect(component.solicitudForm.get('aula')?.value).toBe('');
    expect((component.solicitudForm.get('medios') as any).controls.length).toBe(0);
  });

  it('should load user info on init', () => {
    spyOn(component, 'CargarInfo');
    component.ngOnInit();
    expect(component.CargarInfo).toHaveBeenCalled();
  });

  it('should fetch and process available medios data', () => {
    const mockResponse = {
      data: {
        'Matemáticas': { 'Proyector': 2, 'Pizarra': 1 }
      }
    };
    spyOn(solicitudService, 'checkAvailableClassroomsAndMeans').and.returnValue(of(mockResponse));

    component.CargarInfo();
    fixture.detectChanges();

    expect(component.result.length).toBe(1);
    expect(component.result[0].subject).toBe('Matemáticas');
    expect(component.result[0].medios.length).toBe(2);
  });

  it('should update selected medios when subject changes', () => {
    component.result = [
      { subject: 'Matemáticas', medios: [{ medio: 'Proyector', cantidad: 2 }] },
      { subject: 'Física', medios: [{ medio: 'Telescopio', cantidad: 1 }] }
    ];

    component.onSubjectChange('Física');
    expect(component.selectedSubject).toBe('Física');
    expect(component.selectedMedios.length).toBe(1);
    expect(component.selectedMedios[0].medio).toBe('Telescopio');
  });

  it('should toggle medios disponibles y ocupados', () => {
    spyOn(component, 'cargarMediosOcupados');
    spyOn(component, 'CargarInfo');

    component.toggleMedios();
    expect(component.mostrarMediosOcupados).toBeTrue();
    expect(component.mostrarMediosDisponibles).toBeFalse();
    expect(component.botonTexto).toBe('Ver Medios Disponibles');
    expect(component.cargarMediosOcupados).toHaveBeenCalled();

    component.toggleMedios();
    expect(component.mostrarMediosOcupados).toBeFalse();
    expect(component.mostrarMediosDisponibles).toBeTrue();
    expect(component.botonTexto).toBe('Ver Medios Ocupados');
    expect(component.CargarInfo).toHaveBeenCalled();
  });
});
