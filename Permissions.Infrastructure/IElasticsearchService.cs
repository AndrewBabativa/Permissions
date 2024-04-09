using System.Threading.Tasks;

namespace Permissions.Infrastructure
{
  public interface IElasticsearchService<T> where T : class
  {
    Task<string> CreateDocumentAsync(T document);
  }
}
