using System;
using ElasticsearchCRUD;

namespace ElasticsearchBulkInsert
{
	public class ElasticsearchMappingTestDto : ElasticsearchMapping
	{
		public override string GetIndexForType(Type type)
		{
			return "testdtos_v1";
		}
	}
}