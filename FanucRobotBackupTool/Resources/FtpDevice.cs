using System.IO;
using System.Net;

namespace FanucRobotBackupTool.Resources
{
    public class FtpDevice
    {
        private string _name = "";
        private string _ipAddress = "";
        private string _filePath = "";
        private string _userName = "";
        private string _password = "";
        private string _description = "";
        private string[] _files = new string[0];

        public string Name
        {
            get => _name;
            set { _name = value; }
        }

        public string IpAddress
        {
            get => _ipAddress;
            set { _ipAddress = value; }
        }

        public string FilePath
        {
            get => _filePath;
            set { _filePath = value; }
        }

        public string UserName
        {
            get => _userName;
            set { _userName = value; }
        }

        public string Password
        {
            get => _password;
            set { _password = value; }
        }

        public string Description
        {
            get => _description;
            set { _description = value; }
        }

        public string[] Files
        {
            get => _files;
            set { _files = value; }
        }

        public event Action<string>? LogMessage;

        public FtpDevice() {}

        public FtpDevice(string name, string ipAddress)
        {
            this.Name = name;
            this.IpAddress = ipAddress;
        }

        public FtpDevice(string name, string ipAddress, string userName, string password)
        {
            this.Name = name;
            this.IpAddress = ipAddress;
            this.UserName = userName;
            this.Password = password;
        }

        public void ValidateConnection()
        {
            string ftpServer = $"ftp://{IpAddress}/";

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpServer);
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            request.UsePassive = true;
            request.UseBinary = true;
            request.KeepAlive = false;

            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
                request.Credentials = new NetworkCredential(UserName, Password);

            using FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Description = $"{trimMessage(response.BannerMessage)}";
        }

        public void RetrieveFiles()
        {
            Files = new string[0];
            string ftpServer = $"ftp://{IpAddress}";

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpServer);
            request.Method = WebRequestMethods.Ftp.ListDirectory;

            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
                request.Credentials = new NetworkCredential(UserName, Password);

            request.UsePassive = true;
            request.UseBinary = true;
            request.KeepAlive = false;

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            using (Stream responseStream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(responseStream))
            {
                string fileList = reader.ReadToEnd();
                Files = fileList.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            }
        }

        public void DownloadFile(string fileName)
        {
            string ftpServer = $"ftp://{IpAddress}";
            string fileUri = ftpServer + "/" + fileName.TrimStart('/');
            string localPath = Path.Combine(FilePath, fileName);

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(fileUri);
            request.Method = WebRequestMethods.Ftp.DownloadFile;

            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
                request.Credentials = new NetworkCredential(UserName, Password);

            request.UseBinary = true;
            request.UsePassive = true;
            request.KeepAlive = false;

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            using (Stream responseStream = response.GetResponseStream())
            using (FileStream fileStream = new FileStream(localPath, FileMode.Create))
            {
                responseStream.CopyTo(fileStream);
            }
        }

        private string trimMessage(string message)
        {
            char[] chars = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            return message.TrimStart(chars).Trim();
        }

        public override string ToString() => Name;

    }

}