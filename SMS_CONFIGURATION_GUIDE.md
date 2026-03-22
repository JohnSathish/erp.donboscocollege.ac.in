# SMS Configuration Guide

## Current Configuration

Your SMS settings are configured in `appsettings.json`:

```json
"Notifications": {
  "Sms": {
    "Enabled": true,
    "BaseUrl": "http://164.52.195.161/API/SendMsg.aspx",
    "Username": "20230188",
    "Password": "E6nJu9k9",
    "Sender": "DBTURA",
    "Priority": "1",
    "LoginUrl": "https://admissions.donboscocollege.ac.in/login"
  }
}
```

## SMS Provider Details

- **Provider API**: `http://164.52.195.161/API/SendMsg.aspx`
- **Username**: `20230188`
- **Password**: `E6nJu9k9`
- **Sender ID**: `DBTURA`
- **Priority**: `1` (Normal priority)

## How SMS Sending Works

### Registration SMS
When a student registers, they receive an SMS with:
- Registration confirmation
- Application number
- Login URL
- Username (email)
- Temporary password

**Example SMS Message:**
```
Dear Applicant, Your Registration for Admission at Don Bosco College Tura is Successful. Your Application No is APP001. You can login using the Link https://admissions.donboscocollege.ac.in/login with Username student@example.com and Password TempPass123. Thank You. Management-DBCTURA
```

### Status Update SMS
When application status changes, students receive an SMS notification.

## Testing SMS

### Option 1: Using Swagger UI

1. Open: `http://localhost:5227/swagger`
2. Find `GET /api/test/sms-config` - Check configuration
3. Find `POST /api/test/sms` - Send test SMS

### Option 2: Using PowerShell

**Check SMS Configuration:**
```powershell
Invoke-RestMethod -Uri "http://localhost:5227/api/test/sms-config" -Method Get
```

**Send Test SMS:**
```powershell
$body = @{ MobileNumber = "9876543210" } | ConvertTo-Json
Invoke-RestMethod -Uri "http://localhost:5227/api/test/sms" -Method Post -Body $body -ContentType "application/json"
```

**Note:** Replace `9876543210` with a valid mobile number (10 digits, no spaces or dashes).

## SMS API Parameters

The SMS API is called with these query parameters:

| Parameter | Value | Description |
|-----------|-------|-------------|
| `uname` | `20230188` | Username for authentication |
| `pass` | `E6nJu9k9` | Password for authentication |
| `send` | `DBTURA` | Sender ID |
| `dest` | Mobile number | Destination mobile number |
| `msg` | Message text | SMS message content |
| `priority` | `1` | Message priority |
| `schtm` | Empty | Scheduled time (empty = send now) |

## Updating SMS Configuration

### Update in appsettings.json

Edit `src/server/Api/appsettings.json`:

```json
"Notifications": {
  "Sms": {
    "Enabled": true,
    "BaseUrl": "YOUR_SMS_API_URL",
    "Username": "YOUR_USERNAME",
    "Password": "YOUR_PASSWORD",
    "Sender": "YOUR_SENDER_ID",
    "Priority": "1",
    "LoginUrl": "https://admissions.donboscocollege.ac.in/login"
  }
}
```

### Update via User Secrets (Recommended for Production)

```powershell
cd src/server/Api

# Update SMS credentials
dotnet user-secrets set "Notifications:Sms:Username" "your-username"
dotnet user-secrets set "Notifications:Sms:Password" "your-password"
dotnet user-secrets set "Notifications:Sms:Sender" "YOUR_SENDER"
```

Then restart the backend to load new settings.

## Mobile Number Format

- **Format**: 10 digits (e.g., `9876543210`)
- **No spaces or dashes**: `9876543210` ✅ (not `987-654-3210` or `98765 43210`)
- **Country code**: Not required (assumed by provider)

## SMS Message Limits

- **Character Limit**: Typically 160 characters per SMS
- **Long Messages**: Automatically split into multiple SMS if needed
- **Current Message**: ~150-200 characters (may split into 2 SMS)

## Troubleshooting

### SMS Not Sending

1. **Check Configuration:**
   ```powershell
   Invoke-RestMethod -Uri "http://localhost:5227/api/test/sms-config" -Method Get
   ```

2. **Check Logs:**
   - Look for SMS-related logs in the backend terminal
   - Check for error messages from the SMS provider

3. **Verify Credentials:**
   - Ensure username and password are correct
   - Verify sender ID is approved by provider

4. **Test API Directly:**
   ```powershell
   $url = "http://164.52.195.161/API/SendMsg.aspx?uname=20230188&pass=E6nJu9k9&send=DBTURA&dest=9876543210&msg=Test&priority=1&schtm="
   Invoke-RestMethod -Uri $url -Method Get
   ```

### Common Issues

**Issue: "SMS notifications disabled"**
- **Solution**: Set `"Enabled": true` in SMS settings

**Issue: "No mobile number available"**
- **Solution**: Ensure mobile number is provided during registration

**Issue: "Failed to send SMS"**
- **Solution**: 
  - Check SMS provider API is accessible
  - Verify credentials are correct
  - Check mobile number format
  - Review provider response in logs

## SMS Provider Response

The SMS provider typically returns:
- **Success**: Status code 200 with success message
- **Failure**: Error code with error message

Check backend logs for the exact provider response.

## Security Notes

⚠️ **Important:**
- SMS credentials should be stored securely
- Use User Secrets for production (not appsettings.json)
- Never commit SMS passwords to version control
- Rotate passwords regularly

## Next Steps

1. **Test SMS Configuration:**
   - Use `/api/test/sms-config` to verify settings
   - Use `/api/test/sms` to send a test SMS

2. **Verify Registration Flow:**
   - Register a test student
   - Check if SMS is sent automatically
   - Verify SMS content is correct

3. **Monitor SMS Delivery:**
   - Check backend logs for SMS sending status
   - Verify students receive SMS notifications

## Quick Reference

```powershell
# Check SMS config
Invoke-RestMethod -Uri "http://localhost:5227/api/test/sms-config" -Method Get

# Send test SMS
$body = @{ MobileNumber = "9876543210" } | ConvertTo-Json
Invoke-RestMethod -Uri "http://localhost:5227/api/test/sms" -Method Post -Body $body -ContentType "application/json"
```



















