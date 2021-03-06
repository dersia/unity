﻿// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.


namespace ObjectBuilder2
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public delegate void DynamicBuildPlanMethod(IBuilderContext context);

    /// <summary>
    /// 
    /// </summary>
    public class DynamicMethodBuildPlan : IBuildPlanPolicy
    {
        private readonly DynamicBuildPlanMethod buildMethod;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buildMethod"></param>
        public DynamicMethodBuildPlan(DynamicBuildPlanMethod buildMethod)
        {
            this.buildMethod = buildMethod;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void BuildUp(IBuilderContext context)
        {
            buildMethod(context);
        }
    }
}
