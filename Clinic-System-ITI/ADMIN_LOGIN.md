# 🏥 Clinic Management System - Admin Login

## 🔐 Default Admin Credentials

The default admin user must be created manually with the following credentials:

**Email:** `admin@clinic.com`  
**Password:** `Admin123!`

## 🚀 How to Create and Login

### Step 1: Create Default Admin (First Time Setup)

1. **Run the application**
2. **Navigate to the seeder** - Go to `/Seed/CreateAdmin`
3. **Click "Create Admin User"** - This will create the default admin
4. **Verify creation** - You'll see success message with credentials

### Step 2: Login to Admin Dashboard

1. **Navigate to Admin Login** - Go to `/Account/AdminLogin`
2. **Enter the credentials** above
3. **Access Admin Dashboard** - You'll be redirected to the admin dashboard with full management capabilities

## 🛠️ Manual Seeding Endpoints

- **`/Seed/CreateAdmin`** - Create/fix the default admin user
- **`/Seed/CheckAdmin`** - Check if admin user exists and has proper role

## 🎯 Admin Features

Once logged in, you'll have access to:

- **📊 Dashboard** - Overview of system statistics
- **👨‍⚕️ Doctor Management** - Add, edit, view, and manage doctors
- **🏥 Patient Management** - Comprehensive patient records and management
- **📅 Appointment Management** - Schedule and manage appointments
- **⚙️ Admin Profile** - Manage your admin profile and settings
- **🔒 Security Settings** - Password changes and security configurations

## 🔑 Changing Admin Password

After first login, it's recommended to:

1. Go to **Admin Profile** (`/Admin/AdminProfile`)
2. Click **"Change Password"**
3. Set a new secure password

## 🛡️ Security Notes

- The default password should be changed after first login
- Admin has full system access - protect these credentials
- The system uses ASP.NET Core Identity for secure authentication
- Role-based authorization ensures only admins can access admin features

## 📝 Roles in the System

- **Admin** - Full system management access
- **Doctor** - Medical professional access
- **Patient** - Patient portal access

---

**🎉 Your admin dashboard is now ready to use!**
