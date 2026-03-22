import {
  ChangeDetectionStrategy,
  Component,
  OnInit,
  inject,
  signal,
  CUSTOM_ELEMENTS_SCHEMA,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { StaffApiService, StaffMemberDto } from '@client/shared/data';
import { ToastService } from '../../../shared/toast.service';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-admin-staff-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './admin-staff-form.component.html',
  styleUrls: ['./admin-staff-form.component.scss'],
})
export class AdminStaffFormComponent implements OnInit {
  private readonly api = inject(StaffApiService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);
  private readonly fb = inject(FormBuilder);
  private readonly toast = inject(ToastService);

  protected readonly loading = signal(false);
  protected readonly isEditMode = signal(false);
  protected readonly staffId = signal<string | null>(null);

  protected readonly staffForm = this.fb.group({
    employeeNumber: ['', [Validators.required, Validators.maxLength(50)]],
    firstName: ['', [Validators.required, Validators.maxLength(128)]],
    lastName: ['', [Validators.required, Validators.maxLength(128)]],
    email: ['', [Validators.required, Validators.email, Validators.maxLength(256)]],
    mobileNumber: ['', [Validators.required, Validators.maxLength(32)]],
    dateOfBirth: ['', [Validators.required]],
    gender: ['', [Validators.required, Validators.maxLength(20)]],
    department: ['', [Validators.maxLength(100)]],
    designation: ['', [Validators.maxLength(100)]],
    employeeType: ['', [Validators.maxLength(50)]],
    joinDate: ['', [Validators.required]],
    address: ['', [Validators.maxLength(500)]],
    emergencyContactName: ['', [Validators.maxLength(128)]],
    emergencyContactNumber: ['', [Validators.maxLength(32)]],
    qualifications: ['', [Validators.maxLength(500)]],
    specialization: ['', [Validators.maxLength(200)]],
  });

  async ngOnInit(): Promise<void> {
    const id = this.route.snapshot.paramMap.get('id');
    if (id && id !== 'new') {
      this.isEditMode.set(true);
      this.staffId.set(id);
      await this.loadStaff(id);
    } else {
      // In create mode, employee number is editable
      this.staffForm.get('employeeNumber')?.enable();
    }
  }

  async loadStaff(id: string): Promise<void> {
    this.loading.set(true);
    try {
      const staff = await firstValueFrom(this.api.getStaffMember(id));
      this.populateForm(staff);
    } catch (error) {
      console.error('Failed to load staff member', error);
      this.toast.show('Failed to load staff member details', 'error');
      this.router.navigate(['/admin/staff/list']);
    } finally {
      this.loading.set(false);
    }
  }

  private populateForm(staff: StaffMemberDto): void {
    // In edit mode, employee number is read-only
    this.staffForm.get('employeeNumber')?.disable();
    
    this.staffForm.patchValue({
      employeeNumber: staff.employeeNumber,
      firstName: staff.firstName,
      lastName: staff.lastName,
      email: staff.email,
      mobileNumber: staff.mobileNumber,
      dateOfBirth: this.formatDateForInput(staff.dateOfBirth),
      gender: staff.gender,
      department: staff.department || '',
      designation: staff.designation || '',
      employeeType: staff.employeeType || '',
      joinDate: this.formatDateForInput(staff.joinDate),
      address: staff.address || '',
      emergencyContactName: staff.emergencyContactName || '',
      emergencyContactNumber: staff.emergencyContactNumber || '',
      qualifications: staff.qualifications || '',
      specialization: staff.specialization || '',
    });
  }

  async onSubmit(): Promise<void> {
    if (this.staffForm.invalid) {
      this.staffForm.markAllAsTouched();
      this.toast.show('Please correct the form errors', 'error');
      return;
    }

    this.loading.set(true);
    try {
      const formValue = this.staffForm.getRawValue();
      
      if (this.isEditMode() && this.staffId()) {
        // Update existing staff
        await firstValueFrom(
          this.api.updateStaffMember(this.staffId()!, {
            firstName: formValue.firstName!,
            lastName: formValue.lastName!,
            email: formValue.email!,
            mobileNumber: formValue.mobileNumber!,
            department: formValue.department || null,
            designation: formValue.designation || null,
            employeeType: formValue.employeeType || null,
            address: formValue.address || null,
            emergencyContactName: formValue.emergencyContactName || null,
            emergencyContactNumber: formValue.emergencyContactNumber || null,
            qualifications: formValue.qualifications || null,
            specialization: formValue.specialization || null,
          })
        );
        this.toast.show('Staff member updated successfully', 'success');
      } else {
        // Create new staff
        await firstValueFrom(
          this.api.createStaffMember({
            employeeNumber: formValue.employeeNumber!,
            firstName: formValue.firstName!,
            lastName: formValue.lastName!,
            email: formValue.email!,
            mobileNumber: formValue.mobileNumber!,
            dateOfBirth: formValue.dateOfBirth!,
            gender: formValue.gender!,
            department: formValue.department || null,
            designation: formValue.designation || null,
            employeeType: formValue.employeeType || null,
            joinDate: formValue.joinDate!,
            address: formValue.address || null,
            emergencyContactName: formValue.emergencyContactName || null,
            emergencyContactNumber: formValue.emergencyContactNumber || null,
            qualifications: formValue.qualifications || null,
            specialization: formValue.specialization || null,
          })
        );
        this.toast.show('Staff member created successfully', 'success');
      }
      
      this.router.navigate(['/admin/staff/list']);
    } catch (error: any) {
      console.error('Failed to save staff member', error);
      this.toast.show(
        error?.error?.message || 'Failed to save staff member. Please try again.',
        'error'
      );
    } finally {
      this.loading.set(false);
    }
  }

  onCancel(): void {
    this.router.navigate(['/admin/staff/list']);
  }

  private formatDateForInput(dateString: string): string {
    if (!dateString) return '';
    const date = new Date(dateString);
    const year = date.getFullYear();
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const day = date.getDate().toString().padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  getFieldError(fieldName: string): string | null {
    const control = this.staffForm.get(fieldName);
    if (control && control.invalid && control.touched) {
      if (control.errors?.['required']) {
        return `${this.getFieldLabel(fieldName)} is required`;
      }
      if (control.errors?.['email']) {
        return 'Please enter a valid email address';
      }
      if (control.errors?.['maxlength']) {
        const maxLength = control.errors['maxlength'].requiredLength;
        return `Maximum length is ${maxLength} characters`;
      }
    }
    return null;
  }

  private getFieldLabel(fieldName: string): string {
    const labels: Record<string, string> = {
      employeeNumber: 'Employee Number',
      firstName: 'First Name',
      lastName: 'Last Name',
      email: 'Email',
      mobileNumber: 'Mobile Number',
      dateOfBirth: 'Date of Birth',
      gender: 'Gender',
      department: 'Department',
      designation: 'Designation',
      employeeType: 'Employee Type',
      joinDate: 'Join Date',
    };
    return labels[fieldName] || fieldName;
  }
}
