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
using System.Configuration;
using System.Threading;
using Serilog;
using Serilog.Events;

namespace Serilog.Console.Keen
{
    /// <summary>
    /// A console test program to check delivery of messages to the Keen.io
    /// event sink. 
    /// Add the projectId and writeKey to the app.config to attach to an
    /// endpoint at Keen.io.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            string projectId = ConfigurationManager.AppSettings["Keen_projectId"];
            string writeKey = ConfigurationManager.AppSettings["Keen_writeKey"];

            // create a logger with colored console and Keen.io sinks
            Serilog.ILogger log = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.ColoredConsole()
                .WriteTo.Keen(projectId, writeKey, restrictedToMinimumLevel:LogEventLevel.Verbose)
                .CreateLogger();

            System.Console.WriteLine("Sending log events to the console and to Keen.io");

            // send a flat test message to each of the supported log levels
            log.Verbose("Verbose test message");
            log.Debug("Debug test message");
            log.Information("Information test message");
            log.Warning("Warning test message");
            log.Error("Error test message");
            log.Fatal("Fatal test message");

            // log a rendered message
            var testObject = new {
                Id = 1,
                Name = "test",
                Description = "A simple test object"
            };
            log.Information("Testing logging of simple objects {object}", testObject);

            // log a rendered message of a common object
            log.Information("Local time {datetime}", DateTime.Now);
            Thread.Sleep(1000);
        }
    }
}
