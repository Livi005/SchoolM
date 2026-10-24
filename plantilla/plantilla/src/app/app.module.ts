import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { HttpClientModule } from "@angular/common/http";
import { RouterModule } from "@angular/router";
import { ToastrModule } from 'ngx-toastr';
import { AppComponent } from "./app.component";
import { AdminLayoutComponent } from "./layouts/admin-layout/admin-layout.component";
import { AuthLayoutComponent } from './layouts/auth-layout/auth-layout.component';
import { NgbModule } from "@ng-bootstrap/ng-bootstrap";
import { AppRoutingModule } from "./app-routing.module";
import { ComponentsModule } from "./components/components.module";
import { EstudentService } from "./service/estudiante.service";
import { AuthComponent } from "./components/Autentificacion/auth.component";
import { CommonModule } from '@angular/common';
import { SolicitudComponent } from './pages/solicitud/solicitud.component';
import { ReactiveFormsModule } from '@angular/forms';
import { MedioAuxiliarComponent } from './pages/medios-auxiliares/mediosAuxiliares.component';
import { PeticionesComponent } from './pages/peticiones/peticiones.component';
import { InfoSecretariaComponent } from './pages/info-secretaria/info-secretaria.component';
import { InfoAdministradorComponent } from './pages/info-administrador/info-administrador.component';
import { MediosTecnologicosComponent } from './pages/medios-tecnologicos/medios-tecnologicos.component';
import { MantenimientoComponent } from "./pages/mantenimiento/mantenimiento.component";
import { EvaluacionComponent } from './pages/evaluacion/evaluacion.component';
import { InfoGeneralComponent } from './pages/info-general/info-general.component';
import { CalificacionComponent } from './pages/calificacion/calificacion.component';
import { AsignarComponent } from './pages/asignar-medio/asignar-medio.component';
import { AsignaturaPComponent } from './pages/asignatura-profesor/asignatura-profesor.component';
import { ClassroomComponent } from "./pages/aulas/aulas.component";
import { AusenciasComponent } from "./pages/ausencias/ausencias.component";
@NgModule({
  imports: [
    BrowserAnimationsModule,
    FormsModule,
    HttpClientModule,
    ComponentsModule,
    NgbModule,
    RouterModule,
    AppRoutingModule,
    ToastrModule.forRoot(),
    CommonModule,
    ReactiveFormsModule,

  ],
  declarations: [AppComponent, AdminLayoutComponent, AuthLayoutComponent,AuthComponent, SolicitudComponent, MedioAuxiliarComponent, PeticionesComponent, InfoSecretariaComponent, InfoAdministradorComponent, MediosTecnologicosComponent, EvaluacionComponent , MantenimientoComponent, InfoGeneralComponent, CalificacionComponent, AsignarComponent, AsignaturaPComponent, ClassroomComponent ,AusenciasComponent],
  providers: [EstudentService],
  bootstrap: [AppComponent]
})
export class AppModule {}
