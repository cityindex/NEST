using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;
using NUnit.Framework;
using Nest.Tests.Integration.Yaml;


namespace Nest.Tests.Integration.Yaml.Index7
{
	public partial class Index7YamlTests
	{	


		[NCrunch.Framework.ExclusivelyUses("ElasticsearchYamlTests")]
		public class Parent1Tests : YamlTestsBase
		{
			[Test]
			public void Parent1Test()
			{	

				//do indices.create 
				_body = new {
					mappings= new {
						test= new {
							_parent= new {
								type= "foo"
							}
						}
					}
				};
				this.Do(()=> this._client.IndicesCreatePut("test_1", _body));

				//do cluster.health 
				this.Do(()=> this._client.ClusterHealthGet(nv=>nv
					.Add("wait_for_status", @"yellow")
				));

				//do index 
				_body = new {
					foo= "bar"
				};
				this.Do(()=> this._client.IndexPost("test_1", "test", "1", _body), shouldCatch: @"/RoutingMissingException/");

				//do index 
				_body = new {
					foo= "bar"
				};
				this.Do(()=> this._client.IndexPost("test_1", "test", "1", _body, nv=>nv
					.Add("parent", 5)
				));

				//do get 
				this.Do(()=> this._client.Get("test_1", "test", "1", nv=>nv
					.Add("parent", 5)
					.Add("fields", new [] {
						@"_parent",
						@"_routing"
					})
				));

				//match _response._id: 
				this.IsMatch(_response._id, 1);

				//match _response.fields._parent: 
				this.IsMatch(_response.fields._parent, 5);

				//match _response.fields._routing: 
				this.IsMatch(_response.fields._routing, 5);

			}
		}
	}
}

