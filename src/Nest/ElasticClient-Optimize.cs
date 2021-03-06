﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nest
{
	public partial class ElasticClient
	{

		public IShardsOperationResponse Optimize(Func<OptimizeDescriptor, OptimizeDescriptor> optimizeSelector = null)
		{
			optimizeSelector = optimizeSelector ?? (s => s);
			return this.Dispatch<OptimizeDescriptor, OptimizeQueryString, ShardsOperationResponse>(
				optimizeSelector,
				(p,d) => this.RawDispatch.IndicesOptimizeDispatch(p)
			);
		}

		public Task<IShardsOperationResponse> OptimizeAsync(Func<OptimizeDescriptor, OptimizeDescriptor> optimizeSelector = null)
		{
			optimizeSelector = optimizeSelector ?? (s => s);
			return this.DispatchAsync<OptimizeDescriptor, OptimizeQueryString, ShardsOperationResponse, IShardsOperationResponse>(
				optimizeSelector,
				(p,d) => this.RawDispatch.IndicesOptimizeDispatchAsync(p)
			);

		}
	}
}
