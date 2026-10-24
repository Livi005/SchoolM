import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { By } from '@angular/platform-browser';
import { InfoSecretariaComponent } from './info-secretaria.component';

describe('InfoSecretariaComponent', () => {
  let component: InfoSecretariaComponent;
  let fixture: ComponentFixture<InfoSecretariaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [InfoSecretariaComponent],
      imports: [FormsModule],
    }).compileComponents();

    fixture = TestBed.createComponent(InfoSecretariaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should add a new secretary', () => {
    component.newSecretary = { nameS: 'Maria', salaryS: 3000 };
    component.addSecretary();

    expect(component.secretaries.length).toBe(1);
    expect(component.secretaries[0].secretary.nameS).toBe('Maria');
    expect(component.secretaries[0].secretary.salaryS).toBe(3000);
  });

  it('should edit an existing secretary', () => {
    component.secretaries = [
      {
        id: 1,
        secretary: { nameS: 'Maria', salaryS: 3000 },
      },
    ];

    component.editSecretary(component.secretaries[0]);
    expect(component.editingSecretary).toEqual(component.secretaries[0]);

    component.editingSecretary.secretary.nameS = 'Maria Updated';
    component.saveSecretary();

    expect(component.secretaries[0].secretary.nameS).toBe('Maria Updated');
    expect(component.editingSecretary).toBeNull();
  });

  it('should delete a secretary', () => {
    component.secretaries = [
      {
        id: 1,
        secretary: { nameS: 'Maria', salaryS: 3000 },
      },
    ];

    component.deleteSecretary(1);
    expect(component.secretaries.length).toBe(0);
  });

  it('should cancel editing a secretary', () => {
    component.secretaries = [
      {
        id: 1,
        secretary: { nameS: 'Maria', salaryS: 3000 },
      },
    ];

    component.editSecretary(component.secretaries[0]);
    component.cancelEdit();

    expect(component.editingSecretary).toBeNull();
  });

  it('should display the list of secretaries', () => {
    component.secretaries = [
      {
        id: 1,
        secretary: { nameS: 'Maria', salaryS: 3000 },
      },
      {
        id: 2,
        secretary: { nameS: 'Juan', salaryS: 3500 },
      },
    ];
    fixture.detectChanges();

    const rows = fixture.debugElement.queryAll(By.css('tbody tr'));
    expect(rows.length).toBe(2);
    expect(rows[0].query(By.css('td')).nativeElement.textContent).toContain('Maria');
    expect(rows[1].query(By.css('td')).nativeElement.textContent).toContain('Juan');
  });
});