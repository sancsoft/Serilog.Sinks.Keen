Serilog.Sinks.Keen
==================

Support for logging to Keen.io from Serilog.  The code is a quick knock-off of the 
Serilog.Sinks.Loggy sink.

### Usage

A simple console application is included as an example and for testing.  Configuring the
connection to Keen.io requires a valid **Project ID** and a **Write Key** from an active
Keen.io account.

<pre>
    Serilog.ILogger log = new LoggerConfiguration()
        .MinimumLevel.Verbose()
        .WriteTo.Keen(projectId, writeKey, restrictedToMinimumLevel:LogEventLevel.Information)
        .CreateLogger();

    log.Information("My cool message");
</pre>

The console application pulls the settings from App.config. Update the file with your settings from Keen.io.

### Event Collections

Events are placed in collections named by the Serilog event level (Verbose, Debug, ... Fatal).

### Event Properties

Event properties are passed to Keen along with the following properties from the LogEvent:

* Message: event text renderd by format provider
* Level: integer representation of event level
* Timestamp: event timestamp to avoid relying on the Keen receive timestamp
