using FakeItEasy;
using FakeItEasy.Configuration;
using Microsoft.Extensions.Logging;

namespace PR.CommandCenter.Tests.Helper;

public static class LoggerTestExtensions
{
  public static void VerifyLogMustHaveHappened(this ILogger logger, LogLevel level, string message)
  {
    try
    {
      logger.VerifyLog(level, message)
        .MustHaveHappened();
    }
    catch (Exception e)
    {
      throw new ExpectationException($"while verifying a call to log with message: \"{message}\"", e);
    }
  }

  public static void VerifyLogMustNotHaveHappened(this ILogger logger, LogLevel level, string message)
  {
    try
    {
      logger.VerifyLog(level, message)
        .MustNotHaveHappened();
    }
    catch (Exception e)
    {
      throw new ExpectationException($"while verifying a call to log with message: \"{message}\"", e);
    }
  }

  public static void VerifyAnyLogMustNotHaveHappened(this ILogger logger)
  {
    A.CallTo(logger)
      .Where(call => call.Method.Name == "Log")
      .MustNotHaveHappened();
  }

  public static IVoidArgumentValidationConfiguration VerifyLog(this ILogger logger, LogLevel level, string message)
  {
    return A.CallTo(logger)
      .Where(call => call.Method.Name == "Log"
                     && call.GetArgument<LogLevel>(0) == level
                     && CheckLogMessages(call.GetArgument<IReadOnlyList<KeyValuePair<string, object>>>(2), message));
  }

  private static bool CheckLogMessages(IReadOnlyList<KeyValuePair<string, object>> readOnlyLists, string message)
  {
    foreach (var kvp in readOnlyLists)
    {
      var exceptionMessage = kvp.Value.ToString();
      if (!string.IsNullOrEmpty(message) &&
          exceptionMessage.Contains(message))
      {
        return true;
      }
    }

    return false;
  }
}
