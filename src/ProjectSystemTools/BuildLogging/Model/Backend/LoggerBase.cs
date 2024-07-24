﻿// Copyright (c) Microsoft. All Rights Reserved. Licensed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.IO;
using System.Linq;
using Microsoft.Build.Framework;

namespace Microsoft.VisualStudio.ProjectSystem.Tools.BuildLogging.Model.BackEnd
{
    internal abstract class LoggerBase : ILogger
    {
        protected readonly BackEndBuildTableDataSource DataSource;

        public LoggerVerbosity Verbosity { get => LoggerVerbosity.Diagnostic; set { } }

        public string? Parameters { get; set; }

        protected LoggerBase(BackEndBuildTableDataSource dataSource)
        {
            DataSource = dataSource;
        }

        protected string GetLogPath(Build build)
        {
            var dimensionsString =
                build.Dimensions.Any() ? $"{build.Dimensions.Aggregate((c, n) => string.IsNullOrEmpty(n) ? c : $"{c}_{n}")}_" : string.Empty;

            var filename = $"{Path.GetFileNameWithoutExtension(build.ProjectPath)}_{dimensionsString}{build.BuildType}_{build.StartTime:o}.binlog".Replace(':', '_');

            return PathUtil.GetTempFileName(filename);
        }

        protected void Copy(string from, string to)
        {
            File.Copy(from, to, overwrite: true);
        }

        public abstract void Initialize(IEventSource eventSource);

        public virtual void Shutdown()
        {
        }
    }
}
