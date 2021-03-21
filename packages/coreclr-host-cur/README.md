

- cmake-js print-configure --debug

- cmake-js print-configure


#windows

cmake
--no-warn-unused-cli
-G"Visual Studio 16 2019"
-A"x64"
-DCMAKE_JS_VERSION="6.1.0"
-DCMAKE_BUILD_TYPE="Debug"
-DCMAKE_RUNTIME_OUTPUT_DIRECTORY="D:\Workspaces\MsftWs\main-addon\build"
-DCMAKE_JS_INC="C:\Users\wentaoli\.cmake-js\node-x64\v14.15.0\include\node"
-DCMAKE_JS_SRC="D:/usr/local/node-v14.15.0-win-x64/node_modules/cmake-js/lib/cpp/win_delay_load_hook.cc"
-DNODE_RUNTIME="node"
-DNODE_RUNTIMEVERSION="14.15.0"
-DNODE_ARCH="x64"
-DCMAKE_JS_LIB="C:\Users\wentaoli\.cmake-js\node-x64\v14.15.0\win-x64\node.lib"
-DCMAKE_SHARED_LINKER_FLAGS="/DELAYLOAD:NODE.EXE"

cmake 
    "D:\Workspaces\MsftWs\main-addon"
--no-warn-unused-cli
-G"Visual Studio 16 2019"
-A"x64"
-DCMAKE_JS_VERSION="6.1.0"
-DCMAKE_BUILD_TYPE="Release"
-DCMAKE_RUNTIME_OUTPUT_DIRECTORY="D:\Workspaces\MsftWs\main-addon\build"
-DCMAKE_JS_INC="C:\Users\wentaoli\.cmake-js\node-x64\v14.15.0\include\node"
-DCMAKE_JS_SRC="D:/usr/local/node-v14.15.0-win-x64/node_modules/cmake-js/lib/cpp/win_delay_load_hook.cc"
-DNODE_RUNTIME="node"
-DNODE_RUNTIMEVERSION="14.15.0"
-DNODE_ARCH="x64"
-DCMAKE_JS_LIB="C:\Users\wentaoli\.cmake-js\node-x64\v14.15.0\win-x64\node.lib"
-DCMAKE_SHARED_LINKER_FLAGS="/DELAYLOAD:NODE.EXE"

# linux

--no-warn-unused-cli
-DCMAKE_ORIGIN="IDE"
-DCMAKE_BUILD_TYPE="Debug"

--no-warn-unused-cli
-DCMAKE_ORIGIN="IDE"
-DCMAKE_BUILD_TYPE="Release"

cmake 
"/home/ali/Workspaces/CppWs/main-addon" 
--no-warn-unused-cli 
-G"Unix Makefiles" 
-DCMAKE_JS_VERSION="6.1.0" 
-DCMAKE_BUILD_TYPE="Debug" 
-DCMAKE_LIBRARY_OUTPUT_DIRECTORY="/home/ali/Workspaces/CppWs/main-addon/build/Debug" 
-DCMAKE_JS_INC="/home/ali/.cmake-js/node-x64/v14.16.0/include/node" 
-DCMAKE_JS_SRC="" 
-DNODE_RUNTIME="node" 
-DNODE_RUNTIMEVERSION="14.16.0" 
-DNODE_ARCH="x64"

cmake
"/home/ali/Workspaces/CppWs/main-addon"
--no-warn-unused-cli
-G"Unix Makefiles"
-DCMAKE_JS_VERSION="6.1.0"
-DCMAKE_BUILD_TYPE="Release"
-DCMAKE_LIBRARY_OUTPUT_DIRECTORY="/home/ali/Workspaces/CppWs/main-addon/build/Release"
-DCMAKE_JS_INC="/home/ali/.cmake-js/node-x64/v14.16.0/include/node"
-DCMAKE_JS_SRC=""
-DNODE_RUNTIME="node"
-DNODE_RUNTIMEVERSION="14.16.0"
-DNODE_ARCH="x64"

# mac
cmake
"/Users/yuqingchen/Workspaces/VscodeWs/main-addon"
--no-warn-unused-cli
-G"Unix Makefiles"
-DCMAKE_JS_VERSION="6.1.0"
-DCMAKE_BUILD_TYPE="Debug"
-DCMAKE_LIBRARY_OUTPUT_DIRECTORY="/Users/yuqingchen/Workspaces/VscodeWs/main-addon/build/Debug"
-DCMAKE_JS_INC="/Users/yuqingchen/.cmake-js/node-x64/v14.16.0/include/node"
-DCMAKE_JS_SRC=""
-DNODE_RUNTIME="node"
-DNODE_RUNTIMEVERSION="14.16.0"
-DNODE_ARCH="x64"
-DCMAKE_CXX_FLAGS="-D_DARWIN_USE_64_BIT_INODE=1 -D_LARGEFILE_SOURCE -D_FILE_OFFSET_BITS=64 -DBUILDING_NODE_EXTENSION"
-DCMAKE_SHARED_LINKER_FLAGS="-undefined dynamic_lookup"