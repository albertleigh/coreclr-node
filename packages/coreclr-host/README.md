<h1 align="center">coreclr node</h1>

<div align="center">

[![npm version](https://img.shields.io/npm/v/@albertli90/coreclr-host.svg)](https://www.npmjs.com/package/@albertli90/coreclr-host)
[![nuget version](https://img.shields.io/nuget/v/CoreclrNodeHost)](https://www.nuget.org/packages/CoreclrNodeHost)

A simple bridge between coreclr and nodejs
</div>

> *This is a very early release of this package, do not suggest using it in production till latter stable version*


Add the CoreclrNodeHost reference to the csproj config
```xml
<PackageReference Include="CoreclrNodeHost" Version="0.0.1" />
```

create an example c# entry class
```csharp
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
```

then invoke the static method from javascript
```javascript
const assert = require('assert');
const path = require('path');
const coreclrhost = require('@albertli90/coreclr-host');

const result = coreclrhost.runCoreApp(
    path.join(__dirname, 'bin', 'Debug', 'netcoreapp3.1', 'BasicExample.dll'),
    "AdditionalArgument"
);

const setMainTcsResult = coreclrhost.callManagedFunction(
    "BasicExample.Program",
    "setMainTcs",
    true,
    1e3,
    "oneString",
    {
        message: "oneMessageStringInObj"
    }
)

assert.strictEqual(typeof setMainTcsResult, "object");
assert.strictEqual(setMainTcsResult.argCount, 4);
assert.strictEqual(setMainTcsResult.message, "oneMessageStringInResultObj");


(async ()=>{
    assert.strictEqual(await result, 0);    
})()
```