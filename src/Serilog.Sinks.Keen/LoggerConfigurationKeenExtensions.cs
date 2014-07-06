// Copyright 2014 Serilog Contributors
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Sinks.Keen;

namespace Serilog
{
    /// <summary>
    /// Adds the WriteTo.Keen() extension method to <see cref="LoggerConfiguration"/>.
    /// </summary>
    public static class LoggerConfigurationKeenExtensions
    {
        /// <summary>
        /// Adds a sink that writes log events to the Keen.io webservice. Properties are being send as data and the level is used as category.
        /// </summary>
        /// <param name="loggerConfiguration">The logger configuration.</param>
        /// <param name="projectId">The project identifier created on the Keen.io website.</param>
        /// <param name="writeKey">The API write key for the project identifier retrieved from the Keen.io website.</param>
        /// <param name="restrictedToMinimumLevel">The minimum log event level required in order to write an event to the sink.</param>
        /// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
        /// <returns>Logger configuration, allowing configuration to continue.</returns>
        /// <exception cref="ArgumentNullException">A required parameter is null.</exception>
        public static LoggerConfiguration Keen(
            this LoggerSinkConfiguration loggerConfiguration,
             string projectId,
             string writeKey,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            IFormatProvider formatProvider = null)
        {
            if (loggerConfiguration == null) throw new ArgumentNullException("loggerConfiguration");

            if (string.IsNullOrWhiteSpace(projectId))
                throw new ArgumentNullException("projectId");

            if (string.IsNullOrWhiteSpace(writeKey))
                throw new ArgumentNullException("writeKey");

            return loggerConfiguration.Sink(
                new KeenSink(formatProvider, projectId, writeKey),
                restrictedToMinimumLevel);
        }

    }
}
