import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-admin-timetable',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <div class="container-fluid py-4">
      <h2 class="mb-4">Timetable Management</h2>
      
      <div class="row">
        <div class="col-md-4 mb-3">
          <div class="card h-100">
            <div class="card-body">
              <h5 class="card-title">
                <i class="ri-calendar-line me-2"></i>
                Academic Terms
              </h5>
              <p class="card-text">Manage academic terms and semesters</p>
              <a routerLink="/admin/academics/terms" class="btn btn-primary">
                Manage Terms
              </a>
            </div>
          </div>
        </div>
        
        <div class="col-md-4 mb-3">
          <div class="card h-100">
            <div class="card-body">
              <h5 class="card-title">
                <i class="ri-group-line me-2"></i>
                Class Sections
              </h5>
              <p class="card-text">Create and manage class sections</p>
              <button class="btn btn-primary" disabled>
                Coming Soon
              </button>
            </div>
          </div>
        </div>
        
        <div class="col-md-4 mb-3">
          <div class="card h-100">
            <div class="card-body">
              <h5 class="card-title">
                <i class="ri-time-line me-2"></i>
                Timetable Slots
              </h5>
              <p class="card-text">Schedule classes and manage timetables</p>
              <button class="btn btn-primary" disabled>
                Coming Soon
              </button>
            </div>
          </div>
        </div>
        
        <div class="col-md-4 mb-3">
          <div class="card h-100">
            <div class="card-body">
              <h5 class="card-title">
                <i class="ri-building-line me-2"></i>
                Rooms
              </h5>
              <p class="card-text">Manage rooms and facilities</p>
              <button class="btn btn-primary" disabled>
                Coming Soon
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
})
export class AdminTimetableComponent {}
