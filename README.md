# FanucRobotBackupTool

**FanucRobotBackupTool** is a Windows desktop application built with WPF (Windows Presentation Foundation) for managing and automating file backups from multiple Fanuc robots via FTP.

---

## 🚀 Features

- ✅ **Add, edit, and remove FTP devices**
- 📁 **Browse and download robot files** via FTP
- 📊 **Asynchronous file transfer** with real-time progress tracking
- 🪵 **Detailed log output** with batched UI logging for performance
- 💾 **Persistent device list** saved in JSON between sessions
- 🔍 **TreeView-based interface** for easy navigation and selection
- 🛑 **Task cancellation** for canceling an active backup
- 🔒 **MVVM architecture** with hybrid code-behind logic for separation of concerns

---

## 🖼️ Screenshots

<img width="781" height="439" alt="image" src="https://github.com/user-attachments/assets/df5ae2a5-084d-42d8-b08d-c2617210a3aa" />

---

## 📦 Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/ZachFriery/FanucRobotBackupTool.git

2. Open the solution in Visual Studio.

3. Build the project and run the application or compile as stand alone exe. (Ensure .NET 6.0 or higher is installed.)

## ⚙️ Usage

1. Launch the app.

2. Add/Remove robots from the Device Panel.
   - Click device then press "Remove" to delete.
   - After adding a new device, specify the Name, IP Address, & Save Path, by clicking on the device in the tree and editing the data in the text fields.

<img width="778" height="131" alt="image" src="https://github.com/user-attachments/assets/92bc2754-3361-4dd1-9440-b11b65fc3be1" />


4. Click Run to retrieve all files from each device (check "Log Files" before running to see a log of every file downloaded).

5. Monitor progress via the Progress Bar and Log Window. If a failure occurs, read the Log Window carefully for a description of the error. 

## 📁 File Structure

```
FanucRobotBackupTool
|
├── MainWindow.xaml.cs
|
├── Views
|   └── MainWindow.xaml         # Main WPF view
|
├── Devices
|   └── FtpDevice.cs            # FTP device model with connection validation
|   └── FtpDeviceManager.cs     # Json serialization of FTP device data
|
├── ViewModels
|   └── MainViewModel.cs        # Core ViewModel
|   └── FtpDeviceViewModel.cs   # Ftp device tree view model
```
## 🔧 Settings and Configuration
Devices are persisted in a local JSON file (DeivceList.json)

FTP timeout and error handling is built-in

Uses Ookii.Dialogs.Wpf for enhanced folder selection dialogs

## 🛠️ Dependencies
.NET 6.0+

Ookii.Dialogs.Wpf

## ✍️ Future Improvements
🗂️ Enhanced file management with device list import/export
🔐 Secure FTP (FTPS) support with server user/password
🌐 Device discovery on local network
🧪 Unit tests and diagnostics panel

## 🤝 Contributing
Contributions are welcome! Please fork the repo, create a branch, and submit a pull request.

## 📄 License
This project is licensed under the MIT License. See the LICENSE file for details.

## 👨‍💻 Author
Zachary Friery
FanucRobotBackupTool – Built with love for automation... and sitting at my desk. 
