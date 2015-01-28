using System;
using System.Collections.Generic;
using ElasticsearchCRUD;
using ElasticsearchCRUD.ContextAddDeleteUpdate.IndexModel.SettingsModel;
using ElasticsearchCRUD.Tracing;

namespace ElasticsearchBulkInsert
{
	public class ElasticsearchProvider
	{
		private const string ConnectionString = "http://localhost:9200/";
		private readonly ElasticsearchContext _elasticsearchContext;

		public ElasticsearchProvider()
		{
			_elasticsearchContext = new ElasticsearchContext(ConnectionString, new ElasticsearchMappingResolver());	
		}

		public void CreateIndexWithAlias()
		{		
			IElasticsearchMappingResolver elasticsearchMappingResolver = new ElasticsearchMappingResolver();
			elasticsearchMappingResolver.AddElasticSearchMappingForEntityType(typeof(TestDto), new ElasticsearchMappingTestDto());
			using (var context = new ElasticsearchContext( ConnectionString, new ElasticsearchSerializerConfiguration(elasticsearchMappingResolver, true, true)))
			{
				context.TraceProvider = new ConsoleTraceProvider();

				context.IndexCreate<TestDto>(
					new IndexDefinition
					{
						IndexAliases = new IndexAliases
						{
							Aliases = new List<IndexAlias>
							{
								// alais maps to default index name
								new IndexAlias("testdtos")
							}
						}, 
						IndexSettings = new IndexSettings{RefreshInterval="-1", NumberOfReplicas = 0}
					}
				);
			}
		}

		public void DoBulkInsert()
		{
			// Add a million records
			long id = 1;
			for (int i = 0; i < 100; i++)
			{
				for (int t = 0; t < 10000; t++)
				{
					var item = new TestDto
					{
						Id = id,
						Description = "this is cool",
						Info = "info"
					};
					_elasticsearchContext.AddUpdateDocument(item, item.Id);
					id++;
				}
				// add data ...
				_elasticsearchContext.SaveChanges();
				Console.WriteLine("Saved:" + (i + 1) * 10000 + " items");
			}
		}

		public void UpdateIndexRefreshIntervalTo1S()
		{
			_elasticsearchContext.IndexUpdateSettings
			(
				new IndexUpdateSettings
				{
					RefreshInterval = "1s",
				    NumberOfReplicas = 1
				}
			);
		}
	}
}
