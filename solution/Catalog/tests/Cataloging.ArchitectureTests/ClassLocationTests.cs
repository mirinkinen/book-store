using FluentAssertions;
using NetArchTest.Rules;
using System.Reflection;

namespace Cataloging.ArchitectureTests;

public class LayerTests
{
    private readonly Assembly _assembly;

    public LayerTests()
    {
        _assembly = typeof(ServiceConfigurator).Assembly;
    }

    [Fact]
    public void Assembly_should_not_be_empty()
    {
        Types.InAssembly(_assembly).GetTypes().Should().NotBeEmpty();
    }

    [Fact]
    public void Repository_interfaces_should_reside_in_domain_layer()
    {
        Types.InAssembly(_assembly).That().AreInterfaces().And().HaveNameMatching(".*Repository")
            .Should().ResideInNamespaceStartingWith("Cataloging.Domain")
            .GetResult().FailingTypeNames.Should().BeNullOrEmpty();
    }
    
    [Fact]
    public void Repository_implementations_should_reside_in_infra_layer()
    {
        Types.InAssembly(_assembly).That().AreClasses().And().HaveNameMatching(".*Repository")
            .Should().ResideInNamespaceStartingWith("Cataloging.Infra")
            .GetResult().FailingTypeNames.Should().BeNullOrEmpty();
    }
    
    [Fact]
    public void Query_interfaces_should_resided_in_application_layer()
    {
        Types.InAssembly(_assembly).That().AreInterfaces().And().HaveNameMatching(".*Queries")
            .Should().ResideInNamespaceStartingWith("Cataloging.Application")
            .GetResult().FailingTypeNames.Should().BeNullOrEmpty();
    }
}