using Permissions.Domain.Entities;
using Elasticsearch.Net;
using Nest;

namespace Permissions.Infrastructure
{
  public class ElasticsearchService<T> : IElasticsearchService<T> where T : class
  {
    private readonly ElasticClient _elasticClient;

    public ElasticsearchService(ElasticClient elasticClient)
    {
      _elasticClient = elasticClient;
    }

    public async Task<string> CreateDocumentAsync(T document)
    {
      try
      {
        var result = await _elasticClient.IndexDocumentAsync(document);
        Console.WriteLine($"Registro guardado en Elasticsearch. Id:'{result.Id}' Index:'{result.Index}' Rsult:'{result.Result}' Message:'{result.ApiCall}'");

        if (result.IsValid)
        {
          return "Document created successfully";
        }
        else
        {
          return "Document creation failed: " + result.OriginalException?.Message;
        }
      }
      catch (Exception ex)
      {
        return "Document creation failed: " + ex.Message;
      }
    }

  }

}

