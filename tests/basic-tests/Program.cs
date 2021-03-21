namespace TestApp
{
    using System;
    using FluentAssertions;
    using System.Threading;
    using System.Threading.Tasks;
    using CoreclrNodeHost;
    using CoreclrNodeHost.InProcess;
    using Tests;

    public class Program
    {
          public static JsDynamicObject HelloDotnetFunction(JsValue[] args)
          {
             var host = NodeHost.Instance;

             args.Length.Should().Be(4);

             host.TryGetObject(args[0], typeof(bool), out var oneBool);
             oneBool.Should().Be(true);

             host.TryGetObject(args[1], typeof(double), out var oneNumber);
             oneNumber.Should().Be(1e3);

             host.TryGetObject(args[2], typeof(string), out var oneString);
             oneString.Should().Be("oneString");

             host.TryGetObject(args[3], typeof(Object), out dynamic oneObj);
             // oneObj.Should().BeOfType(typeof(JsDynamicObject));
             ((string) oneObj.message).Should().Be("oneMessageStringInObj");

             var result = host.New();
             host.SetMember(result, "argCount", DotNetValue.FromInt(args.Length));
             host.SetMember(result, "message", DotNetValue.FromString("oneMessageStringInResultObj"));

             return result;
          }

        public static Task<int> Main(string[] args)
        {
            var mainCtx = SynchronizationContext.Current;
            var mainScheduler = TaskScheduler.Current;
            var host = NodeHost.Instance;
            var tcs = new TaskCompletionSource<int>();

            /*Console.WriteLine("Waiting for debugger!");
            while(!System.Diagnostics.Debugger.IsAttached) System.Threading.Thread.Sleep(50);*/
            //System.Diagnostics.Debugger.Launch();

            host.Global.registerAsyncTest(new Func<Task>(() => Task.Delay(5)));

            var tests = new MochaTest[] { new Arguments(args), new Global_test_object(), new Task_scheduler(mainCtx, mainScheduler), new LoadContext() };

            var remainingTestCount = tests.Length;

            // Important: As mocha runs the tests asynchronously, we have to dispose after all tests have been run.
            //            But global after will never be called if we are waiting for the return value of this method
            var afterCallback = new Action(() =>
                                           {
                                               remainingTestCount--;
                                               if (remainingTestCount <= 0)
                                               {
                                                   tcs.SetResult(0);
                                               }
                                           });

            foreach (var test in tests)
                test.Register(host, afterCallback);

            return tcs.Task;
        }
    }
}
