﻿using System.Linq;
using ArchUnitNET.Fluent.Slices;
using ArchUnitNET.xUnit;
using Xunit;

namespace ArchUnitNETTests.Fluent.Slices
{
    public class SlicesTests
    {
        [Fact]
        public void CycleDetectionTest()
        {
            Assert.Throws<FailedArchRuleException>(() =>
                SliceRuleDefinition
                    .Slices()
                    .Matching("TestAssembly.Slices.(**)")
                    .Should()
                    .BeFreeOfCycles()
                    .Check(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture)
            );
            Assert.False(
                SliceRuleDefinition
                    .Slices()
                    .Matching("TestAssembly.Slices.(**)")
                    .Should()
                    .BeFreeOfCycles()
                    .HasNoViolations(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture)
            );
            Assert.True(
                SliceRuleDefinition
                    .Slices()
                    .Matching("TestAssembly.Slices.(**)..")
                    .Should()
                    .BeFreeOfCycles()
                    .HasNoViolations(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture)
            );
        }

        [Fact]
        public void MatchingTest()
        {
            Assert.Equal(
                7,
                SliceRuleDefinition
                    .Slices()
                    .Matching("TestAssembly.Slices.(*)")
                    .GetObjects(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture)
                    .Count()
            );
            Assert.Equal(
                7,
                SliceRuleDefinition
                    .Slices()
                    .Matching("TestAssembly.Slices.(**)")
                    .GetObjects(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture)
                    .Count()
            );
            Assert.Equal(
                7,
                SliceRuleDefinition
                    .Slices()
                    .Matching("TestAssembly.Slices.(*)..")
                    .GetObjects(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture)
                    .Count()
            );
            Assert.Equal(
                3,
                SliceRuleDefinition
                    .Slices()
                    .Matching("TestAssembly.Slices.(**)..")
                    .GetObjects(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture)
                    .Count()
            );
            Assert.Equal(
                2,
                SliceRuleDefinition
                    .Slices()
                    .Matching("TestAssembly.Slices.Slice3.(*)")
                    .GetObjects(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture)
                    .Count()
            );
            Assert.False(
                SliceRuleDefinition
                    .Slices()
                    .Matching("TestAssembly.Slices.Service.(*)")
                    .GetObjects(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture)
                    .Any()
            );
        }

        [Fact]
        public void NotDependOnEachOtherTest()
        {
            SliceRuleDefinition
                .Slices()
                .Matching("TestAssembly.Slices.Slice3.(*)")
                .Should()
                .NotDependOnEachOther()
                .Check(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture);
            SliceRuleDefinition
                .Slices()
                .Matching("TestAssembly.Slices.Slice1.(*)")
                .Should()
                .NotDependOnEachOther()
                .Check(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture);
            Assert.True(
                SliceRuleDefinition
                    .Slices()
                    .Matching("TestAssembly.Slices.Slice1.(*)")
                    .Should()
                    .NotDependOnEachOther()
                    .HasNoViolations(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture)
            );
            Assert.Throws<FailedArchRuleException>(() =>
                SliceRuleDefinition
                    .Slices()
                    .Matching("TestAssembly.Slices.(**)")
                    .Should()
                    .NotDependOnEachOther()
                    .Check(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture)
            );
            Assert.False(
                SliceRuleDefinition
                    .Slices()
                    .Matching("TestAssembly.Slices.(**)")
                    .Should()
                    .NotDependOnEachOther()
                    .HasNoViolations(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture)
            );
            Assert.False(
                SliceRuleDefinition
                    .Slices()
                    .Matching("TestAssembly.Slices.(*)..")
                    .Should()
                    .NotDependOnEachOther()
                    .HasNoViolations(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture)
            );
            Assert.False(
                SliceRuleDefinition
                    .Slices()
                    .Matching("TestAssembly.Slices.(**)..")
                    .Should()
                    .NotDependOnEachOther()
                    .HasNoViolations(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture)
            );
        }
    }
}
