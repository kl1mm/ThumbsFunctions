using Microsoft.Azure.WebJobs.Description;
using System;

namespace kli.ThumbsFunctions.Infrastructure
{
	[Binding]
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
	public sealed class HttpClientFactoryAttribute : Attribute
	{ }
}
