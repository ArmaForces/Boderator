using ArmaForces.Boderator.Core.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ArmaForces.Boderator.BotService.Tests.TestUtilities.TestFixtures;

internal class TestApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.ConfigureServices(
            x => x.AddOrReplaceSingleton(new TestConfigurationFactory().CreateConfiguration()));
    }
}
