using System.IO;
using System.Text.Json;

namespace FanucRobotBackupTool.Resources
{
    class FtpDeviceManager
    {

        private const string FilePath = "DeviceList.json";

        public static void SaveDevices(List<FtpDevice> devices)
        {
            var json = JsonSerializer.Serialize(devices, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }

        public static List<FtpDevice> LoadDevices()
        {
            if (!File.Exists(FilePath))
                return new List<FtpDevice>();

            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<FtpDevice>>(json) ?? new List<FtpDevice>();
        }
    }
}
