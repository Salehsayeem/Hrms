import { Component } from '@angular/core';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent {
  permissions: any[] = [];
  activeMenus: Set<number> = new Set(); // Store the active menu IDs

  constructor() { }

  ngOnInit(): void {
    const storedPermissions = localStorage.getItem('permissions');

    if (storedPermissions) {
      this.permissions = JSON.parse(storedPermissions);
    }
  }

  toggleMenu(menuId: number): void {
    if (this.activeMenus.has(menuId)) {
      this.activeMenus.delete(menuId); // Remove the menu from active if it's open
    } else {
      this.activeMenus.add(menuId); // Add the menu to active if it's closed
    }
  }

  isActiveMenu(menuId: number): boolean {
    return this.activeMenus.has(menuId); // Check if a menu is active
  }

  closeSidebar(): void {
    document.body.classList.remove('toggle-sidebar');
  }

  clearActiveMenus(): void {
    // Clear all active menus when a single menu is selected
    this.activeMenus.clear();
  }
}
