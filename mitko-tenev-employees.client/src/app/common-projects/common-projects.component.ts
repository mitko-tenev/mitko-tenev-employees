import { Component, Input } from '@angular/core';
import { CommonProject } from '../models/models';

@Component({
  selector: 'app-common-projects',
  templateUrl: './common-projects.component.html',
  //styleUrls: ['./common-projects.component.css'] TODO
})
export class CommonProjectsComponent {
  @Input() projects: CommonProject[] = [];

  getTotalDays(): number {
    return this.projects.reduce((sum, project) => sum + project.daysWorked, 0);
  }
}
