# Gmail/Google Workspace Authentication Troubleshooting

## Error: "535: 5.7.8 Username and Password not accepted"

This error means Google is rejecting your credentials. Follow these steps:

## Step 1: Verify App Password is Correct

1. **Go to Google Account App Passwords**:
   - Visit: https://myaccount.google.com/apppasswords
   - Sign in with: `mailconnect@donboscocollege.ac.in`

2. **Check if App Password exists**:
   - Look for an App Password named "ERP Admissions Portal" or similar
   - If it doesn't exist, create a new one

3. **Generate a NEW App Password**:
   - Select **"Mail"** as the app
   - Select **"Other (Custom name)"** as device
   - Name: **"ERP Admissions Portal"**
   - Click **Generate**
   - **Copy the 16-character password** (format: `abcd efgh ijkl mnop`)

4. **Update appsettings.json**:
   ```json
   "Password": "NEW_APP_PASSWORD_HERE"
   ```

## Step 2: Verify 2-Step Verification is Enabled

**Required**: App Passwords only work if 2-Step Verification is enabled.

1. Go to: https://myaccount.google.com/security
2. Under **"How you sign in to Google"**, verify **2-Step Verification** is **ON**
3. If it's OFF, enable it first, then generate a new App Password

## Step 3: Verify Account Permissions

For **Google Workspace** accounts, check:

1. **Admin Console** (if you're an admin):
   - Go to: https://admin.google.com
   - Navigate to: **Apps** → **Google Workspace** → **Gmail** → **End user access**
   - Ensure **"Allow users to enable 'Less secure app access'"** is enabled (if available)
   - Or ensure **"Allow users to manage their access to less secure apps"** is enabled

2. **User Settings**:
   - Go to: https://myaccount.google.com/security
   - Check if there are any restrictions on your account

## Step 4: Verify App Password Format

Gmail App Passwords are **exactly 16 characters** (with or without spaces):

- **With spaces**: `mhba uaxl ozqn zgpx` (19 characters total)
- **Without spaces**: `mhbauaxlozqnzgpx` (16 characters)

The code automatically removes spaces, so both formats work.

## Step 5: Check Logs After Restart

After restarting the API server, check the logs for:

```
Authenticating with username: mailconnect@donboscocollege.ac.in, Original password length: 19, Processed password length: 16, Password preview (first 4 chars): mhba****
```

**Expected values**:
- Original password length: **19** (with spaces) or **16** (without spaces)
- Processed password length: **16** (spaces removed)
- Password preview: First 4 characters of your App Password

## Step 6: Common Issues

### Issue: "App Password doesn't work"
**Solution**: 
- Generate a **NEW** App Password
- Make sure you're copying the entire 16-character password
- Don't include any extra spaces or characters

### Issue: "2-Step Verification not enabled"
**Solution**:
- Enable 2-Step Verification first
- Then generate a new App Password

### Issue: "Account is locked or restricted"
**Solution**:
- Check Google Workspace admin settings
- Verify the account has SMTP access enabled
- Contact your Google Workspace administrator

### Issue: "Password was revoked"
**Solution**:
- If you revoked the App Password, generate a new one
- Update `appsettings.json` with the new password
- Restart the API server

## Step 7: Test Configuration

1. **Restart API server** (important!)
2. **Register a test applicant**
3. **Check logs** for authentication status

### Success Logs:
```
Gmail detected. Processing App Password: Original length: 19, Processed length: 16
Authenticating with username: mailconnect@donboscocollege.ac.in...
Available authentication mechanisms: LOGIN, PLAIN, XOAUTH2...
Authentication successful
Email notification sent to...
```

### Failure Logs:
```
Authentication failed. Username: mailconnect@donboscocollege.ac.in...
ErrorMessage: 535: 5.7.8 Username and Password not accepted
```

## Step 8: Alternative: Use OAuth2 (Advanced)

If App Passwords don't work, you can use OAuth2:
- More secure
- Requires additional setup
- See Google OAuth2 documentation

## Quick Checklist

- [ ] 2-Step Verification is **ENABLED**
- [ ] App Password is **NEWLY GENERATED** (not old/revoked)
- [ ] App Password is **16 characters** (spaces removed = 16)
- [ ] Username matches: `mailconnect@donboscocollege.ac.in`
- [ ] `appsettings.json` has correct password
- [ ] API server was **RESTARTED** after updating config
- [ ] Logs show password is being processed correctly

## Still Not Working?

1. **Generate a completely new App Password**
2. **Double-check** you're copying the entire password
3. **Verify** 2-Step Verification is enabled
4. **Check** Google Workspace admin settings (if applicable)
5. **Review** the detailed logs for password length and format

---

**Note**: The code automatically removes spaces from Gmail App Passwords, so you can paste the password with or without spaces in `appsettings.json`.




















