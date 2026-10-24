import { NgModule } from "@angular/core";
import { HttpClientModule } from "@angular/common/http";
import { RouterModule } from "@angular/router";
import { CommonModule } from "@angular/common";
import { FormsModule,ReactiveFormsModule } from "@angular/forms";
import { AdminLayoutRoutes } from "./admin-layout.routing";
import { ProfesorComponent } from "../../pages/profesor-info/profesor-info.component";
import { InfoEstudiantesComponent } from "../../pages/notas/info-estudiantes.component";
import { TablesComponent } from "../../pages/tables/estudiante.component";
import { NgbModule } from "@ng-bootstrap/ng-bootstrap";

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(AdminLayoutRoutes),
    FormsModule,
    HttpClientModule,
    NgbModule,
    ReactiveFormsModule
  ],
  declarations: [
  
    TablesComponent,
    ProfesorComponent,
    InfoEstudiantesComponent,
    
  ]
})
export class AdminLayoutModule {}
