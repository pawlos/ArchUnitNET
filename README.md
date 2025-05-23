<img src="Logo/ArchUnitNET-Logo.png" height="64" alt="ArchUnit">

# ArchUnitNET [![Build Status](https://github.com/TNG/ArchUnitNET/actions/workflows/build.yaml/badge.svg)](https://github.com/TNG/ArchUnitNET/actions/workflows/build.yaml) [![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](https://github.com/TNG/ArchUnitNET/blob/master/LICENSE) [![Nuget](https://img.shields.io/nuget/v/TngTech.ArchUnitNET)](https://www.nuget.org/packages/TngTech.ArchUnitNET/) [![codecov](https://codecov.io/gh/TNG/ArchUnitNET/graph/badge.svg?token=HgiEXfA0K7)](https://codecov.io/gh/TNG/ArchUnitNET)

Visit our documentation at https://archunitnet.readthedocs.io/en/latest/.

ArchUnitNET is a free, simple library for checking the architecture of C# code. It is the C# fork of https://www.archunit.org/ for Java. ArchUnitNET can check dependencies between
classes, members, interfaces, and more. This is done by analyzing C# bytecode and importing all classes into our C# code
structure. The main focus of ArchUnitNET is to automatically test architecture and coding rules.

## An Example

To use ArchUnitNET, install the ArchUnitNET package from NuGet:

```
PS> Install-Package TngTech.ArchUnitNET
```

If you want to use xUnit, NUnit or MSTestV2 for your unit tests, you should instead install the corresponding ArchUnit extension:

```
PS> Install-Package TngTech.ArchUnitNET.xUnit
PS> Install-Package TngTech.ArchUnitNET.xUnitV3
PS> Install-Package TngTech.ArchUnitNET.NUnit
PS> Install-Package TngTech.ArchUnitNET.MSTestV2
```

#### Create a Test

Then you will want to create a class to start testing. We used xUnit with the ArchUnit extension here, but it works similarly with NUnit or other Unit Test Frameworks.

```cs

using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.Fluent;
using Xunit;

//add a using directive to ArchUnitNET.Fluent.ArchRuleDefinition to easily define ArchRules
using static ArchUnitNET.Fluent.ArchRuleDefinition;


namespace ExampleTest
{
    public class ExampleArchUnitTest
    {
        // TIP: load your architecture once at the start to maximize performance of your tests
        private static readonly Architecture Architecture = new ArchLoader().LoadAssemblies(
            System.Reflection.Assembly.Load("ExampleClassAssemblyName"),
            System.Reflection.Assembly.Load("ForbiddenClassAssemblyName")
        ).Build();
        // replace <ExampleClass> and <ForbiddenClass> with classes from the assemblies you want to test

        //declare variables you'll use throughout your tests up here
        //use As() to give them a custom description
        private readonly IObjectProvider<IType> ExampleLayer =
            Types().That().ResideInAssembly("ExampleAssembly").As("Example Layer");

        private readonly IObjectProvider<Class> ExampleClasses =
            Classes().That().ImplementInterface("IExampleInterface").As("Example Classes");

        private readonly IObjectProvider<IType> ForbiddenLayer =
            Types().That().ResideInNamespace("ForbiddenNamespace").As("Forbidden Layer");

        private readonly IObjectProvider<Interface> ForbiddenInterfaces =
            Interfaces().That().HaveFullNameContaining("forbidden").As("Forbidden Interfaces");


        //write some tests
        [Fact]
        public void TypesShouldBeInCorrectLayer()
        {
            //you can use the fluent API to write your own rules
            IArchRule exampleClassesShouldBeInExampleLayer =
                Classes().That().Are(ExampleClasses).Should().Be(ExampleLayer);
            IArchRule forbiddenInterfacesShouldBeInForbiddenLayer =
                Interfaces().That().Are(ForbiddenInterfaces).Should().Be(ForbiddenLayer);

            //check if your architecture fulfils your rules
            exampleClassesShouldBeInExampleLayer.Check(Architecture);
            forbiddenInterfacesShouldBeInForbiddenLayer.Check(Architecture);

            //you can also combine your rules
            IArchRule combinedArchRule =
                exampleClassesShouldBeInExampleLayer.And(forbiddenInterfacesShouldBeInForbiddenLayer);
            combinedArchRule.Check(Architecture);
        }

        [Fact]
        public void ExampleLayerShouldNotAccessForbiddenLayer()
        {
            //you can give your rules a custom reason, which is displayed when it fails
            //(together with the types that failed the rule)
            IArchRule exampleLayerShouldNotAccessForbiddenLayer = Types().That().Are(ExampleLayer).Should()
                .NotDependOnAny(ForbiddenLayer).Because("it's forbidden");
            exampleLayerShouldNotAccessForbiddenLayer.Check(Architecture);
        }

        [Fact]
        public void ForbiddenClassesShouldHaveCorrectName()
        {
            Classes().That().AreAssignableTo(ForbiddenInterfaces).Should().HaveNameContaining("forbidden")
                .Check(Architecture);
        }

        [Fact]
        public void ExampleClassesShouldNotCallForbiddenMethods()
        {
            Classes().That().Are(ExampleClasses).Should().NotCallAny(
                    MethodMembers().That().AreDeclaredIn(ForbiddenLayer).Or().HaveNameContaining("forbidden"))
                .Check(Architecture);
        }
    }
}
```

#### Run the tests

Since ArchUnitNET is reading the architecture form the analyzed binaries, it is recommended to run ArchUnitNET-based tests in Debug configuration.

```
dotnet test -c Debug
```

For more details on known edge cases, see [the documentation](https://archunitnet.readthedocs.io/en/stable/limitations/debug_artifacts/).

#### Further Info and Help

Check out test examples for the current release at
[ArchUnitNET Examples](https://github.com/TNG/ArchUnitNET/tree/master/ExampleTest "ExampleTests").

## License

ArchUnitNET is published under the Apache License 2.0. For more information concerning the license, see
[Apache License 2.0](http://www.apache.org/licenses/LICENSE-2.0).

## Contributors ✨

Thanks to [all the people who have contributed](https://github.com/TNG/ArchUnitNET/graphs/contributors) to the project.
