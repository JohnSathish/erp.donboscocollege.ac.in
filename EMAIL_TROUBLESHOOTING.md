# Email Sending Troubleshooting Guide

## Quick Test Endpoints

After rebuilding and restarting the backend, use these endpoints to test email:

### 1. Check Email Configuration
```bash
GET http://localhost:5227/api/test/email-config
```

### 2. Send Test Email
```bash
POST http://localhost:5227/api/test/email
Content-Type: application/json

{
  "toEmail": "your-email@example.com"
}
```

Or use Swagger UI: `http://localhost:5227/swagger`

## Current Configuration

Based on your `appsettings.json`:
- **SMTP Host**: `smtp.gmail.com`
- **Port**: `587`
- **SSL**: `true`
- **Username**: `mailconnect@donboscocollege.ac.in`
- **From Address**: `mailconnect@donboscocollege.ac.in`

## Common Issues & Solutions

### Issue 1: Gmail App Password Not Working

**Symptoms:**
- Authentication error: "535: 5.7.8 Username and Password not accepted"
- "BadCredentials" error

**Solutions:**

1. **Verify App Password is Correct:**
   - Go to: https://myaccount.google.com/apppasswords
   - Make sure 2-Step Verification is enabled
   - Generate a NEW App Password specifically for "Mail"
   - Copy the 16-character password (format: `xxxx xxxx xxxx xxxx`)
   - Update `appsettings.json` with the new password

2. **Check Password Format:**
   - The code automatically removes spaces for Gmail
   - Make sure you're using the App Password, NOT your regular Gmail password
   - App Passwords are 16 characters (with or without spaces)

3. **Verify Account Settings:**
   - Ensure "Less secure app access" is NOT needed (it's deprecated)
   - Use App Passwords instead
   - Make sure the account has 2-Step Verification enabled

### Issue 2: Google Workspace Account Issues

**For `mailconnect@donboscocollege.ac.in`:**

1. **Check Admin Settings:**
   - Google Workspace admin may need to enable "Less secure apps" (if using older method)
   - OR ensure App Passwords are enabled for the organization

2. **Verify Account Permissions:**
   - The account must have permission to send emails
   - Check if there are any restrictions on the account

3. **Try Different Port:**
   - Port 587 (STARTTLS) - Current setting
   - Port 465 (SSL) - Alternative, requires `UseSsl: true` and port 465

### Issue 3: Hostinger SMTP Settings

If you want to use Hostinger instead:

```json
"Email": {
  "Enabled": true,
  "SmtpHost": "smtp.hostinger.com",
  "SmtpPort": 465,
  "UseSsl": true,
  "Username": "your-email@yourdomain.com",
  "Password": "your-email-password",
  "FromAddress": "your-email@yourdomain.com",
  "FromName": "Don Bosco College Tura Admissions"
}
```

**Hostinger SMTP Settings:**
- **SMTP Host**: `smtp.hostinger.com`
- **Port**: `465` (SSL) or `587` (STARTTLS)
- **SSL/TLS**: Required
- **Username**: Your full email address
- **Password**: Your email account password (NOT an app password)

### Issue 4: Network/Firewall Issues

**Symptoms:**
- Connection timeout
- "Unable to connect to remote server"

**Solutions:**
1. Check if port 587 or 465 is blocked by firewall
2. Try from a different network
3. Check if your ISP blocks SMTP ports

### Issue 5: SSL Certificate Issues

The code already disables SSL certificate validation for development. If you still have issues:
- Check the logs for SSL-related errors
- Verify the SMTP server's SSL certificate is valid

## Step-by-Step Debugging

### Step 1: Check Configuration
```bash
GET http://localhost:5227/api/test/email-config
```

Verify:
- ✅ `enabled` is `true`
- ✅ `smtpHost` is correct
- ✅ `smtpPort` matches your provider (587 for Gmail, 465 for Hostinger SSL)
- ✅ `useSsl` is `true`
- ✅ `passwordConfigured` is `true`
- ✅ `passwordLength` is correct (16 for Gmail App Password, or your email password length)

### Step 2: Send Test Email
```bash
POST http://localhost:5227/api/test/email
Content-Type: application/json

{
  "toEmail": "your-test-email@gmail.com"
}
```

### Step 3: Check Backend Logs

Look for these log entries:
- `=== EMAIL TEST STARTED ===`
- `Attempting to send email via SMTP`
- `Connecting to SMTP server`
- `Authenticating with username`
- `Available authentication mechanisms`
- `Authentication successful` or error details

### Step 4: Common Error Messages

**"535: 5.7.8 Username and Password not accepted"**
- Wrong password
- Not using App Password (for Gmail)
- App Password expired or revoked

**"530: 5.7.0 Must issue a STARTTLS command first"**
- Port/SSL mismatch
- Try port 587 with `UseSsl: true`

**"Connection timeout"**
- Firewall blocking port
- Wrong SMTP host
- Network issues

**"No authentication mechanisms available"**
- Server doesn't support authentication
- Connection issue

## Testing Checklist

- [ ] Backend is running on `http://localhost:5227`
- [ ] Configuration is loaded correctly (`/api/test/email-config`)
- [ ] Test email endpoint accessible (`/api/test/email`)
- [ ] Gmail App Password is valid and current
- [ ] 2-Step Verification is enabled on Gmail account
- [ ] Port 587 is not blocked by firewall
- [ ] Check backend logs for detailed error messages

## Next Steps (Using Terminal/Cursor)

### Step 1: Build the Backend
```powershell
cd src/server/Api
dotnet build
```

### Step 2: Run the Backend
```powershell
dotnet run --launch-profile http
```

Or with hot reload (auto-restarts on changes):
```powershell
dotnet watch run --launch-profile http
```

### Step 3: Test Email Configuration

**Option A: Using Swagger UI**
1. Open browser: `http://localhost:5227/swagger`
2. Find `GET /api/test/email-config` - Click "Try it out" → "Execute"
3. Find `POST /api/test/email` - Click "Try it out", enter email, click "Execute"

**Option B: Using PowerShell**
```powershell
# Check configuration
Invoke-RestMethod -Uri "http://localhost:5227/api/test/email-config" -Method Get

# Send test email
$body = @{ toEmail = "your-email@gmail.com" } | ConvertTo-Json
Invoke-RestMethod -Uri "http://localhost:5227/api/test/email" -Method Post -Body $body -ContentType "application/json"
```

### Step 4: Check Logs
Watch the terminal output where you ran `dotnet run` - all logs will appear there.

### Step 5: Share Results
If still not working, share:
- The response from `/api/test/email-config`
- The response from `/api/test/email`
- Any error messages from the terminal logs

## Alternative: Use a Different Email Provider

If Gmail/Google Workspace continues to have issues, consider:

1. **SendGrid** (Free tier: 100 emails/day)
2. **Mailgun** (Free tier: 5,000 emails/month)
3. **Amazon SES** (Very cheap, pay-as-you-go)
4. **Hostinger Email** (If you have hosting with them)

Let me know which provider you'd like to configure!

