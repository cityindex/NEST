using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;
using NUnit.Framework;
using Nest.Tests.Integration.Yaml;


namespace Nest.Tests.Integration.Yaml.Mget10
{
	public partial class Mget10YamlTests
	{	


		[NCrunch.Framework.ExclusivelyUses("ElasticsearchYamlTests")]
		public class RealtimeRefresh1Tests : YamlTestsBase
		{
			[Test]
			public void RealtimeRefresh1Test()
			{	

				//do indices.create 
				_body = new {
					settings= new {
						index= new {
							refresh_interval= "-1",
							number_of_replicas= "0"
						}
					}
				};
				this.Do(()=> this._client.IndicesCreatePut("test_1", _body));

				//do cluster.health 
				this.Do(()=> this._client.ClusterHealthGet(nv=>nv
					.Add("wait_for_status", @"green")
				));

				//do index 
				_body = new {
					foo= "bar"
				};
				this.Do(()=> this._client.IndexPost("test_1", "test", "1", _body));

				//do mget 
				_body = new {
					ids= new [] {
						"1"
					}
				};
				this.Do(()=> this._client.MgetPost("test_1", "test", _body, nv=>nv
					.Add("realtime", 0)
				));

				//is_false _response.docs[0].found; 
				this.IsFalse(_response.docs[0].found);

				//do mget 
				_body = new {
					ids= new [] {
						"1"
					}
				};
				this.Do(()=> this._client.MgetPost("test_1", "test", _body, nv=>nv
					.Add("realtime", 1)
				));

				//is_true _response.docs[0].found; 
				this.IsTrue(_response.docs[0].found);

				//do mget 
				_body = new {
					ids= new [] {
						"1"
					}
				};
				this.Do(()=> this._client.MgetPost("test_1", "test", _body, nv=>nv
					.Add("realtime", 0)
					.Add("refresh", 1)
				));

				//is_true _response.docs[0].found; 
				this.IsTrue(_response.docs[0].found);

			}
		}
	}
}

