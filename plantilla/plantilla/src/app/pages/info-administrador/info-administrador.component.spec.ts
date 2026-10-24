import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { By } from '@angular/platform-browser';
import { InfoAdministradorComponent } from './info-administrador.component';

describe('InfoAdministradorComponent', () => {
  let component: InfoAdministradorComponent;
  let fixture: ComponentFixture<InfoAdministradorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [InfoAdministradorComponent],
      imports: [FormsModule],
    }).compileComponents();

    fixture = TestBed.createComponent(InfoAdministradorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should add a new administrator', () => {
    component.newAdministrator = { adminName: 'Carlos', adminSalary: 4000 };
    component.addAdministrator();

    expect(component.administrators.length).toBe(1);
    expect(component.administrators[0].administrator.adminName).toBe('Carlos');
    expect(component.administrators[0].administrator.adminSalary).toBe(4000);
  });

  it('should edit an existing administrator', () => {
    component.administrators = [
      {
        id: 1,
        administrator: { adminName: 'Carlos', adminSalary: 4000 },
      },
    ];

    component.editAdministrator(component.administrators[0]);
    expect(component.editingAdministrator).toEqual(component.administrators[0]);

    component.editingAdministrator.administrator.adminName = 'Carlos Updated';
    component.saveAdministrator();

    expect(component.administrators[0].administrator.adminName).toBe('Carlos Updated');
    expect(component.editingAdministrator).toBeNull();
  });

  it('should delete an administrator', () => {
    component.administrators = [
      {
        id: 1,
        administrator: { adminName: 'Carlos', adminSalary: 4000 },
      },
    ];

    component.deleteAdministrator(1);
    expect(component.administrators.length).toBe(0);
  });

  it('should cancel editing an administrator', () => {
    component.administrators = [
      {
        id: 1,
        administrator: { adminName: 'Carlos', adminSalary: 4000 },
      },
    ];

    component.editAdministrator(component.administrators[0]);
    component.cancelEdit();

    expect(component.editingAdministrator).toBeNull();
  });

  it('should display the list of administrators', () => {
    component.administrators = [
      {
        id: 1,
        administrator: { adminName: 'Carlos', adminSalary: 4000 },
      },
      {
        id: 2,
        administrator: { adminName: 'Ana', adminSalary: 4500 },
      },
    ];
    fixture.detectChanges();

    const rows = fixture.debugElement.queryAll(By.css('tbody tr'));
    expect(rows.length).toBe(2);
    expect(rows[0].query(By.css('td')).nativeElement.textContent).toContain('Carlos');
    expect(rows[1].query(By.css('td')).nativeElement.textContent).toContain('Ana');
  });
});
