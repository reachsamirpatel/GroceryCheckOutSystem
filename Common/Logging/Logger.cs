// ReSharper disable EmptyGeneralCatchClause
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Text;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using log4net;

namespace Logging
{

    internal class Logger : ILogger
    {
        private ILog _log;
        private static bool PUBLISH_EXCEPTIONS = true;
        private const string PUBLISH_EXCEPTIONS_KEY = "PublishExceptionsFromLogger";

        static Logger()
        {
            
            bool publish = PUBLISH_EXCEPTIONS;
            if (null != ConfigurationManager.AppSettings[PUBLISH_EXCEPTIONS_KEY])
            {
                try
                {
                    publish = Convert.ToBoolean(ConfigurationManager.AppSettings[PUBLISH_EXCEPTIONS_KEY]);
                }
                catch (Exception e)
                {
                    ExceptionManager.Publish(e);
                }
            }
            PUBLISH_EXCEPTIONS = publish;
        }

        public Logger(ILog log)
        {
            _log = log;
        }

        #region Fatal


        public void Fatal(string stringToLog)
        {
            try
            {
                _log.Fatal(stringToLog);
            }
            catch (Exception ex)
            {
                try { ExceptionManager.Publish(new ApplicationException("Unable to log message!" + Environment.NewLine + stringToLog , ex)); }
                catch { }

            }
        }

        public void Fatal(string stringToLog, Exception e)
        {
            try
            {
                _log.Fatal(stringToLog, e);

                if (e != null && PUBLISH_EXCEPTIONS)
                {
                    ExceptionManager.Publish(e);
                }
            }
            catch (Exception ex)
            {
                try { ExceptionManager.Publish(new ApplicationException("Unable to log message!" + Environment.NewLine + stringToLog, ex)); }
                catch { }
                try { ExceptionManager.Publish(new ApplicationException("Inner exception!", e)); }
                catch { }
            }
        }

        public void Fatal(string formatString, params object[] formatParameters)
        {
            string stringToLog = string.Empty;
            try
            {
                stringToLog = string.Format(formatString, formatParameters);
                this.Fatal(stringToLog);
            }
            catch (Exception e)
            {
                try { ExceptionManager.Publish(new ApplicationException("Unable to log message!" + Environment.NewLine + stringToLog, e)); }
                catch { }
            }
        }

        #endregion


        #region Error

  

        public void Error(string stringToLog)
        {
            try
            {
                _log.Error(stringToLog);
            }
            catch (Exception ex)
            {
                try { ExceptionManager.Publish(new ApplicationException("Unable to log message!" + Environment.NewLine + stringToLog, ex)); }
                catch { }
            }
        }

        public void Error(string stringToLog, Exception e)
        {

            try
            {
                _log.Error(stringToLog, e);

                if (e != null && PUBLISH_EXCEPTIONS)
                {
                    ExceptionManager.Publish(e);
                }
            }
            catch (Exception ex)
            {
                try { ExceptionManager.Publish(new ApplicationException("Unable to log message!" + Environment.NewLine + stringToLog, ex)); }
                catch { }
                try { ExceptionManager.Publish(new ApplicationException("Inner exception!", e)); }
                catch { }
            }
        }

        public void Error(NameValueCollection additionalInfo, Exception e)
        {
            Error(String.Empty, additionalInfo, e);
        }

        public void Error(string message, NameValueCollection additionalInfo, Exception e)
        {
            StringBuilder sb = new StringBuilder(message);
            foreach (string key in additionalInfo.Keys)
                sb.Append(key + ": " + additionalInfo[key] + "\r\n");

            Error(sb.ToString(), e);
        }

        public void Error(string formatString, params object[] formatParameters)
        {
            string stringToLog = string.Empty;
            try
            {
                stringToLog = string.Format(formatString, formatParameters);
                this.Error(stringToLog);
            }
            catch (Exception e)
            {
                try { ExceptionManager.Publish(new ApplicationException("Unable to log message!" + Environment.NewLine + stringToLog, e)); }
                catch { }
            }
        }
        #endregion


