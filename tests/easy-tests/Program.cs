using System;
using System.Threading.Tasks;
using CoreclrNodeHost;
using CoreclrNodeHost.InProcess;


namespace EasyTests
{
    public class Program
    {

        public static int DummyMethod(JsValue[] args)
        {
            if (args != null)
            {
                return args.Length;
            }

            return 0;
        }

        private static readonly TaskCompletionSource<int> MainTaskCompletionSource = new TaskCompletionSource<int>();
        
        public static int SetMainTcs(JsValue[] args)
        {
            var host = NodeHost.Instance;
            host.TryGetObject(args[0], typeof(double), out var oneNumber);
            var result = Convert.ToInt32(oneNumber);
            MainTaskCompletionSource.SetResult(result);
            return result;
        }

        public static Task<int> Main(string[] args)
        {
            Console.WriteLine("Root entry w/o arguments worked");
            return MainTaskCompletionSource.Task;
        }
    }

}