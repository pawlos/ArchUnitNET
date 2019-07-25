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
using ArchUnitNET.Domain;

namespace ArchUnitNET.Core
{
    internal class NamespaceRegistry
    {
        private readonly Dictionary<string, Namespace> _namespaces = new Dictionary<string, Namespace>();

        public IEnumerable<Namespace> Namespaces => _namespaces.Values;

        public Namespace GetOrCreateNamespace(string typeNamespaceName)
        {
            return RegistryUtils.GetFromDictOrCreateAndAdd(typeNamespaceName, _namespaces,
                s => new Namespace(typeNamespaceName, new List<IType>()));
        }
    }
}