import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { RouterModule } from '@angular/router';
import { MainBodyComponent } from './main-body/main-body.component';
import { HouseComponent } from './house/house.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RoomCategoriesComponent } from './room-categories/room-categories.component';

@NgModule({
  declarations: [
    MainBodyComponent,
    HouseComponent,
    RoomCategoriesComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule
  ],
  exports:[
    MainBodyComponent,
    HouseComponent,
    RoomCategoriesComponent
  ]
})
export class FeaturesModule { }
