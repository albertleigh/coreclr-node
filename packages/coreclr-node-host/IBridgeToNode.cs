namespace CoreclrNodeHost
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using InProcess;

    /// <summary>
    /// Interface for accessing the node host environment.
    /// </summary>
    public interface IBridgeToNode
    {
        /// <summary>
        /// Returns the global JS object
        /// </summary>
        dynamic Global { get; }

        /// <summary>
        /// Instantiates a new JS object
        /// </summary>
        dynamic New();


        /// <summary>
        /// Try to get the dotnet object of one JsValue
        /// </summary>
        /// <param name="jsValue">
        /// Target JsValue
        /// </param>
        /// <param name="targetType">
        /// Expected type of the Dotnet value
        /// </param>
        /// <param name="result">
        /// Output reference of generated dotnet value
        /// </param>
        /// <returns>
        /// A boolean value suggested whether the seizing process succeeded or not.
        /// </returns>
        bool TryGetObject(JsValue jsValue, Type targetType, out object result);

        /// <summary>
        /// Set a member of a js dynamic object by a string name
        /// </summary>
        /// <param name="ownerHandle">
        /// Target js dynamic object
        /// </param>
        /// <param name="name">
        /// The field name in dynamic object
        /// </param>
        /// <param name="value">
        /// The dotnet value to be assigned
        /// </param>
        void SetMember(JsDynamicObject ownerHandle, string name, DotNetValue value);

        /// <summary>
        /// Checks whether the current thread is the node thread and the bridge can be accessed.
        /// If this is not the case use a <see cref="Run"/> overload to access the bridge.
        /// </summary>
        /// <returns></returns>
        bool CheckAccess();

        /// <summary>
        /// Run a delegate on the node thread.
        /// </summary>
        Task<T> Run<T>(Func<T> func, CancellationToken cancellationToken = default);

        /// <summary>
        /// Run a delegate on the node thread.
        /// </summary>
        Task<T> Run<T>(Func<object, T> func, object state, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Extensions for <see cref="IBridgeToNode"/> that should be default interface implementations.
    /// </summary>
    public static class BridgeToNodeExtensions
    {
        public static Task<T> RunAsync<T>(this IBridgeToNode thiz, Func<Task<T>> asyncFunc, CancellationToken cancellationToken = default)
        {
            return thiz.Run(asyncFunc, cancellationToken).Unwrap();
        }

        public static Task RunAsync(this IBridgeToNode thiz, Func<Task> asyncAction, CancellationToken cancellationToken = default)
        {
            return thiz.Run(asyncAction, cancellationToken).Unwrap();
        }

        public static Task Run(this IBridgeToNode thiz, Action action, CancellationToken cancellationToken = default)
        {
            return thiz.Run(RunActionState, action, cancellationToken);
        }

        public static Task<T> RunAsync<T>(this IBridgeToNode thiz, Func<object, Task<T>> asyncFunc, object state, CancellationToken cancellationToken = default)
        {
            return thiz.Run(asyncFunc, state, cancellationToken).Unwrap();
        }

        public static Task RunAsync(this IBridgeToNode thiz, Func<object, Task> asyncAction, object state, CancellationToken cancellationToken = default)
        {
            return thiz.Run(asyncAction, state, cancellationToken).Unwrap();
        }

        public static Task Run(this IBridgeToNode thiz, Action<object> action, object state, CancellationToken cancellationToken = default)
        {
            return thiz.Run(RunActionState, new Action(() => action(state)), cancellationToken);
        }

        private static object RunActionState(object state)
        {
            ((Action)state)();
            return null;
        }
    }
}
