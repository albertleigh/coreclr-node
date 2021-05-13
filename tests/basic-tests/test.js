
const assert = require('assert');
const path = require('path');
const coreclrhosting = require('@albertli90/coreclr-host-cur/build/Debug/al90-coreclr-addon');


// console.log(coreclrhosting.hello()); // 'world'

global.require = function (module){
  switch (module) {
    case "path":
      return path;
  }
  return undefined;
}

global.registerAsyncTest = function registerAsyncTest(testMethod) {
  describe("async test", function () {
    it("should return promise", function () {
      return testMethod();
    });
    it("should invoke done", function (done) {
      testMethod().then(done);
    });

  });

}

global.setupTestObject = function () {
  global.testObject = {
    integerValue: 42,
    doubleValue: 3.1415,
    stringValue: "Hello world",
    stringArray: ["A", "B"],
    mixedArray: ["A", 42, { iValue: 42, strValue: "strValue" }],
    objectValue: {
      nestedStringValue: "Nested hello"
    },
    nullValue: null,
    trueValue: true,
    falseValue: false,
    addIntegerValue: function (value) {
      return value + this.integerValue;
    },
    funcThatThrows: function () {
      throw new Error("Test error message");
    },
    invokeCallback: function (arg, cb) {
      cb(arg + 'Pong');
    },
    invokeIntCallback: function (arg, cb) {
      cb(arg + 42);
    },
    isPromise: function (promise) {
      var isPromise = promise && typeof promise.then === 'function';
      if (isPromise) {
        promise.catch(e => { }); // DM 29.11.2019: This prevents unobserved promise errors
      }
      return isPromise;
    },
    createPromise: function (shouldResolve) {
      return new Promise(function (resolve, reject) {
        setTimeout(function () {
          if (shouldResolve)
            resolve("Resolved");
          else
            reject(new Error("As requested"));
        }, 10);
      });
    },
    awaitPromise: function (promise) {
      return promise;
    },
    TestClass: function (arg) {
      this.value = arg;
    },
    assertByteArray: function (bytes) {
      assert.strictEqual(4, bytes.byteLength);
      assert.strictEqual(1, bytes[0]);
      assert.strictEqual(2, bytes[1]);
      assert.strictEqual(3, bytes[2]);
      assert.strictEqual(4, bytes[3]);
    },
    assertMixedArray: function (array) {
      assert.strictEqual(4, array.length);
      assert.strictEqual("a", array[0]);
      assert.strictEqual(1, array[1]);
      assert.strictEqual("b", array[2]);
      assert.strictEqual(2, array[3]);
    },
    assertStringArray: function (array) {
      assert.strictEqual(2, array.length);
      assert.strictEqual("a", array[0]);
      assert.strictEqual("b", array[1]);
    },
    assertIntArray: function (array) {
      assert.strictEqual(2, array.length);
      assert.strictEqual(1, array[0]);
      assert.strictEqual(2, array[1]);
    }

  };

  global.testObject.TestClass.staticFunc = function (arg) {
    return arg + 42;
  }
};

console.log("PID: " + process.pid);
// '/bin/Debug/netcoreapp3.1/BasicTests.dll'
const result = coreclrhosting.runCoreApp(
    path.join(__dirname, 'bin', 'Debug', 'netcoreapp3.1', 'BasicTests.dll'),
    "AdditionalArgument"
);

const helloDotnetFunctionResult = coreclrhosting.callManagedFunction(
    "TestApp.Program",
    "HelloDotnetFunction",
    true,
    1e3,
    "oneString",
    {
      message: "oneMessageStringInObj"
    }
)
console.log("HelloDotnetFunction", helloDotnetFunctionResult);

const emptyCnt = coreclrhosting.callManagedFunction(
    "TestApp.Program",
    "EmptyInputMethod"
);
console.log("EmptyInputMethod", emptyCnt);
assert.strictEqual(emptyCnt, 0);

describe('coreclrhosting', function () {

  it ('should seize the string from HelloDotnetFunction', ()=>{
    assert.strictEqual(typeof helloDotnetFunctionResult, "object");
    assert.strictEqual(helloDotnetFunctionResult.argCount, 4);
    assert.strictEqual(helloDotnetFunctionResult.message, "oneMessageStringInResultObj");
  })

  it('should return promise resolving to 0', async function () {
    assert.strictEqual(await result, 0);
  });

});