        #region Warn

        public void Warn(string stringToLog)
        {
            try
            {
                _log.Warn(stringToLog);
            }
            catch (Exception ex)
            {
                try { ExceptionManager.Publish(new ApplicationException("Unable to log message!" + Environment.NewLine + stringToLog, ex)); }
                catch { }
            }
        }

        public void Warn(string stringToLog, Exception e)
        {
            try
            {
                _log.Warn(stringToLog, e);

                if (e != null && PUBLISH_EXCEPTIONS)
                {
                    ExceptionManager.Publish(e);
                }
            }
            catch (Exception ex)
            {
                try { ExceptionManager.Publish(new ApplicationException("Unable to log message!" + Environment.NewLine + stringToLog, ex)); }
                catch { }
                try { ExceptionManager.Publish(new ApplicationException("Inner exception!", e)); }
                catch { }
            }
        }

        public void Warn(string formatString, params object[] formatParameters)
        {
            string stringToLog = string.Empty;
            try
            {
                stringToLog = string.Format(formatString, formatParameters);
                this.Warn(stringToLog);
            }
            catch (Exception e)
            {
                try { ExceptionManager.Publish(new ApplicationException("Unable to log message!" + Environment.NewLine + stringToLog, e)); }
                catch { }
            }
        }

        #endregion

        #region Info

        public void Info(string stringToLog)
        {
            try
            {
                _log.Info(stringToLog);
            }
            catch (Exception ex)
            {
                try { ExceptionManager.Publish(new ApplicationException("Unable to log message!" + Environment.NewLine + stringToLog, ex)); }
                catch { }
            }
        }

        public void Info(string stringToLog, Exception e)
        {
            try
            {
                _log.Info(stringToLog, e);

                if (e != null && PUBLISH_EXCEPTIONS)
                {
                    ExceptionManager.Publish(e);
                }
            }
            catch (Exception ex)
            {
                try { ExceptionManager.Publish(new ApplicationException("Unable to log message!" + Environment.NewLine + stringToLog, ex)); }
                catch { }
                try { ExceptionManager.Publish(new ApplicationException("Inner exception!", e)); }
                catch { }
            }
        }

        public void Info(string formatString, params object[] formatParameters)
        {
            string stringToLog = string.Empty;
            try
            {
                stringToLog = string.Format(formatString, formatParameters);
                this.Info(stringToLog);
            }
            catch (Exception e)
            {
                try { ExceptionManager.Publish(new ApplicationException("Unable to log message!" + Environment.NewLine + stringToLog, e)); }
                catch { }
            }
        }

        #endregion

        #region Debug

        public void Debug(string stringToLog, Exception e)
        {
            try
            {
                _log.Debug(stringToLog, e);

                if (e != null && PUBLISH_EXCEPTIONS)
                {
                    ExceptionManager.Publish(e);
                }
            }
            catch (Exception ex)
            {
                try { ExceptionManager.Publish(new ApplicationException("Unable to log message!" + Environment.NewLine + stringToLog, ex)); }
                catch { }
                try { ExceptionManager.Publish(new ApplicationException("Inner exception!", e)); }
                catch { }
            }
        }

        public void Debug(string stringToLog)
        {
            try
            {
                _log.Debug(stringToLog);
            }
            catch (Exception ex)
            {
                try { ExceptionManager.Publish(new ApplicationException("Unable to log message!" + Environment.NewLine + stringToLog, ex)); }
                catch { }
            }
        }

        public void Debug(string formatString, params object[] formatParameters)
        {
            string stringToLog = string.Empty;
            try
            {
                stringToLog = string.Format(formatString, formatParameters);
                this.Debug(stringToLog);
            }
            catch (Exception e)
            {
                try { ExceptionManager.Publish(new ApplicationException("Unable to log message!" + Environment.NewLine + stringToLog, e)); }
                catch { }
            }
        }

        #endregion

        public bool IsDebugEnabled()
        {
            return _log.IsDebugEnabled;
        }


    }
}
// ReSharper restore EmptyGeneralCatchClause
