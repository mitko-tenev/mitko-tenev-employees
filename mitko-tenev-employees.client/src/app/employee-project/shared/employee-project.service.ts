import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CommonProject } from '../../models/models';

@Injectable({
  providedIn: 'root'
})
export class EmployeeProjectService {
  private apiUrl = '/api/EmployeeProject'; // TODO: Replace with your API endpoint

  constructor(private http: HttpClient) { }

  uploadFile(file: File): Observable<CommonProject[]> {
    const formData = new FormData();
    formData.append('file', file);

    return this.http.post<CommonProject[]>(`${this.apiUrl}/upload`, formData);
  }
}
