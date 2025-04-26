import { Component, EventEmitter, Output } from '@angular/core';
import { EmployeeProjectService } from '../employee-project/shared/employee-project.service';
import { CommonProject } from '../models/models';

@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.css']
})
export class FileUploadComponent {
  @Output() projectsLoaded = new EventEmitter<CommonProject[]>();
  selectedFile: File | null = null;
  isLoading = false;
  errorMessage = '';

  constructor(private employeeProjectService: EmployeeProjectService) { }

  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.selectedFile = file;
    }
  }

  onUpload() {
    this.errorMessage = '';

    if (!this.selectedFile) {
      this.errorMessage = 'Please select a file first';
      return;
    }

    this.isLoading = true;
    this.employeeProjectService.uploadFile(this.selectedFile)
      .subscribe({
        next: (data) => {
          this.projectsLoaded.emit(data);
          this.isLoading = false;
        },
        error: (error) => {
          this.errorMessage = 'Error processing file: ' + error.message;
          this.isLoading = false;
        }
      });
  }
}
