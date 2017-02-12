using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace Logging
{
	/// <summary>
	/// Summary description for EnvironmentVariableWriter.
	/// </summary>
	internal class EnvironmentVariableWriter
	{
		// Import the Kernel32 dll file.
		[DllImport("kernel32.dll",CharSet=CharSet.Auto, SetLastError=true)]

		[return:MarshalAs(UnmanagedType.Bool)]

			// The declaration is similar to the SDK function
		public static extern bool SetEnvironmentVariable(string lpName, string lpValue);

		public static bool WriteEnvironmentVariable(string environmentVariable, string variableValue)
		{
			try
			{
				// Get the write permission to set the environment variable.
				EnvironmentPermission environmentPermission = new EnvironmentPermission(EnvironmentPermissionAccess.Write,environmentVariable);

				environmentPermission.Demand(); 

				return SetEnvironmentVariable(environmentVariable, variableValue);
			}
			catch( SecurityException e)
			{
				Console.WriteLine("Exception:" + e.Message);
			}
			return false;
		}

	}
}
