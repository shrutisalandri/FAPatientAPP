using System;
using System.Collections.Generic;
using System.Threading;

namespace Shared.Core
{
    public static class Retry
    {
        public static void Do(
            Action action,
            TimeSpan retryInterval,
            int retryCount = 3)
        {
            Do<object>(() =>
            {
                action();
                return null;
            }, retryInterval, retryCount);
        }

        public static T Do<T>(
            Func<T> action,
            TimeSpan retryInterval,
            int retryCount = 3)
        {
            var exceptions = new List<Exception>();

            for (int retry = 0; retry < retryCount; retry++)
            {
                try
                {
                    return action();
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                    Thread.Sleep(retryInterval);
                }
            }

            if (exceptions.Count > 0)
            {
                // in an ideal world a AggregateException would be thrown
                // This is a .NET 3.5 assembly being called from .NET 4
                // so not an ideal world
                throw exceptions[0];
            }
            else
            {
                throw new Exception("Retry.Do did not return action, but no exceptions logged.");
            }
        }
    }
}
