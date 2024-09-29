import { Component,ViewChild , EventEmitter, Input, OnInit, Output, AfterViewInit, ElementRef, TemplateRef } from '@angular/core';
import { NgbActiveModal, NgbModal,NgbModalConfig,NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'form-modal',
  templateUrl: './form-modal.component.html',
  styleUrl: './form-modal.component.css',
  providers: [NgbActiveModal,NgbModalConfig,NgbModal],
})
export class FormModalComponent {
  @Input()
  title!: string;
  @Input() body: any;
  //if type == 0 ; Add, type ==1 ; Update
  @Input() type!:number ;
  @Input() modalBody!: TemplateRef<any>;
  //
  constructor(public activeModal: NgbActiveModal,config: NgbModalConfig, private modalService: NgbModal){
    config.backdrop = 'static';
    config.keyboard = false;
  }
  close(){
    this.modalService.dismissAll();
  }
}
