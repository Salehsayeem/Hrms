import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { finalize, Observable } from 'rxjs';
import { environment } from '../../environment';
import { LoginRequest, LoginResponse } from './interfaces/auth.interface';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { CommonResponse } from './interfaces/response.interface';
import { AuthService } from './auth.service';
import { CreateOrUpdateHouseRequest, CreateOrUpdateRoomCategoriesRequest } from './interfaces/requests.interface';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private baseUrl = environment.baseUrl;
  userId: string | undefined = "";
  //#region  AuthAPi

  constructor(private http: HttpClient, authService: AuthService) {
    const token = localStorage.getItem('token');
    this.userId = authService.getDecodedToken(token!)?.UserId;
  }

  checkConnection(): Observable<boolean> {
    return this.http.get<boolean>(`${this.baseUrl}api/Health/CheckConnection`);
  }

  login(payload: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.baseUrl}api/Auth/Login`, payload);
  }
  //#endregion

  //#region HouseApi
  HouseLandingPagination(search: string, pageNo: number, pageSize: number): Observable<CommonResponse> {
    const params = new HttpParams()
      .set('search', search)
      .set('userId', this.userId!)
      .set('pageNo', pageNo)
      .set('pageSize', pageSize);

    return this.http.get<CommonResponse>(`${this.baseUrl}api/Houses/HouseLandingPagination`, { params });
  }

  GetHouseById(id: number): Observable<CommonResponse> {
    const params = new HttpParams()
      .set('id', id);
    return this.http.get<CommonResponse>(`${this.baseUrl}api/Houses/GetHouseById`, { params });
  }
  DeleteHouse(id: number): Observable<CommonResponse> {
    const params = new HttpParams()
      .set('id', id)
      .set('userId', this.userId!);
    return this.http.delete<CommonResponse>(`${this.baseUrl}api/Houses/DeleteHouse`, { params });
  }
  createOrUpdateHouse(request: CreateOrUpdateHouseRequest): Observable<CommonResponse> {
    request.userId = this.userId!;
    return this.http.post<CommonResponse>(`${this.baseUrl}api/Houses/CreateOrUpdateHouse`, request);
  }
  //#endregion
  //#region Common
  HouseListByUser(): Observable<CommonResponse> {
    const params = new HttpParams()
      .set('userId', this.userId!);
    return this.http.get<CommonResponse>(`${this.baseUrl}api/Common/HouseListByUser`, { params });
  }
  //#endregion
  //#region Room categories
  createOrUpdateRoomCategories(request: CreateOrUpdateRoomCategoriesRequest): Observable<CommonResponse> {
    request.userId = this.userId!;
    return this.http.post<CommonResponse>(`${this.baseUrl}api/Houses/CreateUpdateRoomCategory`, request);
  }
  //#endregion
}
