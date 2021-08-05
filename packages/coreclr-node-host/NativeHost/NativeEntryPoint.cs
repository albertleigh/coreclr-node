namespace CoreclrNodeHost.NativeHost
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Runtime.Loader;
    using InProcess;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void EntryPointSignature(
        IntPtr context,
        IntPtr nativeMethods,
        int argc,
        [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr, SizeParamIndex = 2)]
        string[] argv,
        IntPtr result);
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void ManagedFunctionSignature(
        int argc, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr, SizeParamIndex = 0)] string[] argv,
        int jsArgC, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.SysInt, SizeParamIndex = 2)] IntPtr[] jsArgV,
        IntPtr result);

    internal static class NativeEntryPoint
    {
        private static NativeNodeHost _host;
        private static string _assembly_path;
        private static readonly EntryPointSignature CompileCheck = RunHostedApplication;        
        private static readonly ManagedFunctionSignature CompileCheck2 = InvokeManagedFunction;

        internal static void RunHostedApplication(IntPtr context,
                                                  IntPtr nativeMethodsPtr,
                                                  int argc,
                                                  [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr, SizeParamIndex = 2)]
                                                  string[] argv,
                                                  IntPtr resultValue)
        {
            
            // Console.WriteLine($"NativeEntryPoint c :: asm path {_assembly_path}");
            // Console.WriteLine("NativeEntryPoint Waiting for debugger!");
            // while(!System.Diagnostics.Debugger.IsAttached) System.Threading.Thread.Sleep(50);
            // System.Diagnostics.Debugger.Launch();
            // Console.WriteLine("NativeEntryPoint Debugger awaited!");
            
            
            // Switch to default ALC
            // var myAlc = AssemblyLoadContext.GetLoadContext(typeof(NativeEntryPoint).Assembly);
            // if (myAlc != AssemblyLoadContext.Default)
            // {
            //     var inCtx = AssemblyLoadContext.Default.LoadFromAssemblyName(typeof(NativeEntryPoint).Assembly.GetName());
            //     var tInCtx = inCtx.GetType(typeof(NativeEntryPoint).FullName);
            //     tInCtx.GetMethod(nameof(RunHostedApplication), BindingFlags.Static | BindingFlags.NonPublic)
            //           .Invoke(null, new object[] { context, nativeMethodsPtr, argc, argv, resultValue });
            //     return;
            // }

            var nativeMethods = Marshal.PtrToStructure<NativeApi>(nativeMethodsPtr);

            _host = new NativeNodeHost(context, nativeMethods);
            NodeHost.Instance = new NodeBridge(_host);

            try
            {
                _assembly_path = argv[0];
                var assembly = Assembly.Load(Path.GetFileNameWithoutExtension(_assembly_path));

                var entryPoint = assembly.EntryPoint;
                if (entryPoint.IsSpecialName && entryPoint.Name.StartsWith("<") && entryPoint.Name.EndsWith(">"))
                    entryPoint = entryPoint.DeclaringType.GetMethod(entryPoint.Name.Substring(1, entryPoint.Name.Length - 2),
                                                                    BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
                
                // Console.WriteLine($"NativeEntryPoint c :: asm path {_assembly_path} entrypoint name {entryPoint.Name}");
                // Console.WriteLine("NativeEntryPoint Waiting for debugger!");
                // while(!System.Diagnostics.Debugger.IsAttached) System.Threading.Thread.Sleep(50);
                // System.Diagnostics.Debugger.Launch();
                // Console.WriteLine("NativeEntryPoint Debugger awaited!");

                var result = _host.Scheduler
                                 .RunCallbackSynchronously(s =>
                                                               entryPoint.Invoke(null,
                                                                                 new object[] { argv.Skip(1).ToArray() }),
                                                           null);

                Marshal.StructureToPtr(DotNetValue.FromObject(result, _host), resultValue, false);
            }
            catch (TargetInvocationException tie)
            {
                Marshal.StructureToPtr(DotNetValue.FromObject(tie.InnerException, _host), resultValue, false);
            }
            catch (Exception e)
            {
                Marshal.StructureToPtr(DotNetValue.FromObject(e, _host), resultValue, false);
            }
        }
        
        
        internal static void InvokeManagedFunction(
         int argc, 
         [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr, SizeParamIndex = 0)] string[] argv,
         int jsArgC,
         [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.SysInt, SizeParamIndex = 2)] IntPtr[] jsArgV,
         IntPtr resultValue)
      {
          
          // Console.WriteLine($"InvokeManagedFunction c :: asm path {_assembly_path}");
          // Console.WriteLine("InvokeManagedFunction Waiting for debugger!");
          // while(!System.Diagnostics.Debugger.IsAttached) System.Threading.Thread.Sleep(50);
          // System.Diagnostics.Debugger.Launch();
          // Console.WriteLine("InvokeManagedFunction Debugger awaited!");
          
         // Switch to default ALC
         // var myAlc = AssemblyLoadContext.GetLoadContext(typeof(NativeEntryPoint).Assembly);
         // if (myAlc != AssemblyLoadContext.Default)
         // {
         //    var inCtx = AssemblyLoadContext.Default.LoadFromAssemblyName(typeof(NativeEntryPoint).Assembly.GetName());
         //    var tInCtx = inCtx.GetType(typeof(NativeEntryPoint).FullName);
         //
         //    tInCtx.GetMethod(nameof(InvokeManagedFunction), BindingFlags.Static | BindingFlags.NonPublic)
         //       .Invoke(null, new object[]
         //       {
         //          argc,
         //          argv,
         //          jsArgC,
         //          jsArgV,
         //          resultValue
         //       });
         //    return;
         // }

         if (_host == null)
         {
            throw new Exception("Dll entrypoint was not activated");
         }

         try
         {
            var asmTyp = argv[0];
            var asmMethod = argv[1];
            var assembly = Assembly.Load(Path.GetFileNameWithoutExtension(_assembly_path)!);
            var tInCtx = assembly.GetType(asmTyp);
            
            if (tInCtx == null) throw new Exception("Cannot find the corresponding type");
            
            var tMethod = tInCtx
               .GetMethod(asmMethod, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            
            if (tMethod == null) throw new Exception("Cannot find the corresponding method");

            var result = _host.Scheduler.RunCallbackSynchronously(s =>
               tMethod.Invoke(null,
                   jsArgV == null ? 
                       new object[]
                       {
                           null
                       }:
                       new object[]{
                           jsArgV.Select(jsPtr => (JsValue) Marshal.PtrToStructure(jsPtr, typeof(JsValue))).ToArray()
                        }),
               null
            );
            Marshal.StructureToPtr(DotNetValue.FromObject(result, _host), resultValue, false);
         }
         catch (TargetInvocationException tie)
         {
            Marshal.StructureToPtr(DotNetValue.FromObject(tie.InnerException, _host), resultValue, false);
         }
         catch (Exception e)
         {
            Marshal.StructureToPtr(DotNetValue.FromObject(e, _host), resultValue, false);
         }

      }
    }
}
