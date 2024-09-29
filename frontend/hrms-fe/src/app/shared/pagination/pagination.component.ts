import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'pagination',
  templateUrl: './pagination.component.html',
  styleUrl: './pagination.component.css'
})
export class PaginationComponent {
  @Input() currentPage: number=0;
  @Input() totalPages: number=0;
  @Input() totalCount: number=0;
  @Input() pageSize: number =0;
  @Output() pageChanged = new EventEmitter<number>();
  get pages(): number[] {
    const pages: number[] = [];
    for (let i = 1; i <= this.totalPages; i++) {
      pages.push(i);
    }
    return pages;
  }
  onPrevious() {
    if (this.currentPage > 1) {
      this.onPageChange(this.currentPage - 1);
    }
  }
  onNext() {
    if (this.currentPage < this.totalPages) {
      this.onPageChange(this.currentPage + 1);
    }
  }
  onPageChange(page: number) {
    this.pageChanged.emit(page);
  }
}
