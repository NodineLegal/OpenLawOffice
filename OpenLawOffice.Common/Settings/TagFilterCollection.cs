// -----------------------------------------------------------------------
// <copyright file="TagFilterCollection.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Common.Settings
{
    using System.Configuration;
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TagFilterCollection : ConfigurationElementCollection
    {
        public new TagFilterElement this[string name]
        {
            get
            {
                if (IndexOf(name) < 0) return null;
                return (TagFilterElement)BaseGet(name);
            }
        }

        public TagFilterElement this[int index]
        {
            get { return (TagFilterElement)BaseGet(index); }
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
            return new TagFilterElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TagFilterElement)element).Name;
        }

        protected override string ElementName
        {
            get
            {
                return "tagFilter";
            }
        }

        public List<Common.Models.Settings.TagFilter> ToUserSettingsModel()
        {
            List<Common.Models.Settings.TagFilter> tagFilters;
            System.Collections.IEnumerator ie;

            tagFilters = new List<Common.Models.Settings.TagFilter>();

            ie = Common.Settings.Manager.Instance.System.GlobalTaskTagFilters.GetEnumerator();

            while (ie.MoveNext())
            {
                TagFilterElement ele = (TagFilterElement)ie.Current;
                tagFilters.Add(new Common.Models.Settings.TagFilter()
                {
                    Category = ele.Category,
                    Tag = ele.Tag
                });
            }

            return tagFilters;
        }
    }
}
