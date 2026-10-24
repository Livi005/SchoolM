

import { Routes } from "@angular/router";
import { ProfesorComponent } from "../../pages/profesor-info/profesor-info.component";
import { InfoEstudiantesComponent } from "../../pages/notas/info-estudiantes.component";
import { TablesComponent } from "../../pages/tables/estudiante.component";
import { SolicitudComponent } from "src/app/pages/solicitud/solicitud.component";
import {MedioAuxiliarComponent } from "src/app/pages/medios-auxiliares/mediosAuxiliares.component";
import { PeticionesComponent } from "src/app/pages/peticiones/peticiones.component";
import { InfoSecretariaComponent} from "src/app/pages/info-secretaria/info-secretaria.component";
import { InfoAdministradorComponent } from "src/app/pages/info-administrador/info-administrador.component";
import { MediosTecnologicosComponent } from "src/app/pages/medios-tecnologicos/medios-tecnologicos.component";
import { EvaluacionComponent } from "src/app/pages/evaluacion/evaluacion.component";
import { MantenimientoComponent } from "src/app/pages/mantenimiento/mantenimiento.component";
import { CalificacionComponent } from "src/app/pages/calificacion/calificacion.component";
import { AsignarComponent } from "src/app/pages/asignar-medio/asignar-medio.component";
import { AsignaturaPComponent } from "src/app/pages/asignatura-profesor/asignatura-profesor.component";
import { ClassroomComponent } from "src/app/pages/aulas/aulas.component";
import { AusenciasComponent } from "src/app/pages/ausencias/ausencias.component";
export const AdminLayoutRoutes: Routes = [
  { path: "profesor", component: ProfesorComponent },
  { path: "calificacion-estudiantes", component: InfoEstudiantesComponent },
  { path: "estudiantes", component: TablesComponent },
  { path : "solicitar", component : SolicitudComponent},
  { path : "medio-auxiliar" , component :MedioAuxiliarComponent},
  { path : "peticiones" , component : PeticionesComponent },
  { path : "info-secretaria" , component :InfoSecretariaComponent},
  { path : "info-administrador" , component :InfoAdministradorComponent},
  { path : "medio-tecnologico" , component :MediosTecnologicosComponent},
  { path :  "evaluaciones" , component :EvaluacionComponent},
  { path : "mantenimiento" , component :MantenimientoComponent},
  { path : "calificacion" , component :CalificacionComponent},
  { path : "asignar-medio" , component :AsignarComponent},
  { path : "asignatura-profesor" , component :AsignaturaPComponent},
  { path : "aulas" , component :ClassroomComponent},
  {path : "ausencias" , component :AusenciasComponent}
];
