using AwesomeAssertions;
using NetArchTest.Rules;
using System.Reflection;

namespace Cataloging.ArchitectureTests;

public class LayeredArchitectureTests
{
    private readonly Assembly _assembly;

    public LayeredArchitectureTests()
    {
        _assembly = typeof(ServiceConfigurator).Assembly;
    }

    [Fact]
    public void Assembly_should_not_be_empty()
    {
        Types.InAssembly(_assembly).GetTypes().Should().NotBeEmpty();
    }

    [Fact]
    public void Domain_layer_should_not_depend_on_other_layers()
    {
        Types.InAssembly(_assembly).That().ResideInNamespaceStartingWith("Cataloging.Domain")
            .Should().NotHaveDependencyOn("Cataloging.API")
            .And().NotHaveDependencyOn("Cataloging.Application")
            .And().NotHaveDependencyOn("Cataloging.Infra")
            .GetResult().FailingTypeNames.Should().BeNullOrEmpty();
    }

    [Fact]
    public void Application_layer_should_not_depend_on_infra_or_API()
    {
        Types.InAssembly(_assembly).That().ResideInNamespaceStartingWith("Cataloging.Application")
            .Should().NotHaveDependencyOn("Cataloging.Infra")
            .And().NotHaveDependencyOn("Cataloging.API")
            .GetResult().FailingTypeNames.Should().BeNullOrEmpty();
    }

    [Fact]
    public void Infra_layer_should_not_depend_on_API()
    {
        Types.InAssembly(_assembly).That().ResideInNamespaceStartingWith("Cataloging.Infra")
            .Should().NotHaveDependencyOn("Cataloging.API")
            .GetResult().FailingTypeNames.Should().BeNullOrEmpty();
    }
}