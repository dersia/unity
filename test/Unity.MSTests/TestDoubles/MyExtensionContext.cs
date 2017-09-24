// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.
using System;
using ObjectBuilder2;
using Unity.ObjectBuilder;

namespace Unity.Tests.TestDoubles
{
    public class MyExtensionContext : ExtensionContext
    {
        private UnityContainer container;
        private int i = 0;

        public MyExtensionContext(UnityContainer container)
        {
            this.container = container;
            this.Strategies.Add(new LifetimeStrategy(), UnityBuildStage.Lifetime);
        }

        public override IUnityContainer Container
        {
            get { return this.container; }
        }

        public override StagedStrategyChain<UnityBuildStage> Strategies
        {
            get { return new StagedStrategyChain<UnityBuildStage>(); }
        }

        public override StagedStrategyChain<UnityBuildStage> BuildPlanStrategies
        {
            get { return new StagedStrategyChain<UnityBuildStage>(); }
        }

        public override IPolicyList Policies
        {
            get { return new PolicyList(); }
        }

        public override ILifetimeContainer Lifetime
        {
            get { return null; }
        }

        public override void RegisterNamedType(Type t, string name)
        {
        }

        public override event EventHandler<RegisterEventArgs> Registering
        {
            add { this.i++; }
            remove { this.i--; }
        }

        /// <summary>
        /// This event is raised when the <see cref="UnityContainer.RegisterInstance(Type,string,object,LifetimeManager)"/> method,
        /// or one of its overloads, is called.
        /// </summary>
        public override event EventHandler<RegisterInstanceEventArgs> RegisteringInstance
        {
            add { this.i++; }
            remove { this.i--; }
        }

#pragma warning disable 67
        public override event EventHandler<ChildContainerCreatedEventArgs> ChildContainerCreated;
#pragma warning restore 67
    }
}
