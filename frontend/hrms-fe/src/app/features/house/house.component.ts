import { Component, TemplateRef, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from '../../core/api.service';
import { HouseTable } from '../../core/interfaces/response.interface';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FormModalComponent } from '../../shared/form-modal/form-modal.component';
import { ConfirmationModalComponent } from '../../shared/confirmation-modal/confirmation-modal.component';

@Component({
  selector: 'app-house',
  templateUrl: './house.component.html',
  styleUrl: './house.component.css'
})
export class HouseComponent {
  search: string = '';
  pageNo: number = 1;
  pageSize: number = 5;
  totalCount: number = 0;
  houses: HouseTable[] = [];
  isModalOpen:boolean = false;
  //for modal
  id: number = 0;
  houseForm!: FormGroup;
  modalTitle: string = '';
  modalBody: any;
  //if type == 0 ; Add, type ==1 ; Update
  addOrUpdate!: number;
  submitted = false;
  @ViewChild('formTemplate', { static: true }) formTemplate!: TemplateRef<any>;
  //for preview
  @ViewChild('previewTemplate', { static: true }) previewTemplate!: TemplateRef<any>;
  deleteId = 0;
  @ViewChild('deleteTemplate', { static: true }) deleteTemplate!: TemplateRef<any>;
  previewData: any;
  constructor(private toastrService: ToastrService,
    private apiService: ApiService,
    private fb: FormBuilder,
    private modalService: NgbModal
  ) {
    this.getHousesPagination();
    this.houseForm = this.fb.group({
      id: new FormControl(0),
      name: new FormControl("", Validators.required),
      address: new FormControl("", Validators.required),
      contact: new FormControl("", Validators.required),
    });
  }

  getHousesPagination(): void {
    this.apiService.HouseLandingPagination(this.search, this.pageNo, this.pageSize)
      .subscribe(resp => {

        this.houses = (<any>resp).data.response || [];
        this.totalCount = (<any>resp).data.totalCount;
      })
  }
  onPageChange(pageNumber: number): void {
    this.pageNo = pageNumber;
    this.getHousesPagination();
  }
  get formControls() {
    return this.houseForm.controls;
  }
  openModal(type: number, id?: number) {
    this.isModalOpen = true;
    const modalRef = this.modalService.open(FormModalComponent);
    if (type == 0) {
      this.houseForm.reset();
    }
    if (type == 1) {
      if (id === null || id === undefined || id == 0) {
        return;
      }
      else {
        this.apiService.GetHouseById(id).subscribe((resp) => {
          const apiData = (<any>resp).data;
          console.log(apiData)
          this.id = id;
          this.houseForm = this.fb.group({
            id: new FormControl(id, Validators.required),
            name: new FormControl(apiData.name, Validators.required),
            address: new FormControl(apiData.address, Validators.required),
            contact: new FormControl(apiData.contact, Validators.required),
          });
        });
      }
    }
    // Show the modal
    modalRef.componentInstance.title = ' House';
    modalRef.componentInstance.type = type;
    modalRef.componentInstance.modalBody = this.formTemplate;
  }

  deleteModal(id: number) {
    const modalRef = this.modalService.open(ConfirmationModalComponent);
    if (id === null || id === undefined || id == 0) {
      return;
    }
    this.deleteId = id;
    modalRef.componentInstance.title = `Are you sure? `;
    modalRef.componentInstance.modalBody = this.deleteTemplate;
  }
  modalSaveOrUpdate() {
    this.submitted = true;

    if (this.houseForm.invalid) {
      return;
    }

    if (this.houseForm.valid) {
      this.houseForm.value.id = this.id;

      try {
        this.apiService.createOrUpdateHouse(this.houseForm.value).subscribe(
          (data) => {
            try {
              if ((<any>data).succeed === true && (<any>data).statusCode === 200) {
                this.toastrService.success((<any>data).message);
                this.modalService.dismissAll();
                this.houseForm.reset();
                this.getHousesPagination();
                this.submitted = false;
              } else {
                this.toastrService.error((<any>data).message);
                setTimeout(() => (this.submitted = false), 1000);
              }
            } catch (error) {
              this.toastrService.error('An error occurred while processing the response.');
              console.error('Response Error:', error);
            }
          },
          (error) => {
            // Handle API error (like 500 or network errors)
            this.toastrService.error('An error occurred while saving the house. Please try again.');
            console.error('API Error:', error.HttpErrorResponse );
            this.submitted = false;
          }
        );
      } catch (error) {
        // Handle unexpected errors
        this.toastrService.error('Unexpected error occurred.');
        console.error('Unexpected Error:', error);
        this.submitted = false;
      }
    } else {
      this.submitted = false;
    }
  }

  deleteHouse() {
    try {
      this.apiService.DeleteHouse(this.deleteId).subscribe(
        (data) => {
          if ((<any>data).succeed === true && (<any>data).statusCode === 200) {

            this.toastrService.success((<any>data).message);
            this.getHousesPagination();
          } else {
            this.toastrService.error((<any>data).message);
          }
          this.modalService.dismissAll();
        },
        (error) => {
          this.toastrService.error('An error occurred while deleting the house');
          console.error('Error deleting house:', error);
        }
      );
    } catch (error) {
      this.toastrService.error('An unexpected error occurred');
      console.error('Unexpected error:', error);
    }
  }

}
