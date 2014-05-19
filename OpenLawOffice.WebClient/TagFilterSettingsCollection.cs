// -----------------------------------------------------------------------
// <copyright file="TagFilterSettingsCollection.cs" company="Nodine Legal, LLC">
// Licensed to Nodine Legal, LLC under one
// or more contributor license agreements.  See the NOTICE file
// distributed with this work for additional information
// regarding copyright ownership.  Nodine Legal, LLC licenses this file
// to you under the Apache License, Version 2.0 (the
// "License"); you may not use this file except in compliance
// with the License.  You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing,
// software distributed under the License is distributed on an
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
// KIND, either express or implied.  See the License for the
// specific language governing permissions and limitations
// under the License.
// </copyright>
// -----------------------------------------------------------------------

namespace OpenLawOffice.WebClient
{
    using System.Configuration;

    public class TagFilterSettingsCollection : ConfigurationElementCollection
    {
        public new TagFilterSettingElement this[string name]
        {
            get
            {
                if (IndexOf(name) < 0) return null;
                return (TagFilterSettingElement)BaseGet(name);
            }
        }

        public TagFilterSettingElement this[int index]
        {
            get { return (TagFilterSettingElement)BaseGet(index); }
        }

        public int IndexOf(string name)
        {
            name = name.ToLower();

            for (int i = 0; i < base.Count; i++)
            {
                if (this[i].Name.ToLower() == name)
                    return i;
            }

            return -1;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new TagFilterSettingElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TagFilterSettingElement)element).Name;
        }

        protected override string ElementName
        {
            get
            {
                return "tagFilter";
            }
        }
    }
}