import { HttpErrorResponse } from '@angular/common/http';

/**
 * Resolves a human-readable message from an Angular HttpClient error.
 * Handles Blob bodies (e.g. failed POST with responseType: 'blob') and JSON ProblemDetails.
 */
export async function readHttpErrorMessage(error: unknown): Promise<string> {
  if (!(error instanceof HttpErrorResponse)) {
    return error instanceof Error ? error.message : 'Request failed';
  }

  const body = error.error;

  if (body instanceof Blob) {
    const text = (await body.text()).trim();
    return parseBodyText(text) || fallbackStatusMessage(error);
  }

  if (typeof body === 'string' && body.trim()) {
    return parseBodyText(body.trim()) ?? body.trim();
  }

  if (body && typeof body === 'object') {
    const o = body as Record<string, unknown>;
    const msg =
      (typeof o.message === 'string' && o.message) ||
      (typeof o.detail === 'string' && o.detail) ||
      (typeof o.title === 'string' && o.title);
    if (msg) {
      return msg;
    }
  }

  return fallbackStatusMessage(error);
}

function parseBodyText(text: string): string | null {
  if (!text) {
    return null;
  }
  if (text.startsWith('{')) {
    try {
      const o = JSON.parse(text) as Record<string, unknown>;
      const msg =
        (typeof o.message === 'string' && o.message) ||
        (typeof o.detail === 'string' && o.detail) ||
        (typeof o.title === 'string' && o.title);
      if (msg) {
        return msg;
      }
    } catch {
      /* plain text or non-JSON */
    }
  }
  return text;
}

function fallbackStatusMessage(error: HttpErrorResponse): string {
  if (error.status === 0) {
    return 'Network error. Check that the API is running and CORS is allowed.';
  }
  if (error.statusText) {
    return `${error.statusText} (${error.status})`;
  }
  return `Request failed (${error.status})`;
}
