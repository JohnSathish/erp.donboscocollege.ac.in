# Email Configuration Guide

This guide explains how to configure email sending for the Don Bosco College ERP Admissions Portal.

## Configuration File

Email settings are configured in `src/server/Api/appsettings.json` under the `Notifications:Email` section:

```json
{
  "Notifications": {
    "Email": {
      "Enabled": true,
      "SmtpHost": "smtp.gmail.com",
      "SmtpPort": 587,
      "UseSsl": true,
      "Username": "your-email@gmail.com",
      "Password": "your-app-password",
      "FromAddress": "admissions@donboscocollege.ac.in",
      "FromName": "Don Bosco College Tura Admissions"
    }
  }
}
```

## Gmail Configuration

### Step 1: Enable 2-Step Verification
1. Go to your Google Account settings
2. Navigate to Security → 2-Step Verification
3. Enable 2-Step Verification

### Step 2: Generate App Password
1. Go to Google Account → Security
2. Under "2-Step Verification", click "App passwords"
3. Select "Mail" and "Other (Custom name)"
4. Enter "Don Bosco ERP" as the name
5. Click "Generate"
6. Copy the 16-character password (use this in `Password` field)

### Step 3: Update Configuration
```json
{
  "Notifications": {
    "Email": {
      "Enabled": true,
      "SmtpHost": "smtp.gmail.com",
      "SmtpPort": 587,
      "UseSsl": true,
      "Username": "your-email@gmail.com",
      "Password": "xxxx xxxx xxxx xxxx",
      "FromAddress": "your-email@gmail.com",
      "FromName": "Don Bosco College Tura Admissions"
    }
  }
}
```

## Other SMTP Providers

### Outlook/Office 365
```json
{
  "SmtpHost": "smtp.office365.com",
  "SmtpPort": 587,
  "UseSsl": true,
  "Username": "your-email@outlook.com",
  "Password": "your-password"
}
```

### Custom SMTP Server
```json
{
  "SmtpHost": "mail.yourdomain.com",
  "SmtpPort": 587,
  "UseSsl": true,
  "Username": "admissions@yourdomain.com",
  "Password": "your-password"
}
```

## Security Best Practices

### Using User Secrets (Recommended for Development)

For local development, store sensitive credentials in user secrets instead of `appsettings.json`:

```bash
dotnet user-secrets init --project src/server/Api
dotnet user-secrets set "Notifications:Email:Username" "your-email@gmail.com" --project src/server/Api
dotnet user-secrets set "Notifications:Email:Password" "your-app-password" --project src/server/Api
```

### Using Environment Variables (Recommended for Production)

Set environment variables:
```bash
Notifications__Email__Username=your-email@gmail.com
Notifications__Email__Password=your-app-password
```

Or in Azure App Service:
- Go to Configuration → Application settings
- Add:
  - `Notifications:Email:Username`
  - `Notifications:Email:Password`

## Testing Email Configuration

1. Start the API server
2. Register a new applicant
3. Check the application logs for email sending status
4. Verify the email is received

## Email Templates

The system sends HTML-formatted emails for:
- **Registration Confirmation**: Sent when a new applicant registers
- **Application Submission**: Sent when an application is submitted (includes PDF attachment)
- **Status Updates**: Sent when application status changes (Approved, Rejected, Waitlisted, Entrance Exam)
- **Password Change**: Sent when password is changed

All emails include:
- Professional HTML formatting
- College branding
- Clear call-to-action buttons
- Responsive design

## Troubleshooting

### Email Not Sending

1. **Check Logs**: Look for email-related errors in application logs
2. **Verify Settings**: Ensure `Enabled` is `true` and all required fields are filled
3. **Test SMTP Connection**: Verify SMTP host and port are correct
4. **Check Credentials**: Ensure username and password are correct
5. **Firewall**: Ensure port 587 (or your SMTP port) is not blocked

### Common Errors

- **"Authentication failed"**: Check username/password, ensure App Password is used for Gmail
- **"Connection timeout"**: Verify SMTP host and port, check firewall settings
- **"SSL/TLS error"**: Ensure `UseSsl` matches your SMTP server requirements

## Disabling Email Notifications

To disable email sending (useful for testing):

```json
{
  "Notifications": {
    "Email": {
      "Enabled": false
    }
  }
}
```

When disabled, emails will be logged but not sent.




