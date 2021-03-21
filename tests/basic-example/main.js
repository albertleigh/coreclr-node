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
