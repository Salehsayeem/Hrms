import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'custom-button',
  templateUrl: './button.component.html',
  styleUrl: './button.component.css'
})
export class ButtonComponent {

  @Input()
  text:string | undefined;
  @Input()
  btnClass:string | undefined;
  @Input()
  btnDisabled:boolean | undefined;
  @Output() Onclick = new EventEmitter<string>();
  emitEvent() {
    this.Onclick.emit();
  }

}

