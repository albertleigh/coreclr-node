

- cmake-js print-configure --debug

- cmake-js print-configure --debug


#windows

cmake 
    "D:\Workspaces\CppWs\main-addon" 
    --no-warn-unused-cli 
    -G"Visual Studio 15 2017 Win64" 
    -DCMAKE_JS_VERSION="6.1.0" 
    -DCMAKE_BUILD_TYPE="Debug" 
    -DCMAKE_RUNTIME_OUTPUT_DIRECTORY="D:\Workspaces\CppWs\main-addon\build" 
    -DCMAKE_JS_INC="C:\Users\wentaoli\.cmake-js\node-x64\v14.16.0\include\node" 
    -DCMAKE_JS_SRC="D:/usr/local/node-v14.16.0-win-x64/node_modules/cmake-js/lib/cpp/win_delay_load_hook.cc" 
    -DNODE_RUNTIME="node" 
    -DNODE_RUNTIMEVERSION="14.16.0" 
    -DNODE_ARCH="x64" 
    -DCMAKE_JS_LIB="C:\Users\wentaoli\.cmake-js\node-x64\v14.16.0\win-x64\node.lib" 
    -DCMAKE_SHARED_LINKER_FLAGS="/DELAYLOAD:NODE.EXE"