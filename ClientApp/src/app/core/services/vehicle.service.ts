import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { VehicleMake, VehicleModel, VehicleType } from '../models/vehicle.model';


@Injectable({
    providedIn: 'root'
})
export class VehicleService {
    private apiUrl = `${environment.apiUrl}/vehicle`;


    constructor(private http: HttpClient) { }

    getAllMakes(): Observable<VehicleMake[]> {
        return this.http.get<VehicleMake[]>(`${this.apiUrl}/makes`);
    }

    getVehicleTypes(makeId: number): Observable<VehicleType[]> {
        return this.http.get<VehicleType[]>(`${this.apiUrl}/types/${makeId}`);
    }

    getModels(makeId: number, year: number): Observable<VehicleModel[]> {
        return this.http.get<VehicleModel[]>(`${this.apiUrl}/models/${makeId}/${year}`);
    }
}
