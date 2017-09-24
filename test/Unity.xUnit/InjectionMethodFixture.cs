﻿// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Reflection;
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

namespace Unity.Tests
{
     
    public class InjectionMethodFixture
    {
        [Fact]
        public void QualifyingInjectionMethodCanBeConfiguredAndIsCalled()
        {
            IUnityContainer container = new UnityContainer()
                .RegisterType<LegalInjectionMethod>(
                        new InjectionMethod("InjectMe"));

            LegalInjectionMethod result = container.Resolve<LegalInjectionMethod>();
            Assert.True(result.WasInjected);
        }

        [Fact]
        public void CannotConfigureGenericInjectionMethod()
        {
            AssertExtensions.AssertException<InvalidOperationException>(() =>
                {
                    new UnityContainer()
                        .RegisterType<OpenGenericInjectionMethod>(
                        new InjectionMethod("InjectMe"));
                });
        }

        [Fact]
        public void CannotConfigureMethodWithOutParams()
        {
            AssertExtensions.AssertException<InvalidOperationException>(() =>
                {
                    new UnityContainer().RegisterType<OutParams>(
                        new InjectionMethod("InjectMe", 12));
                });
        }

        [Fact]
        public void CannotConfigureMethodWithRefParams()
        {
            AssertExtensions.AssertException<InvalidOperationException>(() =>
                {
                    new UnityContainer()
                        .RegisterType<RefParams>(
                        new InjectionMethod("InjectMe", 15));
                });
        }

        [Fact]
        public void CanInvokeInheritedMethod()
        {
            IUnityContainer container = new UnityContainer()
                          .RegisterType<InheritedClass>(
                                  new InjectionMethod("InjectMe"));

            InheritedClass result = container.Resolve<InheritedClass>();
            Assert.True(result.WasInjected);
        }

        public class LegalInjectionMethod
        {
            public bool WasInjected = false;

            public void InjectMe()
            {
                WasInjected = true;
            }
        }

        public class OpenGenericInjectionMethod
        {
            public void InjectMe<T>()
            {
            }
        }

        public class OutParams
        {
            public void InjectMe(out int x)
            {
                x = 2;
            }
        }

        public class RefParams
        {
            public void InjectMe(ref int x)
            {
                x *= 2;
            }
        }

        public class InheritedClass : LegalInjectionMethod
        {
        }
    }
}
