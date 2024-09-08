using Serilog;
using Serilog.Core;
using Serilog.Events;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace NetTest.Logging
{
    public class CustomEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var stackTrace = new StackTrace(true);
            StackFrame frame = null;

            for (int i = 0; i < stackTrace.FrameCount; i++)
            {
                var tempFrame = stackTrace.GetFrame(i);
                var method = tempFrame.GetMethod();
                if (method != null && method.DeclaringType != null && !method.DeclaringType.Namespace.StartsWith("Serilog"))
                {
                    frame = tempFrame;
                    break;
                }
            }

            var lineNumber = frame?.GetFileLineNumber() ?? 0;
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("LineNumber", lineNumber));
        }
    }
}
