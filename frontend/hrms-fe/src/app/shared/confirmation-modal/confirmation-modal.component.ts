import { Component, Input, TemplateRef } from '@angular/core';
import { NgbActiveModal, NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'confirmation-modal',
  templateUrl: './confirmation-modal.component.html',
  styleUrl: './confirmation-modal.component.css'
})
export class ConfirmationModalComponent {
  @Input()
  title!: string;
  @Input() body: any;
  @Input() modalBody!: TemplateRef<any>;
  constructor(public activeModal: NgbActiveModal,config: NgbModalConfig, private modalService: NgbModal){
    config.backdrop = 'static';
    config.keyboard = false;
  }
  close(){
    this.modalService.dismissAll();
  }
}
