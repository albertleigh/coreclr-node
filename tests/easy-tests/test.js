const path = require('path');
const coreclrhosting = require('@albertli90/coreclr-host-cur/build/Debug/al90-coreclr-addon');

// '/bin/Debug/netcoreapp3.1/EasyTests.dll'
const appExit = coreclrhosting.runCoreApp(
    path.join(__dirname, 'bin', 'Debug', 'netcoreapp3.1', 'EasyTests.dll')
);


try{
    const result = coreclrhosting.callManagedFunction(
        "EasyTests.Program",
        "DummyMethod"
    );
    console.error("[EasyTest DummyMethod] res 0#",result);
}catch(e){
    console.error("[EasyTest DummyMethod] err 0#",e);
}

(async ()=>{
    coreclrhosting.callManagedFunction(
        "EasyTests.Program",
        "SetMainTcs",
        0,
    );
    console.log('exit w/', await appExit);    
})()