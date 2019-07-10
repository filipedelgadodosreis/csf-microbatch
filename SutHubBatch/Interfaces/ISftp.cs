using SutHubBatch.Model;

namespace SutHubBatch.Interfaces
{
    public interface ISftp
    {
        void Download(ServiceConfigurations serviceConfigurations);
    }
}
