using Renci.SshNet;
using Renci.SshNet.Sftp;
using SutHubBatch.Interfaces;
using SutHubBatch.Model;
using System;
using System.IO;

namespace SutHubBatch.Service
{
    public class SftpServiceImplementation : ISftp
    {
        public void Download(ServiceConfigurations serviceConfigurations)
        {
            var connectionInfo = new ConnectionInfo(serviceConfigurations.Host, serviceConfigurations.Username, new PasswordAuthenticationMethod(serviceConfigurations.Username, serviceConfigurations.Password));

            using (var client = new SftpClient(connectionInfo))
            {
                client.Connect();
                DownloadDirectory(client, serviceConfigurations.Source, serviceConfigurations.Destination);
            }
        }

        private static void DownloadDirectory(SftpClient client, string source, string destination)
        {
            var files = client.ListDirectory(source);

            foreach (var file in files)
            {
                if (!file.IsDirectory && !file.IsSymbolicLink)
                {
                    DownloadFile(client, file, destination);
                }
                else if (file.IsSymbolicLink)
                {
                    Console.WriteLine("Symbolic link {0}", file.FullName);
                }
                else if (file.Name != "." && file.Name != "..")
                {
                    var dir = Directory.CreateDirectory(Path.Combine(destination, file.Name));
                    DownloadDirectory(client, file.FullName, dir.FullName);
                }
            }
        }

        private static void DownloadFile(SftpClient client, SftpFile file, string directory)
        {
            Console.WriteLine("Downloading {0}", file.FullName);

            using (Stream fileStream = File.OpenWrite(Path.Combine(directory, file.Name)))
            {
                client.DownloadFile(file.FullName, fileStream);
            }
        }
    }
}
