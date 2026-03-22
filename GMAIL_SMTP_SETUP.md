# Gmail SMTP Setup Guide

This guide will help you configure Gmail SMTP settings for the ERP Admissions Portal.

## Step 1: Enable 2-Step Verification

Gmail requires 2-Step Verification to generate App Passwords.

1. **Go to Google Account Settings**:
   - Visit: https://myaccount.google.com/
   - Sign in with your Gmail account

2. **Enable 2-Step Verification**:
   - Click **Security** in the left sidebar
   - Under **"How you sign in to Google"**, click **2-Step Verification**
   - Follow the prompts to enable it (you'll need your phone)

## Step 2: Generate Gmail App Password

1. **Go to App Passwords**:
   - Visit: https://myaccount.google.com/apppasswords
   - Or: Google Account → Security → 2-Step Verification → App passwords

2. **Create App Password**:
   - Select **"Mail"** as the app
   - Select **"Other (Custom name)"** as the device
   - Enter a name like: **"ERP Admissions Portal"**
   - Click **Generate**

3. **Copy the App Password**:
   - Google will show a 16-character password like: `abcd efgh ijkl mnop`
   - **Copy this password** (you won't see it again!)
   - Format: It will have spaces, but you can copy it with or without spaces

## Step 3: Update appsettings.json

Update the email configuration in `src/server/Api/appsettings.json`:

```json
"Email": {
  "Enabled": true,
  "SmtpHost": "smtp.gmail.com",
  "SmtpPort": 587,
  "UseSsl": true,
  "Username": "your-email@gmail.com",
  "Password": "abcd efgh ijkl mnop",
  "FromAddress": "your-email@gmail.com",
  "FromName": "Don Bosco College Tura Admissions"
}
```

### Example:
```json
"Email": {
  "Enabled": true,
  "SmtpHost": "smtp.gmail.com",
  "SmtpPort": 587,
  "UseSsl": true,
  "Username": "admissions.donbosco@gmail.com",
  "Password": "abcd efgh ijkl mnop",
  "FromAddress": "admissions.donbosco@gmail.com",
  "FromName": "Don Bosco College Tura Admissions"
}
```

## Step 4: Important Notes

### Password Format:
- **App Passwords come with spaces**: `abcd efgh ijkl mnop`
- **The code automatically removes spaces** for Gmail authentication
- You can paste the password **with or without spaces** - both will work

### Security:
- **Never commit** your App Password to Git
- Store it securely
- If compromised, revoke it and generate a new one

### Gmail SMTP Settings:
- **SMTP Host**: `smtp.gmail.com`
- **SMTP Port**: `587` (STARTTLS) or `465` (SSL)
- **Encryption**: TLS/STARTTLS (port 587) or SSL (port 465)
- **Authentication**: Required (use App Password, not regular password)

## Step 5: Test the Configuration

1. **Restart the API server** to load new settings
2. **Register a test applicant** to trigger email sending
3. **Check the logs** for authentication status:
   ```
   Attempting to send email via SMTP. Host: smtp.gmail.com, Port: 587...
   Gmail detected. Processing App Password...
   Authentication successful
   ```

## Troubleshooting

### Error: "535: 5.7.8 Username and Password not accepted"

**Solutions:**
1. ✅ Verify you're using an **App Password**, not your regular Gmail password
2. ✅ Ensure **2-Step Verification** is enabled
3. ✅ Check that the App Password was copied correctly
4. ✅ Make sure `UseSsl: true` is set in `appsettings.json`

### Error: "Connection timeout"

**Solutions:**
1. ✅ Check your internet connection
2. ✅ Verify firewall isn't blocking port 587
3. ✅ Try port 465 instead (change `SmtpPort` to `465`)

### Error: "Less secure app access"

**Note:** Gmail no longer supports "Less secure app access". You **must** use App Passwords with 2-Step Verification enabled.

## Alternative: Port 465 (Direct SSL)

If port 587 doesn't work, you can use port 465:

```json
"Email": {
  "SmtpHost": "smtp.gmail.com",
  "SmtpPort": 465,
  "UseSsl": true,
  ...
}
```

The code automatically detects port 465 and uses direct SSL connection.

## Code Implementation

The code automatically:
- ✅ Detects Gmail SMTP host
- ✅ Removes spaces from App Password
- ✅ Uses STARTTLS for port 587
- ✅ Uses SSL for port 465
- ✅ Logs detailed authentication information

## Quick Reference

| Setting | Value |
|---------|-------|
| SMTP Host | `smtp.gmail.com` |
| SMTP Port | `587` (STARTTLS) or `465` (SSL) |
| Use SSL | `true` |
| Username | Your full Gmail address |
| Password | Gmail App Password (16 characters) |
| From Address | Your Gmail address |

---

**Need Help?** Check the application logs for detailed error messages and authentication status.




















