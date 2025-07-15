using System.ComponentModel;
using System.Runtime.CompilerServices;
using FanucRobotBackupTool.Devices;

namespace FanucRobotBackupTool.ViewModels
{
    public class FtpDeviceViewModel : INotifyPropertyChanged
    {
        public FtpDevice Device { get; }

        public string Name
        {
            get => Device.Name;
            set { Device.Name = value; OnPropertyChanged(); }
        }

        public string IpAddress
        {
            get => Device.IpAddress;
            set { Device.IpAddress = value; OnPropertyChanged(); }
        }

        public string FilePath
        {
            get => Device.FilePath;
            set { Device.FilePath = value; OnPropertyChanged(); }
        }

        public FtpDeviceViewModel(FtpDevice device) => Device = device;

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
