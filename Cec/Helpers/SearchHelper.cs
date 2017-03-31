using Cec.Models;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Cec.Helpers
{
    public class SearchHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        private static string searchName = ConfigurationManager.AppSettings["SearchServiceName"];
        private static string searchKey = ConfigurationManager.AppSettings["SearchServiceApiKey"];
        private static SearchServiceClient azureSearchServiceClient = new SearchServiceClient(searchName, new SearchCredentials(searchKey));
        private static SearchIndexClient materialIndexClient = azureSearchServiceClient.Indexes.GetClient("material");

        private static async void DeleteMaterialIndexIfExistsAsync()
        {
            if (await azureSearchServiceClient.Indexes.ExistsAsync("material"))
            {
                await azureSearchServiceClient.Indexes.DeleteAsync("material");
            }
        }

        private static async void CreateMaterialIndexAsync()
        {
            if (!await azureSearchServiceClient.Indexes.ExistsAsync("material"))
            {
                var materialIndex = new Index("material", Material.GetSearchableFields());
                try
                {
                    await azureSearchServiceClient.Indexes.CreateAsync(materialIndex);
                    UploadMaterialAsync(db.Materials);
                }
                catch (IndexBatchException)
                {
                    throw;
                }
            }
        }

        private static async void UploadMaterialAsync(IEnumerable<Material> material)
        {
            var documents = material.Select(m => m.AsSearchDocument());
            var batch = IndexBatch.Upload(documents);
            try
            {
                await materialIndexClient.Documents.IndexAsync(batch);
            }
            catch (IndexBatchException)
            {
                throw;
            }
        }

        private static async void MergeMaterialAsync(IEnumerable<Material> material)
        {
            var documents = material.Select(m => m.AsSearchDocument());
            var batch = IndexBatch.Merge(documents);
            try
            {
                await materialIndexClient.Documents.IndexAsync(batch);
            }
            catch (IndexBatchException)
            {
                throw;
            }
        }

        private static async void DeleteMaterialAsync(IEnumerable<Material> material)
        {
            var documents = material.Select(m => m.AsSearchDocument());
            var batch = IndexBatch.Delete(documents);
            try
            {
                await materialIndexClient.Documents.IndexAsync(batch);
            }
            catch (IndexBatchException)
            {
                throw;
            }
        }
    }
}