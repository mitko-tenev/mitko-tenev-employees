import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CommonProject } from '../../models/models';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class EmployeeProjectService {
  constructor(private http: HttpClient) { }

  uploadFile(file: File): Observable<CommonProject[]> {
    const formData = new FormData();
    formData.append('file', file);

    return this.http.post<CommonProject[]>(`${environment.apiUrl}/api/EmployeeProject/upload`, formData);
  }
}
