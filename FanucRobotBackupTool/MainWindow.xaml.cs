using FanucRobotBackupTool.Resources;
using System.IO;
using System.Windows;
using Ookii.Dialogs.Wpf;
using System.Windows.Controls;
using System.Windows.Forms;

namespace FanucRobotBackupTool
{
    public partial class MainWindow : Window
    {
        private int _totalFiles = 0;
        private CancellationTokenSource _cts = new();
        private CancellationToken _token => _cts.Token;
        private readonly List<string> _logBuffer = new();
        private readonly object _logLock = new(); // for thread safety

        public MainViewModel ViewModel => (MainViewModel)DataContext;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
            ViewModel.LoadDevices();
        }
        private void AddDeviceButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AddDevice();
        }

        private void RemoveDeviceButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedDevice != null)
                ViewModel.RemoveDevice(ViewModel.SelectedDevice);
        }

        private void DeviceTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is FtpDeviceViewModel device)
                ViewModel.SelectedDevice = device;
        }

        private void DeviceNameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedDevice != null && string.IsNullOrWhiteSpace(DeviceNameTextBox.Text))
            {
                string defaultName = $"Device {ViewModel.Devices.IndexOf(ViewModel.SelectedDevice) + 1}";
                ViewModel.SelectedDevice.Name = $"Device {ViewModel.Devices.IndexOf(ViewModel.SelectedDevice) + 1}";
            }
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new VistaFolderBrowserDialog();
            if (dialog.ShowDialog() == true && ViewModel.SelectedDevice != null)
            {
                ViewModel.SelectedDevice.FilePath = dialog.SelectedPath;
                ViewModel.SaveDevices();
            }
        }

        private async void RunButton_Click(object sender, RoutedEventArgs e)
        {

            SetRunUi(false);
            _totalFiles = 0;
            _cts = new CancellationTokenSource();

            var ftpDevices = ViewModel.Devices.Select(d => d.Device).ToList();

            try
            {
                if (!ftpDevices.Any())
                    throw new OperationAbortException("No devices to validate.");

                await ValidateDeviceData(ftpDevices);
                await ValidateConnections(ftpDevices);
                await RetrieveDeviceFiles(ftpDevices);
                await DownloadDeviceFiles(ftpDevices);
            }
            catch (OperationCanceledException ex)
            {
                WriteLog($"Backup operation was canceled by the user: {ex.Message}");
            }
            catch (OperationAbortException ex)
            {
                WriteLog($"{ex.Message}");
            }
            catch (Exception ex)
            {
                WriteLog($"{ex.Message}");
            }
            finally
            {
                SetRunUi(true);
                WriteLog("");
            }
        }

        private async Task ValidateDeviceData(List<FtpDevice> devices)
        {
            await Task.Run(() =>
            {
                foreach (var device in devices)
                {
                    if (string.IsNullOrWhiteSpace(device.Name))
                        throw new OperationAbortException($"Device name is empty for IP: {device.IpAddress}");

                    if (string.IsNullOrWhiteSpace(device.IpAddress) || !System.Net.IPAddress.TryParse(device.IpAddress, out _))
                        throw new OperationAbortException($"Invalid IP address for device: {device.Name}");

                    if (string.IsNullOrWhiteSpace(device.FilePath) || !Directory.Exists(device.FilePath))
                        throw new OperationAbortException($"Invalid or missing save path for device: {device.Name}");

                    _token.ThrowIfCancellationRequested();
                }
            }, _token);
        }

        private async Task ValidateConnections(List<FtpDevice> devices)
        {
            await Task.Run(() =>
            {
                foreach (var device in devices)
                {
                    try
                    {
                        device.ValidateConnection();
                    }
                    catch (Exception ex)
                    {
                        throw new OperationAbortException($"[{device.Name}] Connection failed - {ex.Message}");
                    }
                    WriteLog($"[{device.Name}] {device.Description}");
                    _token.ThrowIfCancellationRequested();
                }
            }, _token);
        }

        private async Task RetrieveDeviceFiles(List<FtpDevice> devices)
        {
            await Task.Run(() =>
            {
                foreach (FtpDevice device in devices)
                {
                    try
                    {
                        device.RetrieveFiles();

                        if (device.Files.Length == 0)
                            throw new OperationAbortException($"[{device.Name}] No files found.");

                        else
                        {
                            Interlocked.Add(ref _totalFiles, device.Files.Length);
                            WriteLog($"[{device.Name}] {device.Files.Length} file(s) found.");
                        }
                        _token.ThrowIfCancellationRequested();
                    }
                    catch (OperationCanceledException)
                    {
                        throw;
                    }
                    catch (OperationAbortException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"[{device.Name}] Error retrieving files - {ex.Message}");
                    }
                }
                WriteLog($"{_totalFiles} total files found.");
            }, _token);
        }

        private async Task DownloadDeviceFiles(List<FtpDevice> devices)
        {
            int completedFiles = 0;
            bool logFiles = ViewModel.LogFiles;

            await Task.Run(() =>
            {
                foreach (FtpDevice device in devices)
                {
                        WriteLog($"[{device.Name}] Starting download...");
                        foreach (string file in device.Files)
                        {
                            try
                            {
                                device.DownloadFile(file);
                                if (logFiles) WriteLog($"[{device.Name}] Downloaded {file} to {device.FilePath}.");

                                Interlocked.Increment(ref completedFiles);
                                UpdateProgress(completedFiles, _totalFiles);
                                _token.ThrowIfCancellationRequested();
                            }
                            catch (OperationCanceledException)
                            {
                                throw new OperationCanceledException($"{completedFiles} out of {_totalFiles} files downloaded.");
                            }
                            catch (Exception ex)
                            {
                                throw new Exception($"[{device.Name}] Failed to download {file} - {ex.Message}");
                            }
                        }
                        WriteLog($"[{device.Name}] Backup complete.");
                }
                WriteLog("Backup complete!");
            }, _token);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            _cts.Cancel();
            WriteLog("Cancel requested...");
        }

        private void UpdateProgress(int completed, int total)
        {
            Dispatcher.InvokeAsync(() =>
            {
                double percent = (double)completed / total * 100;
                ProgressBar.Value = percent;
                ProgressLabel.Text = $"Progress: {completed} / {total}";
            });
        }

        private void SetRunUi(bool enabled)
        {
            DeviceNameTextBox.IsEnabled = enabled;
            IpAddressTextBox.IsEnabled = enabled;
            DirectoryTextBox.IsEnabled = enabled;
            BrowseButton.IsEnabled = enabled;
            AddButton.IsEnabled = enabled;
            RemoveButton.IsEnabled = enabled;
            LogFilesCheckBox.IsEnabled = enabled;

            RunButton.IsEnabled = enabled;
            RunButton.Visibility = enabled ? Visibility.Visible : Visibility.Hidden;
            CancelButton.IsEnabled = !enabled;
            CancelButton.Visibility = !enabled ? Visibility.Visible : Visibility.Hidden;

            ProgressBar.Value = 0;
            ProgressBar.Visibility = !enabled ? Visibility.Visible : Visibility.Hidden;
            ProgressLabel.Text = "Backup Starting...";
            ProgressLabel.Visibility = !enabled ? Visibility.Visible : Visibility.Hidden;
        }

        public void WriteLog(string message)
        {
            // Store message in buffer safely
            lock (_logLock)
            {
                _logBuffer.Add(message);
            }

            // Tell the UI thread to flush logs soon
            Dispatcher.InvokeAsync(() =>
            {
                lock (_logLock)
                {
                    foreach (var msg in _logBuffer)
                    {
                        LogTextBox.AppendText(msg + Environment.NewLine);
                    }
                    _logBuffer.Clear(); // empty buffer
                    LogTextBox.ScrollToEnd();
                }
            });
        }
    }
}