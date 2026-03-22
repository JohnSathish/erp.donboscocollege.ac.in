import { Component, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApplicantPortalStore } from '../dashboard/applicant-portal.store';
import {
  ApplicantApplicationApiService,
  UploadDocumentResult,
} from '@client/shared/data';
import { finalize } from 'rxjs/operators';
import { HttpErrorResponse } from '@angular/common/http';

interface DocumentUploadState {
  isUploading: boolean;
  error: string | null;
}

@Component({
  selector: 'app-documents',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './documents.component.html',
  styleUrls: ['./documents.component.scss'],
})
export class DocumentsComponent {
  private readonly store = inject(ApplicantPortalStore);
  private readonly api = inject(ApplicantApplicationApiService);

  readonly documents = computed(
    () => this.store.dashboard()?.documents ?? []
  );

  // Map document names to document types
  private readonly documentTypeMap: Record<string, string> = {
    'Class X Marksheet': 'StdXMarksheet',
    'Class XII Marksheet': 'StdXIIMarksheet',
    'CUET Marksheet': 'CuetMarksheet',
    'Differently Abled Proof': 'DifferentlyAbledProof',
    'Economically Weaker Proof': 'EconomicallyWeakerProof',
  };

  readonly uploadStates = signal<Record<string, DocumentUploadState>>({});

  readonly allowedFileTypes = ['application/pdf', 'image/jpeg', 'image/png', 'image/jpg'];
  readonly maxFileSizeMB = 5;
  readonly maxFileSizeBytes = this.maxFileSizeMB * 1024 * 1024;

  onFileSelected(event: Event, documentName: string): void {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];

    if (!file) {
      return;
    }

    // Validate file type
    if (!this.allowedFileTypes.includes(file.type)) {
      this.setUploadError(
        documentName,
        `Invalid file type. Allowed types: PDF, JPEG, PNG`
      );
      input.value = '';
      return;
    }

    // Validate file size
    if (file.size > this.maxFileSizeBytes) {
      this.setUploadError(
        documentName,
        `File size exceeds ${this.maxFileSizeMB} MB limit`
      );
      input.value = '';
      return;
    }

    const documentType = this.documentTypeMap[documentName];
    if (!documentType) {
      this.setUploadError(documentName, 'Unknown document type');
      input.value = '';
      return;
    }

    this.uploadDocument(documentName, documentType, file);
  }

  private uploadDocument(
    documentName: string,
    documentType: string,
    file: File
  ): void {
    this.setUploading(documentName, true);

    this.api
      .uploadDocument(documentType, file)
      .pipe(
        finalize(() => {
          this.setUploading(documentName, false);
        })
      )
      .subscribe({
        next: (result: UploadDocumentResult) => {
          this.clearUploadError(documentName);
          // Reload dashboard to refresh document status
          this.store.loadDashboard();
        },
        error: (error: unknown) => {
          let message = 'Failed to upload document. Please try again.';
          if (error instanceof HttpErrorResponse) {
            message =
              (error.error as { message?: string })?.message ?? message;
          }
          this.setUploadError(documentName, message);
        },
      });
  }

  private setUploading(documentName: string, isUploading: boolean): void {
    const current = this.uploadStates();
    this.uploadStates.set({
      ...current,
      [documentName]: {
        ...current[documentName],
        isUploading,
      },
    });
  }

  private setUploadError(documentName: string, error: string): void {
    const current = this.uploadStates();
    this.uploadStates.set({
      ...current,
      [documentName]: {
        ...current[documentName],
        error,
        isUploading: false,
      },
    });
  }

  private clearUploadError(documentName: string): void {
    const current = this.uploadStates();
    this.uploadStates.set({
      ...current,
      [documentName]: {
        ...current[documentName],
        error: null,
      },
    });
  }

  getUploadState(documentName: string): DocumentUploadState {
    return this.uploadStates()[documentName] ?? {
      isUploading: false,
      error: null,
    };
  }

  getDocumentType(documentName: string): string {
    return this.documentTypeMap[documentName] ?? '';
  }
}
