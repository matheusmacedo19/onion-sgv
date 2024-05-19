import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DashboardOrder } from '../models/dashboardOrder';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {

  baseUrl: string = "https://localhost:44322/api";

  constructor(private http : HttpClient) { }

  findAll():Observable<DashboardOrder[]>{
    const url = this.baseUrl + "/dashboard";
    return this.http.get<DashboardOrder[]>(url);
  }
}
