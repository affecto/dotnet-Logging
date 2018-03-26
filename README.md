# Logging
* **Affecto.Logging**
  * Interfaces and base implementation for creating and using loggers.
  * NuGet: https://www.nuget.org/packages/Affecto.Logging
* **Affecto.Logging.Log4Net**
  * Implements the logging interfaces of Affecto.Logging package using Log4Net.
  * NuGet: https://www.nuget.org/packages/Affecto.Logging.Log4Net
* **Affecto.Logging.NLog**
  * Implements the logging interfaces of Affecto.Logging package using NLog.
  * NuGet: https://www.nuget.org/packages/Affecto.Logging.NLog


### Configuration

Version 3.0.0 introduced breaking change in configuration requiring separete configuration file named "log4net.config".
The configuration file is structured according to log4net [documentation](https://logging.apache.org/log4net/release/manual/configuration.html).

Here is example of log4net.config content:

```xml
<log4net>
    <!-- A1 is set to be a ConsoleAppender -->
    <appender name="A1" type="log4net.Appender.ConsoleAppender">
 
        <!-- A1 uses PatternLayout -->
        <layout type="log4net.Layout.PatternLayout">
            <conversionPattern value="%-4timestamp [%thread] %-5level %logger %ndc - %message%newline" />
        </layout>
    </appender>
    
    <!-- Set root logger level to DEBUG and its only appender to A1 -->
    <root>
        <level value="DEBUG" />
        <appender-ref ref="A1" />
    </root>
</log4net>
```


### Build status

[![Build status](https://ci.appveyor.com/api/projects/status/a59odl6kpgrgy4r8?svg=true)](https://ci.appveyor.com/project/affecto/dotnet-logging)