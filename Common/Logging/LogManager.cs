using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using log4net.Config;
using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace Logging
{
    /// <summary>
    /// Summary description for LogManager.
    /// </summary>
    public class LogManager : IConfigurationSectionHandler
    {

        public static readonly string LoggingConfigSection = "LogManagerConfig";

        private static bool _isInitialized = false;

        public object Create(object parent, object context, XmlNode section)
        {
            string runtimeEvironment = "Web";// ConfigurationManager.AppSettings[MachineConfigKey.RuntimeEnvironment];

            XmlNode rootNodeOfDeserialization = null;
            XmlNode enviromentSpecificNode = section[runtimeEvironment];
            if (enviromentSpecificNode != null)
            {
                rootNodeOfDeserialization = enviromentSpecificNode;
            }
            else
            {
                rootNodeOfDeserialization = section["Base"];
            }



            if (rootNodeOfDeserialization != null)
            {
                ConfigureLoging(rootNodeOfDeserialization.InnerXml);
            }
            return null;
        }

        private static void ConfigureLoging(string xmlLoggingConfiguration)
        {
            try
            {
                // ${ProcessName}
                EnvironmentVariableWriter.SetEnvironmentVariable("ProcessName", Process.GetCurrentProcess().ProcessName);
            }
            catch (Exception e)
            {
                ExceptionManager.Publish(e);
                EnvironmentVariableWriter.SetEnvironmentVariable("ProcessName", "ICSUnknownProcess");
            }

            try
            {
                // ${ProcessId}
                EnvironmentVariableWriter.SetEnvironmentVariable("ProcessId", Process.GetCurrentProcess().Id.ToString());
            }
            catch (Exception e)
            {
                ExceptionManager.Publish(e);
                EnvironmentVariableWriter.SetEnvironmentVariable("ProcessName", "<ICSUnknownPID>");
            }

            try
            {
                // ${AppDomainName}
                string appDomainName = AppDomain.CurrentDomain.FriendlyName;
                appDomainName = appDomainName.Replace(Path.DirectorySeparatorChar, '_');
                appDomainName = appDomainName.Replace(Path.AltDirectorySeparatorChar, '_');
                EnvironmentVariableWriter.SetEnvironmentVariable("AppDomainName", appDomainName);
            }
            catch (Exception e)
            {
                ExceptionManager.Publish(e);
                EnvironmentVariableWriter.SetEnvironmentVariable("AppDomainName", "<Unknown_AppDomain>");
            }

            //
            try
            {
                // ${ProcessCommandLineArguments}
                EnvironmentVariableWriter.SetEnvironmentVariable("ProcessCommandLineArguments", GetCommandLineArgumentString());

            }
            catch (Exception e)
            {
                ExceptionManager.Publish(e);
                EnvironmentVariableWriter.SetEnvironmentVariable("ProcessCommandLineArguments", string.Empty); // Fall back to previous behavior.
            }

            try
            {
                string loggingRoot = Path.GetPathRoot(AppDomain.CurrentDomain.BaseDirectory);
                loggingRoot = Path.Combine(loggingRoot, "Logs");
                if (ConfigurationManager.AppSettings["RuntimeEnvironment"] != null)
                    loggingRoot = Path.Combine(loggingRoot, ConfigurationManager.AppSettings["RuntimeEnvironment"]);
                EnvironmentVariableWriter.SetEnvironmentVariable("LoggingRoot", loggingRoot);
            }
            catch (Exception e)
            {
                ExceptionManager.Publish(e);
                EnvironmentVariableWriter.SetEnvironmentVariable("LoggingRoot", string.Empty);
            }

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml("<log4net>" + xmlLoggingConfiguration + "</log4net>");
                XmlConfigurator.Configure(doc.DocumentElement);
                _isInitialized = true;
            }
            catch (Exception)
            {
            }

        }

        private const int MAX_ARGLIST_LENGTH = 240;
        private static string GetCommandLineArgumentString()
        {
            StringBuilder argString = new StringBuilder();
            string[] args = Environment.GetCommandLineArgs();

            // for each command line argument passed in, strip out any chars that 
            // would be invalid in a file name
            // and append all of them into a single string.
            //
            for (int i = 1; i < args.Length; i++) // skip the first element - it is the exe name.
            {
                argString.Append('_');
                string arg = args[i];
                foreach (char c in arg)
                {
                    bool skipThisChar = false;
                    foreach (char c1 in Path.GetInvalidFileNameChars())
                    {
                        if (c1 == c)
                        {
                            skipThisChar = true;
                            break;
                        }
                    }
                    if (!skipThisChar)
                        argString.Append(c);
                }
            }
            if (argString.Length > MAX_ARGLIST_LENGTH)
                return argString.ToString(0, MAX_ARGLIST_LENGTH);
            else
                return argString.ToString();
        }

        //        private static string DEFAULT_LOGGING_CONFIGURATION = @"
        //			<appender name=""Console"" type=""log4net.Appender.ConsoleAppender"">
        //				<layout type=""log4net.Layout.PatternLayout"">
        //					<!-- Use %F and %L to output the caller's file name and line number -->
        //					<conversionPattern value=""%d [%t] %-5p %c [%x] - %m%n"" />
        //				</layout>
        //			</appender>
        //
        //
        //			<appender name=""EventLog"" type=""log4net.Appender.EventLogAppender"" >
        //				<param name=""ApplicationName"" value=""${ProcessName}"" />
        //				<layout type=""log4net.Layout.PatternLayout"">
        //					<param name=""ConversionPattern"" value=""[%-5p] %m%n(%c)%n"" />
        //				</layout> 
        //				<filter type=""log4net.Filter.LevelRangeFilter"">
        //					<param name=""LevelMin"" value=""ERROR"" />
        //					<param name=""LevelMax"" value=""FATAL"" />
        //				</filter>
        //			</appender>
        //
        //			<root>
        //				<level value=""DEBUG"" />
        //				<appender-ref=""EventLog"" />
        //				<appender-ref=""Console"" />
        //			</root>				
        //		";

        private static string DEFAULT_LOGGING_CONFIGURATION = @"
        			<appender name=""Console"" type=""log4net.Appender.ConsoleAppender"">
        				<layout type=""log4net.Layout.PatternLayout"">
        					<!-- Use %F and %L to output the caller's file name and line number -->
        					<conversionPattern value=""%d [%t] %-5p %c [%x] - %m%n"" />
        				</layout>
        			</appender>
        
        
        			<appender name=""EventLog"" type=""log4net.Appender.EventLogAppender"" >
        				<param name=""ApplicationName"" value=""${ProcessName}"" />
        				<layout type=""log4net.Layout.PatternLayout"">
        					<param name=""ConversionPattern"" value=""[%-5p] %m%n(%c)%n"" />
        				</layout> 
        				<filter type=""log4net.Filter.LevelRangeFilter"">
        					<param name=""LevelMin"" value=""ERROR"" />
        					<param name=""LevelMax"" value=""FATAL"" />
        				</filter>
        			</appender>
        
        			<root>
        				<level value=""DEBUG"" />
        				<appender-ref=""EventLog"" />
        				<appender-ref=""Console"" />
        			</root>				
        		";
        private static void ConfigureDefaultLogging()
        {
            ConfigureLoging(DEFAULT_LOGGING_CONFIGURATION);
        }

        private static void Init()
        {
            ConfigurationManager.GetSection(LoggingConfigSection);
            if (!_isInitialized)
                ConfigureDefaultLogging();
            _isInitialized = true;
        }



        public static ILogger GetLogger(string name)
        {
            if (!_isInitialized)
            {
                Init();
            }
            return new Logging.Logger(log4net.LogManager.GetLogger(name));
        }

        public static ILogger GetLogger(object something)
        {
            if (!_isInitialized)
            {
                Init();
            }
            if (something is Type)
            {
                return new Logger(log4net.LogManager.GetLogger((Type)something));
            }
            else
            {
                return GetLogger(something.GetType());
            }
        }

    }
}
