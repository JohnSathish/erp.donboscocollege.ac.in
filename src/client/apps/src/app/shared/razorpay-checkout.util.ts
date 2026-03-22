/**
 * Helpers for Razorpay Standard Checkout (checkout.razorpay.com).
 * Dashboard: enable UPI, UPI Intent, and QR under Payment Methods for the linked MID.
 */

export function isMobileBrowser(): boolean {
  if (typeof navigator === 'undefined') {
    return false;
  }
  const ua = navigator.userAgent || '';
  if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(ua)) {
    return true;
  }
  // iPadOS 13+ reports as MacIntel with touch
  if (navigator.maxTouchPoints > 1 && /MacIntel/.test(ua)) {
    return true;
  }
  return false;
}

export function normalizeIndianContact(contact: string | null | undefined): string {
  if (!contact) {
    return '';
  }
  const digits = contact.replace(/\D/g, '');
  if (!digits) {
    return '';
  }
  if (digits.length === 10) {
    return `+91${digits}`;
  }
  if (digits.length === 12 && digits.startsWith('91')) {
    return `+${digits}`;
  }
  const t = contact.trim();
  if (t.startsWith('+')) {
    return t;
  }
  return `+${digits}`;
}

export interface RazorpayPrefill {
  name?: string;
  email?: string;
  contact?: string;
}

/** Razorpay pre-selects `method` only when email and contact are prefilled (see integration docs). */
export function shouldPreselectUpi(prefill: RazorpayPrefill): boolean {
  if (!isMobileBrowser()) {
    return false;
  }
  const email = prefill.email?.trim();
  const contact = normalizeIndianContact(prefill.contact);
  return !!email && !!contact;
}

export interface BuildRazorpayOptionsParams {
  key: string;
  amountPaise: number;
  currency: string;
  name: string;
  description: string;
  orderId: string;
  handler: (response: Record<string, string>) => void | Promise<void>;
  prefill: RazorpayPrefill;
  themeColor?: string;
  onDismiss?: () => void;
}

/** Options for `new Razorpay(options).open()`. */
export function buildRazorpayStandardOptions(params: BuildRazorpayOptionsParams): Record<string, unknown> {
  const prefill = {
    name: params.prefill.name ?? '',
    email: params.prefill.email ?? '',
    contact: normalizeIndianContact(params.prefill.contact),
  };

  const options: Record<string, unknown> = {
    key: params.key,
    amount: params.amountPaise,
    currency: params.currency,
    name: params.name,
    description: params.description,
    order_id: params.orderId,
    handler: params.handler,
    prefill,
    theme: {
      color: params.themeColor ?? '#1b5e9d',
    },
    modal: {
      ondismiss: () => params.onDismiss?.(),
    },
  };

  if (shouldPreselectUpi(prefill)) {
    options['method'] = 'upi';
  }

  return options;
}
