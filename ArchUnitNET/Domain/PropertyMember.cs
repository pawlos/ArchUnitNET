﻿using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain.Dependencies;
using JetBrains.Annotations;
using static ArchUnitNET.Domain.Visibility;

namespace ArchUnitNET.Domain
{
    public class PropertyMember : IMember, ITypeInstance<IType>
    {
        private readonly ITypeInstance<IType> _typeInstance;

        public PropertyMember(
            IType declaringType,
            string name,
            string fullName,
            ITypeInstance<IType> type,
            bool isCompilerGenerated,
            bool? isStatic,
            Writability writability
        )
        {
            Name = name;
            FullName = fullName;
            AssemblyQualifiedName = System.Reflection.Assembly.CreateQualifiedName(
                declaringType.Assembly.FullName,
                fullName
            );
            _typeInstance = type;
            DeclaringType = declaringType;
            IsCompilerGenerated = isCompilerGenerated;
            PropertyTypeDependency = new PropertyTypeDependency(this);
            IsStatic = isStatic;
            Writability = writability;
        }

        public bool IsVirtual { get; internal set; }
        public bool IsAutoProperty { get; internal set; } = true;
        public Visibility SetterVisibility => Setter?.Visibility ?? NotAccessible;
        public Visibility GetterVisibility => Getter?.Visibility ?? NotAccessible;

        [CanBeNull]
        public MethodMember Getter { get; internal set; }

        [CanBeNull]
        public MethodMember Setter { get; internal set; }

        public Writability? Writability { get; }

        public List<IMemberTypeDependency> AttributeDependencies { get; } =
            new List<IMemberTypeDependency>();

        public IMemberTypeDependency PropertyTypeDependency { get; }
        public bool IsCompilerGenerated { get; }

        public bool IsGeneric => false;
        public bool? IsStatic { get; }

        public List<GenericParameter> GenericParameters => new List<GenericParameter>();

        public Assembly Assembly => DeclaringType.Assembly;
        public Namespace Namespace => DeclaringType.Namespace;
        public Visibility Visibility =>
            GetterVisibility < SetterVisibility ? GetterVisibility : SetterVisibility;
        public string Name { get; }
        public string FullName { get; }
        public string AssemblyQualifiedName { get; }
        public IType DeclaringType { get; }
        public IEnumerable<Attribute> Attributes =>
            AttributeInstances.Select(instance => instance.Type);
        public List<AttributeInstance> AttributeInstances { get; } = new List<AttributeInstance>();

        public List<IMemberTypeDependency> MemberDependencies
        {
            get
            {
                var setterDependencies =
                    Setter?.MemberDependencies ?? Enumerable.Empty<IMemberTypeDependency>();
                var getterDependencies =
                    Getter?.MemberDependencies ?? Enumerable.Empty<IMemberTypeDependency>();
                return setterDependencies
                    .Concat(getterDependencies)
                    .Concat(AttributeDependencies)
                    .Concat(new[] { PropertyTypeDependency })
                    .ToList();
            }
        }

        public List<IMemberTypeDependency> MemberBackwardsDependencies { get; } =
            new List<IMemberTypeDependency>();

        public List<ITypeDependency> Dependencies =>
            MemberDependencies.Cast<ITypeDependency>().ToList();

        public List<ITypeDependency> BackwardsDependencies =>
            MemberBackwardsDependencies.Cast<ITypeDependency>().ToList();

        public IType Type => _typeInstance.Type;
        public IEnumerable<GenericArgument> GenericArguments => _typeInstance.GenericArguments;
        public IEnumerable<int> ArrayDimensions => _typeInstance.ArrayDimensions;
        public bool IsArray => _typeInstance.IsArray;

        public override string ToString()
        {
            return $"{DeclaringType.FullName}{'.'}{Name}";
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((PropertyMember)obj);
        }

        private bool Equals(PropertyMember other)
        {
            return Equals(FullName, other.FullName);
        }

        public override int GetHashCode()
        {
            return FullName.GetHashCode();
        }
    }
}
