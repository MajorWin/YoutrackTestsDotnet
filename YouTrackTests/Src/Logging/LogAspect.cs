using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using PostSharp.Aspects;
using PostSharp.Serialization;

namespace YouTrackWebdriverTests.Logging
{
    [PSerializable]
    public class LogAspect : OnMethodBoundaryAspect
    {
        private static int myIndentationLevel = 0;
        private static readonly Stack<DateTime> Timers = new Stack<DateTime>();

        public override void OnEntry(MethodExecutionArgs args)
        {
            TestContext.Out.WriteLine($"{GetIndent()}{GetMethodCallString(args)}");

            Timers.Push(DateTime.Now);

            myIndentationLevel++;
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            var workTime = DateTime.Now - Timers.Pop();

            TestContext.Out.WriteLine(
                $"{GetIndent()}{GetMethodCallString(args)} {(int) workTime.TotalMilliseconds}ms");

            myIndentationLevel--;
        }

        private static string GetIndent() => new string(' ', myIndentationLevel * 4);

        private static string GetMethodCallString(MethodExecutionArgs args)
        {
            var callString = new StringBuilder();

            callString.Append(args.Method.DeclaringType?.Name ?? "");
            if (callString.Length > 0)
                callString.Append(".");
            callString.Append(args.Method.Name);

            callString.Append("(");
            for (var i = 0; i < args.Arguments.Count; i++)
            {
                var argumentName = args.Method.GetParameters()[i].Name;
                var argumentValue = args.Arguments[i];
                var argumentValueString = argumentValue.GetType().Namespace == "System"
                    ? argumentValue
                    : argumentValue.GetType().Name;

                callString.Append($"{argumentName}: {argumentValueString}");

                if (i != args.Arguments.Count - 1)
                {
                    callString.Append(", ");
                }
            }

            callString.Append(")");

            return callString.ToString();
        }
    }
}
