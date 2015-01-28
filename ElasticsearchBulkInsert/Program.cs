using System;

namespace ElasticsearchBulkInsert
{
	class Program
	{
		static void Main(string[] args)
		{
			var provider = new ElasticsearchProvider();

			provider.CreateIndexWithAlias();

			Console.WriteLine("### index, type mapping, alias creation complete...");
			Console.ReadLine();

			provider.DoBulkInsert();

			Console.WriteLine("### bulk insert complete...");
			Console.ReadLine();

			provider.UpdateIndexRefreshIntervalTo1S();

			Console.WriteLine("### ready for search...");
			Console.ReadLine();
		}
	}
}
