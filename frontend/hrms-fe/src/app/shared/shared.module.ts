import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SidebarComponent } from './sidebar/sidebar.component';
import { HeaderComponent } from './header/header.component';
import { FooterComponent } from './footer/footer.component';
import { RouterModule } from '@angular/router';
import { ButtonComponent } from './button/button.component';
import { FormModalComponent } from './form-modal/form-modal.component';
import { ConfirmationModalComponent } from './confirmation-modal/confirmation-modal.component';
import { PaginationComponent } from './pagination/pagination.component';
import { NgbActiveModal, NgbModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
  declarations: [
    SidebarComponent,
    HeaderComponent,
    FooterComponent,
    ButtonComponent,
    FormModalComponent,
    ConfirmationModalComponent,
    PaginationComponent,
  ],
  imports: [
    CommonModule,
    RouterModule,
    NgbModule
  ],
  exports: [
    SidebarComponent,
    HeaderComponent,
    FooterComponent,
    ButtonComponent,
    FormModalComponent,
    ConfirmationModalComponent,
    PaginationComponent,
  ],
  providers: [
    NgbActiveModal // Add NgbActiveModal to providers
  ]
})
export class SharedModule { }
