using System;
using System.Collections.Specialized;

namespace Logging
{
	
	public interface ILogger
	{
	    void Fatal(string stringToLog);
		void Fatal(string stringToLog, Exception e);
        void Fatal(string formatString, params object[] formatParameters);

        void Error(string stringToLog);
		void Error(string stringToLog, Exception e);
		void Error(NameValueCollection additionalInfo, Exception e);
		void Error(string message, NameValueCollection additionalInfo, Exception e);
        void Error(string formatString, params object[] formatParameters);


		void Warn(string stringToLog);
		void Warn(string stringToLog, Exception e);
        void Warn(string formatString, params object[] formatParameters);

        void Info(string stringToLog);
		void Info(string stringToLog, Exception e);
        void Info(string formatString, params object[] formatParameters);

        void Debug(string stringToLog, Exception e);
		void Debug(string stringToLog);
		void Debug(string formatString, params object[] formatParameters);
		bool IsDebugEnabled();
	}	
}

