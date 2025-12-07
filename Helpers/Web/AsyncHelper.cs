// Cloned From TrueNorthPLM, Free License 08-02-2025 //

namespace FreePLM.Core.Helpers.Web
{
    public static class AsyncHelper
    {
        private static readonly TaskFactory _taskFactory = new
            TaskFactory(CancellationToken.None,
                        TaskCreationOptions.None,
                        TaskContinuationOptions.None,
                        TaskScheduler.Default);

        /// <summary>
        /// Runs a task synchronously on the current thread, without a callback
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static TResult RunSync<TResult>(Func<Task<TResult>> func, CancellationToken cancellationToken = default)
            => _taskFactory
                .StartNew(func)
                .Unwrap()
                .GetAwaiter()
                .GetResult();

        /// <summary>
        /// Runs a task synchronously on the current thread, without a call back
        /// </summary>
        /// <param name="func"></param>
        /// <param name="cancellationToken"></param>
        public static void RunSync(Func<Task> func, CancellationToken cancellationToken = default)
            => _taskFactory
                .StartNew(func, cancellationToken)
                .Unwrap()
                .GetAwaiter()
                .GetResult();

        /// <summary>
        /// Runs a Task synchronously on the current thread, with a callback
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <param name="onCompletion"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static TResult RunSync<TResult>(Func<Task<TResult>> func, Action<TResult>? onCompletion = null, CancellationToken cancellationToken = default)
        {
            TResult result = _taskFactory.StartNew(func).Unwrap().GetAwaiter().GetResult();
            onCompletion?.Invoke(result); // Invoke the callback if provided
            return result;
        }

        /// <summary>
        /// Runs a Task synchronously on the current thread, with a callback
        /// </summary>
        /// <param name="func"></param>
        /// <param name="onCompletion"></param>
        /// <param name="cancellationToken"></param>
        public static void RunSync(Func<Task> func, Action? onCompletion = null, CancellationToken cancellationToken = default)
        {
            _taskFactory.StartNew(func, cancellationToken).Unwrap().GetAwaiter().GetResult();
            onCompletion?.Invoke(); // Invoke the callback if provided
        }
    }
}

// Example usage:
//  AsyncHelper.RunSync(
//      async () =>
//      {
//          // Your asynchronous operation here
//          await Task.Delay(2000); // Example asynchronous operation
//          return "Operation completed!";
//      },
//      result =>
//      {
//          // Notification logic using the result
//          Console.WriteLine($"Notification: {result}");
//      }
//  );

