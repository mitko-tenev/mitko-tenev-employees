import { Component } from '@angular/core';
import { CommonProject } from './models/models';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'mitko-tenev-employees';
  commonProjects: CommonProject[] = [];

  onProjectsLoaded(projects: CommonProject[]) {
    this.commonProjects = projects;
  }
}
