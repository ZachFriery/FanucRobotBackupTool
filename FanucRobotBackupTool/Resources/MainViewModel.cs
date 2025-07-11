using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace FanucRobotBackupTool.Resources
{
    public class MainViewModel : INotifyPropertyChanged
    {

        private bool _logFiles;
        private bool _isRunning;
        private FtpDeviceViewModel? _selectedDevice;
        public ObservableCollection<FtpDeviceViewModel> Devices { get; set; } = new();

        public FtpDeviceViewModel? SelectedDevice
        {
            get => _selectedDevice;
            set
            {
                if (_selectedDevice != value)
                {
                    _selectedDevice = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool LogFiles
        {
            get => _logFiles;
            set { _logFiles = value; OnPropertyChanged(); }
        }

        public bool IsRunning
        {
            get => _isRunning;
            set { _isRunning = value; }
        }

        public ICommand AddDeviceCmd { get; }
        public ICommand RemoveDeviceCmd { get; }
        public ICommand RunBackupCmd { get; }
        public ICommand CancelBackupCmd { get; }
        public ICommand BrowseCmd { get; }


        public void LoadDevices()
        {
            var loaded = FtpDeviceManager.LoadDevices()
                          .Select(d => new FtpDeviceViewModel(d));
            Devices.Clear();
            foreach (var device in loaded)
                Devices.Add(device);
        }

        public void SaveDevices()
        {
            var models = Devices.Select(d => d.Device).ToList();
            FtpDeviceManager.SaveDevices(models);
        }

        public void AddDevice()
        {
            var newDevice = new FtpDeviceViewModel(new FtpDevice($"Device {Devices.Count + 1}", ""));
            Devices.Add(newDevice);
            SelectedDevice = newDevice;
            SaveDevices();
        }

        public void RemoveDevice(FtpDeviceViewModel device)
        {
            Devices.Remove(device);
            if (SelectedDevice == device)
                SelectedDevice = null;
            SaveDevices();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}