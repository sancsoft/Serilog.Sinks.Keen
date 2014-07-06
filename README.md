Serilog.Sinks.Keen
==================

Support for logging to Keen.io from Serilog.  The code is a quick knock-off of the 
Serilog.Sinks.Loggy sink.

### Usage

<pre>
    Serilog.ILogger log = new LoggerConfiguration()
        .MinimumLevel.Verbose()
        .WriteTo.Keen(projectId, writeKey, restrictedToMinimumLevel:LogEventLevel.Information)
        .CreateLogger();

    log.Information("My cool message");
</pre>

* projectId:  Identifies project on the Keen.io account
* writeKey: Write Key from the API keys for the project

#### Event Collections

Events are placed in collections named by the Serilog event level (Verbose, Debug, ... Fatal).

### Event Properties

Event properties are passed to Keen along with the following properties from the LogEvent:

* Message: event text renderd by format provider
* Level: integer representation of event level
* Timestamp: event timestamp to avoid relying on the Keen receive timestamp
