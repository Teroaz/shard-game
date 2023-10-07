using Microsoft.AspNetCore.Mvc.Testing;
using Shard.Shared.Web.IntegrationTests;
using Shard.Web.ImplementationAPI;
using Xunit.Abstractions;

namespace Shard.IntegrationTests;

public class IntegrationTests : BaseIntegrationTests<Program>
{
    public IntegrationTests(
        WebApplicationFactory<Program> factory,
        ITestOutputHelper testOutputHelper)
        : base(factory, testOutputHelper)
    {
    }
}