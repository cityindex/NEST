using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;
using NUnit.Framework;
using Nest.Tests.Integration.Yaml;


namespace Nest.Tests.Integration.Yaml.Create8
{
	public partial class Create8YamlTests
	{	


		[NCrunch.Framework.ExclusivelyUses("ElasticsearchYamlTests")]
		public class Refresh1Tests : YamlTestsBase
		{
			[Test]
			public void Refresh1Test()
			{	

				//do indices.create 
				_body = new {
					settings= new Dictionary<string, object> {
						 { "index.refresh_interval",  "-1" }
					}
				};
				this.Do(()=> this._client.IndicesCreatePut("test_1", _body));

				//do cluster.health 
				this.Do(()=> this._client.ClusterHealthGet(nv=>nv
					.Add("wait_for_status", @"yellow")
				));

				//do create 
				_body = new {
					foo= "bar"
				};
				this.Do(()=> this._client.IndexPost("test_1", "test", "1", _body, nv=>nv
					.Add("op_type", @"create")
				));

				//do search 
				_body = new {
					query= new {
						term= new {
							_id= "1"
						}
					}
				};
				this.Do(()=> this._client.SearchPost("test_1", "test", _body));

				//match _response.hits.total: 
				this.IsMatch(_response.hits.total, 0);

				//do create 
				_body = new {
					foo= "bar"
				};
				this.Do(()=> this._client.IndexPost("test_1", "test", "2", _body, nv=>nv
					.Add("refresh", 1)
					.Add("op_type", @"create")
				));

				//do search 
				_body = new {
					query= new {
						term= new {
							_id= "2"
						}
					}
				};
				this.Do(()=> this._client.SearchPost("test_1", "test", _body));

				//match _response.hits.total: 
				this.IsMatch(_response.hits.total, 1);

			}
		}
	}
}

