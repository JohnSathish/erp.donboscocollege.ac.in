# Fix Gmail Authentication Error

## Current Error
```
535: 5.7.8 Username and Password not accepted
```

## Solution: Generate a New Gmail App Password

### Step 1: Generate New App Password

1. **Go to Google Account Settings:**
   - Visit: https://myaccount.google.com/apppasswords
   - Or: Google Account → Security → 2-Step Verification → App passwords

2. **Verify 2-Step Verification is Enabled:**
   - If not enabled, enable it first (required for App Passwords)

3. **Generate App Password:**
   - Select "Mail" as the app
   - Select "Other (Custom name)" → Enter: "ERP System"
   - Click "Generate"
   - **Copy the 16-character password** (format: `xxxx xxxx xxxx xxxx`)

### Step 2: Update appsettings.json

Update the password in `src/server/Api/appsettings.json`:

```json
"Email": {
  "Enabled": true,
  "SmtpHost": "smtp.gmail.com",
  "SmtpPort": 587,
  "UseSsl": true,
  "Username": "mailconnect@donboscocollege.ac.in",
  "Password": "YOUR_NEW_16_CHAR_APP_PASSWORD_HERE",
  "FromAddress": "mailconnect@donboscocollege.ac.in",
  "FromName": "Don Bosco College Tura Admissions"
}
```

**Important:**
- You can include spaces in the password (the code removes them automatically)
- Or remove spaces: `xxxxxxxxxxxxxxxx` (16 characters)
- Make sure it's exactly 16 characters (without spaces)

### Step 3: Restart Backend

1. **Stop the running backend:**
   ```powershell
   # Find the process
   netstat -ano | findstr :5227
   
   # Kill it (replace PID with actual process ID)
   taskkill /PID <PID> /F
   ```

2. **Restart the backend:**
   ```powershell
   cd src/server/Api
   dotnet run --launch-profile http
   ```

3. **Test again:**
   - Go to: `http://localhost:5227/swagger`
   - Test: `POST /api/test/email`

## Alternative: Check Current App Passwords

If you want to see existing App Passwords or revoke old ones:
1. Go to: https://myaccount.google.com/apppasswords
2. Review existing App Passwords
3. Revoke any old/unused ones
4. Generate a fresh one

## Troubleshooting

### If Still Getting Authentication Error:

1. **Verify Account:**
   - Make sure `mailconnect@donboscocollege.ac.in` is a valid Google Workspace account
   - Check if the account can send emails normally

2. **Check Google Workspace Admin Settings:**
   - Admin may need to enable "Less secure app access" (deprecated but sometimes needed)
   - Or ensure App Passwords are enabled for the organization

3. **Try Different Port:**
   - Port 587 (STARTTLS) - Current
   - Port 465 (SSL) - Alternative
   
   If trying port 465, update `appsettings.json`:
   ```json
   "SmtpPort": 465,
   "UseSsl": true
   ```

4. **Verify Password Format:**
   - App Password should be exactly 16 characters
   - Can include spaces (will be removed automatically)
   - Should NOT be your regular Gmail password

5. **Check Account Restrictions:**
   - Make sure the account isn't locked or restricted
   - Verify the account has permission to send emails

## Quick Test After Update

```powershell
# Check configuration
Invoke-RestMethod -Uri "http://localhost:5227/api/test/email-config" -Method Get

# Send test email
$body = @{ toEmail = "your-email@gmail.com" } | ConvertTo-Json
Invoke-RestMethod -Uri "http://localhost:5227/api/test/email" -Method Post -Body $body -ContentType "application/json"
```



















