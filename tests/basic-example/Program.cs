namespace BasicExample
{
    using System;
    using FluentAssertions;
    using System.Threading.Tasks;
    using CoreclrNodeHost;
    using CoreclrNodeHost.InProcess;

    public class Program
    {
        
        private static readonly TaskCompletionSource<int> MainTaskCompletionSource = new TaskCompletionSource<int>();

        public static JsDynamicObject setMainTcs(JsValue[] args)
        {
            
            Console.WriteLine($"Basic example setMainTcs invoked w/o {args.Length} arguments");
            
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
            
            MainTaskCompletionSource.SetResult(0);
            
            return result;
        }

        public static Task<int> Main(string[] args)
        {
            Console.WriteLine($"Basic example entry invoked {args[0]}");

            return MainTaskCompletionSource.Task;
        }
    }
}