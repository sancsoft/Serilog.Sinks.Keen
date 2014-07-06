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
using System.Linq;
using Keen.Core;
using Keen.Net;
using Serilog.Core;
using Serilog.Debugging;
using Serilog.Events;

namespace Serilog.Sinks.Keen
{
    /// <summary>
    /// Writes log events to the Keen.io service.
    /// </summary>
    class KeenSink : ILogEventSink
    {
        readonly IFormatProvider _formatProvider;
        KeenClient _client;

        /// <summary>
        /// Construct a sink that saves logs to the specified storage account. Properties are being send as data and the level is used as tag.
        /// </summary>
        ///  <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
        /// <param name="projectId">The project id as found on the Keen website.</param>
        /// <param name="writeKey">The write key as defined for the specified project on the Keen website.</param>
        public KeenSink(IFormatProvider formatProvider, string projectId, string writeKey)
        {
            _formatProvider = formatProvider;

            ProjectSettingsProvider settings = new ProjectSettingsProvider(projectId, writeKey: writeKey);
            _client = new KeenClient(settings);
        }

        /// <summary>
        /// Emit the provided log event to the sink.
        /// </summary>
        /// <param name="logEvent">The log event to write.</param>
        public void Emit(LogEvent logEvent)
        {
            // create a dictionary of the properties associated with the event,
            // the format and simplification is swiped from the Loggly sink
            var properties = logEvent.Properties
                         .Select(pv => new { Name = pv.Key, Value = KeenPropertyFormatter.Simplify(pv.Value) })
                         .ToDictionary(a => a.Name, b => b.Value);

            // if there is an associated exception, add it to the properties
            if (logEvent.Exception != null)
                properties.Add("Exception", logEvent.Exception);

            // add on the rendered message using the sink's format provider
            properties.Add("Message", logEvent.RenderMessage(_formatProvider));
            // add the numeric representation of the level
            properties.Add("Level", logEvent.Level);
            // include the event timestamp so we don't have to use the receive timestamp on the provider
            properties.Add("Timestamp", logEvent.Timestamp);
            // log the event categorized by the level name
            _client.AddEventAsync(logEvent.Level.ToString(), properties);
        }
    }
}
