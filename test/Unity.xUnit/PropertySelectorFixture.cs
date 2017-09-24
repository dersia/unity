﻿// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ObjectBuilder2.Tests.TestDoubles;
using Unity.TestSupport;
#if NETFX_CORE
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#elif __IOS__
using NUnit.Framework;
using TestClassAttribute = NUnit.Framework.TestFixtureAttribute;
using TestInitializeAttribute = NUnit.Framework.SetUpAttribute;
using TestMethodAttribute = NUnit.Framework.TestAttribute;
#else
using Xunit;
#endif

namespace ObjectBuilder2.Tests
{
     
    public class PropertySelectorFixture
    {
        [Fact]
        public void SelectorReturnsEmptyListWhenObjectHasNoSettableProperties()
        {
            List<PropertyInfo> properties = SelectProperties(typeof(object));

            Assert.Equal(0, properties.Count);
        }

        [Fact]
        public void SelectorReturnsOnlySettablePropertiesMarkedWithAttribute()
        {
            List<PropertyInfo> properties = SelectProperties(typeof(ClassWithProperties));

            Assert.Equal(1, properties.Count);
            Assert.Equal("PropTwo", properties[0].Name);
        }

        [Fact]
        public void SelectorIgnoresIndexers()
        {
            List<PropertyInfo> properties = SelectProperties(typeof(ClassWithIndexer));

            Assert.Equal(1, properties.Count);
            Assert.Equal("Key", properties[0].Name);
        }

        private List<PropertyInfo> SelectProperties(Type t)
        {
            IPropertySelectorPolicy selector = new PropertySelectorPolicy<DependencyAttribute>();
            IBuilderContext context = GetContext(t);
            var properties = new List<SelectedProperty>(selector.SelectProperties(context, context.PersistentPolicies));
            return properties.Select(sp => sp.Property).ToList();
        }

        private MockBuilderContext GetContext(Type t)
        {
            var context = new MockBuilderContext();
            context.BuildKey = new NamedTypeBuildKey(t);
            return context;
        }
    }

    internal class ClassWithProperties
    {
        private string two;
        private string three;

        [Dependency]
        public string PropOne
        {
            get { return null; }
        }

        [Dependency]
        public string PropTwo
        {
            get { return two; }
            set { two = value; }
        }

        public string PropThree
        {
            get { return three; }
            set { three = value; }
        }
    }

    internal class ClassWithIndexer
    {
        private string key;

        [Dependency]
        public string this[int index]
        {
            get { return null; }
            set { key = index.ToString() + value; }
        }

        [Dependency]
        public string Key
        {
            get { return key; }
            set { key = value; }
        }
    }
}
