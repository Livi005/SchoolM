// auth.component.spec.ts
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { AuthComponent } from './auth.component';
import { By } from '@angular/platform-browser';

describe('AuthComponent', () => {
  let component: AuthComponent;
  let fixture: ComponentFixture<AuthComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AuthComponent],
      imports: [ReactiveFormsModule, FormsModule],
    }).compileComponents();

    fixture = TestBed.createComponent(AuthComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('debe crear el componente', () => {
    expect(component).toBeTruthy();
  });

  it('debe inicializar el formulario correctamente', () => {
    expect(component.loginForm.contains('username')).toBeTruthy();
    expect(component.loginForm.contains('password')).toBeTruthy();
  });

  it('debe marcar el formulario como inválido si los campos están vacíos', () => {
    component.loginForm.setValue({ username: '', password: '' });
    expect(component.loginForm.invalid).toBeTrue();
  });

  it('debe marcar el formulario como válido si los campos están completos', () => {
    component.loginForm.setValue({ username: 'usuario', password: 'contraseña' });
    expect(component.loginForm.valid).toBeTrue();
  });

  it('debe llamar a onSubmit() al enviar el formulario', () => {
    spyOn(component, 'onSubmit');
    const form = fixture.debugElement.query(By.css('form'));
    form.triggerEventHandler('ngSubmit', null);
    expect(component.onSubmit).toHaveBeenCalled();
  });

  it('debe renderizar correctamente los elementos del formulario', () => {
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('input[formControlName="username"]')).toBeTruthy();
    expect(compiled.querySelector('input[formControlName="password"]')).toBeTruthy();
    expect(compiled.querySelector('button.submit')).toBeTruthy();
  });
});
