import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from '../../core/api.service';
import { RoomCategoryData } from '../../core/interfaces/requests.interface';
import { CommonDdl, CommonResponse } from '../../core/interfaces/response.interface';

@Component({
  selector: 'app-room-categories',
  templateUrl: './room-categories.component.html',
  styleUrl: './room-categories.component.css'
})
export class RoomCategoriesComponent {
  //dropdown
  houses: CommonDdl []= [];
  //form
  categoryName = '';
  categories: RoomCategoryData[] = [];
  errorMessage = '';
  houseId: number = 0;

  constructor(private apiService: ApiService,
    private toastr: ToastrService)
    {
      this.houseListByUser();
    }


  //Dropdown
  houseListByUser() {
      this.apiService.HouseListByUser().subscribe(
        (response) => {
          if (response.succeed && response.statusCode === 200) {
            this.houses = response.data;
          } else {
            this.toastr.error(response.message);
          }
        },
        (error) => {
          this.toastr.error('An error occurred while saving categories');
        }
      );
  }
  // Add a new category to the list
  addCategory() {
    if (this.categoryName.trim() === '') {
      this.errorMessage = 'Category name cannot be empty';
      return;
    }

    // Add category to the local array with a temporary id
    this.categories.push({
      id: 0,
      name: this.categoryName,
    });

    // Clear input field and error message
    this.categoryName = '';
    this.errorMessage = '';
  }

  // Save categories
  saveCategories() {
    const dataToSend = {
      userId: '',
      houseId: this.houseId,
      data: this.categories,
    };

    this.apiService.createOrUpdateRoomCategories(dataToSend).subscribe(
      (response) => {
        if (response.succeed) {
          this.toastr.success('Categories saved successfully');
          this.categories = response.data;
        } else {
          this.toastr.error('Failed to save categories');
        }
      },
      (error) => {
        this.toastr.error('An error occurred while saving categories');
      }
    );
  }
}
