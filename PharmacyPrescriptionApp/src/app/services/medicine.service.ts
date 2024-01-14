import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Medicine } from '../models/prescription.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MedicineService {
  private apiUrl = '${environment.apiUrl}/medicine';

  constructor(private http: HttpClient) { }

  searchMedicine(term: string): Observable<Medicine[]> {
    const url = `${this.apiUrl}/search?name=${term}`;
    return this.http.get<Medicine[]>(url);
  }
}
