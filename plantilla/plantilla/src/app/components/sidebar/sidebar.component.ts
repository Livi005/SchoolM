import { Component, OnInit } from "@angular/core";
import { RoleService } from "src/app/service/role.service";
import { MenuItem, } from "src/app/models/role-permissions.model";
import { AuthGuard } from "../Autentificacion/auth.service";

@Component({
  selector: "app-sidebar",
  templateUrl: "./sidebar.component.html",
  styleUrls: ["./sidebar.component.css"]
})
export class SidebarComponent implements OnInit {
  menuItems: MenuItem[] = [];
  constructor(private roleService: RoleService , private authGuard :AuthGuard) {}

  ngOnInit():void {
    const userRole = this.authGuard.getUserRole(); 
    this.menuItems = this.roleService.getMenuItemsForRole(userRole);
  }

  isMobileMenu() {
    if (window.innerWidth > 980) {
      return false;
    }
    return true;
  }

}
