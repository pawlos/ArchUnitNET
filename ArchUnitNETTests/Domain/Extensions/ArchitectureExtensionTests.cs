using System;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Exceptions;
using ArchUnitNET.Domain.Extensions;
using TestAssembly;
using TestAssembly.Domain.Marker;
using Xunit;

namespace ArchUnitNETTests.Domain.Extensions
{
    public class ArchitectureExtensionTests
    {
        private readonly Architecture _architecture =
            StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture;

        [Fact]
        public void FoundCorrectClassInArchitecture()
        {
            Assert.Equal(
                _architecture.Classes.SingleOrDefault(archClass =>
                    archClass.Name == nameof(Class1)
                ),
                _architecture.GetClassOfType(typeof(Class1))
            );
        }

        [Fact]
        public void FoundCorrectInterfaceInArchitecture()
        {
            Assert.Equal(
                _architecture.Interfaces.SingleOrDefault(archClass =>
                    archClass.Name == nameof(IEntity)
                ),
                _architecture.GetInterfaceOfType(typeof(IEntity))
            );
        }

        [Fact]
        public void TypeNotInArchitectureNotFound()
        {
            Assert.Throws<TypeDoesNotExistInArchitecture>(() =>
                _architecture.GetITypeOfType(typeof(Guid))
            );
        }
    }
}
