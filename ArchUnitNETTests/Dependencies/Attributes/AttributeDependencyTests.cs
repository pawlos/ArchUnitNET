/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paula.ruiz@tngtech.com>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Matcher;
using ArchUnitNETTests.Dependencies.Members;
using ArchUnitNETTests.Fluent;
using TestAssembly;
using Xunit;
using static ArchUnitNETTests.Dependencies.Attributes.AttributeAssertionRepository;

namespace ArchUnitNETTests.Dependencies.Attributes
{
    public class AttributeDependencyTests
    {
        private static readonly Architecture Architecture = StaticTestArchitectures.AttributeDependencyTestArchitecture;
        private readonly IEnumerable<Class> _eventHandlerImplementClasses;
        private readonly Class _originClass;
        private readonly Class _hello;
        private readonly Class _helloEvent;
        private readonly Class _class1;
        private readonly Class _class2;
        private readonly Class _classWithAttribute;
        private readonly Class _classWithBodyTypeA;

        public AttributeDependencyTests()
        {
            var eventHandler = Architecture.GetInterfaceOfType(typeof(IEventHandler<>));
            _eventHandlerImplementClasses = Architecture.Classes.Where(cls => cls.Implements(eventHandler));
            _originClass = Architecture.GetClassOfType(typeof(ClassWithInnerAttributeDependency));
            _hello = Architecture.GetClassOfType(typeof(Hello));
            _helloEvent = Architecture.GetClassOfType(typeof(HelloEvent));

            _class1 = Architecture.GetClassOfType(typeof(Class1));
            _class2 = Architecture.GetClassOfType(typeof(Class2));
            _classWithAttribute = Architecture.GetClassOfType(typeof(ClassWithExampleAttribute));
            _classWithBodyTypeA = Architecture.GetClassOfType(typeof(ClassWithBodyTypeA));

        }
        [Fact]
        public void ForbidAttributeForClass()
        {
            _eventHandlerImplementClasses.ShouldAll(cls => !TypeExtensions.DependsOn(cls, "forbidden"));
        }

        [Fact]
        public void ClassAttributeInnerDependencyAssignedToOriginClass()
        {
            //Setup
            var expectedClassTargets = new[] {_hello, _helloEvent};
            
            //Assert
            expectedClassTargets.ShouldAll(targetClass => _originClass.DependsOn(targetClass.Name));
        }
        
        [Fact]
        public void MemberAttributeInnerDependencyAssignedToOriginClass()
        {
            //Setup
            var expectedClassTargets = new[] {_class1, _class2, _classWithAttribute, _classWithBodyTypeA};
            
            //Assert
            expectedClassTargets.ShouldAll(targetClass => _originClass.DependsOn(targetClass.Name));
        }

        [Theory]
        [ClassData(typeof(AttributeTestsBuild.TypeAttributesAreFoundData))]
        public void TypeAttributesAreFound(IType targetType, Class attributeClass, Attribute attribute)
        {
            TypeAttributeAsExpected(targetType, attributeClass, attribute);
            AttributeDependencyAsExpected(targetType, attributeClass);
        }

        [Theory]
        [ClassData(typeof(AttributeTestsBuild.MemberAttributesAreFoundData))]
        public void MemberAttributesAreFound(IMember targetMember, Class attributeClass,
            Attribute attribute)
        {
            MemberAttributeAsExpected(targetMember, attributeClass, attribute);
            AttributeDependencyAsExpected(targetMember, attributeClass);
        }

        [Fact]
        public void OriginAsExpected()
        {
            _originClass.GetAttributeTypeDependencies().ShouldAll(dependency => 
                dependency.Origin.Equals(_originClass));
        }
    }
}