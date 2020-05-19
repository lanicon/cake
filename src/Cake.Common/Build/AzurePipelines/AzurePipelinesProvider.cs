﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Build.AzurePipelines.Data;
using Cake.Core;
using Cake.Core.Diagnostics;

namespace Cake.Common.Build.AzurePipelines
{
    /// <summary>
    /// Responsible for communicating with Team Foundation Build (VSTS or TFS).
    /// </summary>
    public sealed class AzurePipelinesProvider : IAzurePipelinesProvider
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzurePipelinesProvider"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="log">The log.</param>
        public AzurePipelinesProvider(ICakeEnvironment environment, ICakeLog log)
        {
            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }
            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }
            _environment = environment;
            Environment = new AzurePipelinesEnvironmentInfo(environment);
            Commands = new AzurePipelinesCommands(environment, log);
        }

        /// <summary>
        /// Gets a value indicating whether the current build is running on Azure Pipelines.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current build is running on Azure Pipelines; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunningOnAzurePipelines
            => !string.IsNullOrWhiteSpace(_environment.GetEnvironmentVariable("TF_BUILD")) && !IsHostedAgent;

        /// <summary>
        /// Gets a value indicating whether the current build is running on hosted Azure Pipelines.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current build is running on hosted Azure Pipelines; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunningOnAzurePipelinesHosted
            => !string.IsNullOrWhiteSpace(_environment.GetEnvironmentVariable("TF_BUILD")) && IsHostedAgent;

        /// <summary>
        /// Gets the TF Build environment.
        /// </summary>
        /// <value>
        /// The TF Build environment.
        /// </value>
        public AzurePipelinesEnvironmentInfo Environment { get; }

        /// <summary>
        /// Gets the TF Build Commands provider.
        /// </summary>
        /// <value>
        /// The TF Build commands provider.
        /// </value>
        public IAzurePipelinesCommands Commands { get; }

        /// <summary>
        /// Gets a value indicating whether the current build is running on a hosted build agent.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current build is running on a hosted agent; otherwise, <c>false</c>.
        /// </value>
        private bool IsHostedAgent => Environment.Agent.IsHosted;
    }
}