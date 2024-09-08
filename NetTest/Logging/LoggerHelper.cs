using Serilog;
using System.Runtime.CompilerServices;

namespace NetTest.Logging
{
    public class LoggerHelper
    {
        public static void LogInformation(string message,
                                      [CallerMemberName] string memberName = "",
                                      [CallerFilePath] string filePath = "",
                                      [CallerLineNumber] int lineNumber = 0)
        {
            Log.ForContext("FilePath", filePath)
               .ForContext("LineNumber", lineNumber)
               .ForContext("MemberName", memberName)
               .Information(message);
        }

        public static void LogError(string message,
                                [CallerMemberName] string memberName = "",
                                [CallerFilePath] string filePath = "",
                                [CallerLineNumber] int lineNumber = 0)
        {
            Log.ForContext("FilePath", filePath)
               .ForContext("LineNumber", lineNumber)
               .ForContext("MemberName", memberName)
               .Error(message);
        }
    }
}
