export interface CreateOrUpdateHouseRequest{
  id: number;
  name: string;
  address: string;
  contact:string;
  userId:string;
}

export interface RoomCategoryData {
  id: number;
  name: string;
}

export interface CreateOrUpdateRoomCategoriesRequest{
  userId: string;
  houseId: number;
  data: RoomCategoryData[];
}

