using Codecaine.Common.Storage.Providers.Abstractions;

namespace Codecaine.Common.Storage
{
    public interface IStorageProviderFactory
    {
        IStorageProvider CreateProvider<T>(T provider) where T : Provider;
        
    }
}
