cmake_minimum_required(VERSION 3.17)
cmake_policy(SET CMP0042 NEW)
set (CMAKE_CXX_STANDARD 17)

set(CMAKE_VERBOSE_MAKEFILE ON)

project (al90-coreclr-addon)

if (NOT DEFINED CMAKE_BUILD_TYPE)
    set(CMAKE_BUILD_TYPE, "Debug")
endif()

if( CMAKE_ORIGIN MATCHES IDE)

    function(GET_CMAKE_JS_CONFIG OUTPUT_VARIABLE)
        if (CMAKE_BUILD_TYPE MATCHES "(D|d)ebug")
            if(WIN32)
                execute_process(
                        COMMAND .\\node_modules\\.bin\\cmake-js.cmd  print-configure --debug
                        WORKING_DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}
                        OUTPUT_VARIABLE CMAKE_JS_CONF_ALL_OUTPUT
                )
            else()
                execute_process(
                        COMMAND ./node_modules/.bin/cmake-js  print-configure --debug
                        WORKING_DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}
                        OUTPUT_VARIABLE CMAKE_JS_CONF_ALL_OUTPUT)
            endif()
        else()
            if(WIN32)
                execute_process(
                        COMMAND .\\node_modules\\.bin\\cmake-js.cmd  print-configure
                        WORKING_DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}
                        OUTPUT_VARIABLE CMAKE_JS_CONF_ALL_OUTPUT)
            else()
                execute_process(
                        COMMAND ./node_modules/.bin/cmake-js  print-configure
                        WORKING_DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}
                        OUTPUT_VARIABLE CMAKE_JS_CONF_ALL_OUTPUT)
            endif()
        endif()

        string(LENGTH "${CMAKE_JS_CONF_ALL_OUTPUT}" CMAKE_JS_CONF_ALL_OUTPUT_LENGTH)
        string(FIND "${CMAKE_JS_CONF_ALL_OUTPUT}" "cmake " CMAKE_INDEX)
        string(SUBSTRING "${CMAKE_JS_CONF_ALL_OUTPUT}" "${CMAKE_INDEX}" "${CMAKE_JS_CONF_ALL_OUTPUT_LENGTH}" CMAKE_JS_CONF)
        set("${OUTPUT_VARIABLE}" "${CMAKE_JS_CONF}" PARENT_SCOPE)
    endfunction(GET_CMAKE_JS_CONFIG)

    function(GET_CMAKE_JS_SHORT_VARIABLE INPUT_STRING VARIABLE_TO_SELECT OUTPUT_VARIABLE)
        set(SEARCH_STRING "-${VARIABLE_TO_SELECT}\"")
        string(LENGTH "${SEARCH_STRING}" SEARCH_STRING_LENGTH)
        string(LENGTH "${INPUT_STRING}" INPUT_STRING_LENGTH)

        string(FIND "${INPUT_STRING}" "-${VARIABLE_TO_SELECT}\"" SEARCH_STRING_INDEX)

        math(EXPR SEARCH_STRING_INDEX "${SEARCH_STRING_INDEX}+${SEARCH_STRING_LENGTH}")

        string(SUBSTRING "${INPUT_STRING}" ${SEARCH_STRING_INDEX} ${INPUT_STRING_LENGTH} AFTER_SEARCH_STRING)
        string(FIND "${AFTER_SEARCH_STRING}" "\"" QUOTE_INDEX)
        string(SUBSTRING "${AFTER_SEARCH_STRING}" "0" "${QUOTE_INDEX}" RESULT_STRING)
        set("${OUTPUT_VARIABLE}" "${RESULT_STRING}" PARENT_SCOPE)
    endfunction(GET_CMAKE_JS_SHORT_VARIABLE)

    function(GET_CMAKE_JS_VARIABLE INPUT_STRING VARIABLE_TO_SELECT OUTPUT_VARIABLE)
        set(SEARCH_STRING "${VARIABLE_TO_SELECT}=\"")
        string(LENGTH "${SEARCH_STRING}" SEARCH_STRING_LENGTH)
        string(LENGTH "${INPUT_STRING}" INPUT_STRING_LENGTH)

        string(FIND "${INPUT_STRING}" "${VARIABLE_TO_SELECT}=\"" SEARCH_STRING_INDEX)

        math(EXPR SEARCH_STRING_INDEX "${SEARCH_STRING_INDEX}+${SEARCH_STRING_LENGTH}")

        string(SUBSTRING "${INPUT_STRING}" ${SEARCH_STRING_INDEX} ${INPUT_STRING_LENGTH} AFTER_SEARCH_STRING)
        string(FIND "${AFTER_SEARCH_STRING}" "\"" QUOTE_INDEX)
        string(SUBSTRING "${AFTER_SEARCH_STRING}" "0" "${QUOTE_INDEX}" RESULT_STRING)
        set("${OUTPUT_VARIABLE}" "${RESULT_STRING}" PARENT_SCOPE)
    endfunction(GET_CMAKE_JS_VARIABLE)

    get_cmake_js_config(CMAKE_JS_CONF)

    if(WIN32)
#        windows
        get_cmake_js_short_variable("${CMAKE_JS_CONF}" "G" CMAKE_GENERATOR)
        get_cmake_js_short_variable("${CMAKE_JS_CONF}" "A" CMAKE_GENERATOR_PLATFORM)
        get_cmake_js_variable("${CMAKE_JS_CONF}" "CMAKE_JS_VERSION" CMAKE_JS_VERSION)
        get_cmake_js_variable("${CMAKE_JS_CONF}" "CMAKE_BUILD_TYPE" CMAKE_BUILD_TYPE)
        get_cmake_js_variable("${CMAKE_JS_CONF}" "CMAKE_RUNTIME_OUTPUT_DIRECTORY" CMAKE_RUNTIME_OUTPUT_DIRECTORY)
        get_cmake_js_variable("${CMAKE_JS_CONF}" "CMAKE_JS_INC" CMAKE_JS_INC)
        get_cmake_js_variable("${CMAKE_JS_CONF}" "CMAKE_JS_SRC" CMAKE_JS_SRC)
        get_cmake_js_variable("${CMAKE_JS_CONF}" "NODE_RUNTIME" NODE_RUNTIME)
        get_cmake_js_variable("${CMAKE_JS_CONF}" "NODE_RUNTIMEVERSION" NODE_RUNTIMEVERSION)
        get_cmake_js_variable("${CMAKE_JS_CONF}" "NODE_ARCH" NODE_ARCH)
        get_cmake_js_variable("${CMAKE_JS_CONF}" "CMAKE_JS_LIB" CMAKE_JS_LIB)
        get_cmake_js_variable("${CMAKE_JS_CONF}" "CMAKE_SHARED_LINKER_FLAGS" CMAKE_SHARED_LINKER_FLAGS)
    elseif((UNIX AND NOT APPLE) OR LINUX)
#        linux
        get_cmake_js_short_variable("${CMAKE_JS_CONF}" "G" CMAKE_GENERATOR)
        get_cmake_js_variable("${CMAKE_JS_CONF}" "CMAKE_JS_VERSION" CMAKE_JS_VERSION)
        get_cmake_js_variable("${CMAKE_JS_CONF}" "CMAKE_BUILD_TYPE" CMAKE_BUILD_TYPE)
        get_cmake_js_variable("${CMAKE_JS_CONF}" "CMAKE_LIBRARY_OUTPUT_DIRECTORY" CMAKE_LIBRARY_OUTPUT_DIRECTORY)
        get_cmake_js_variable("${CMAKE_JS_CONF}" "CMAKE_JS_INC" CMAKE_JS_INC)
        get_cmake_js_variable("${CMAKE_JS_CONF}" "CMAKE_JS_SRC" CMAKE_JS_SRC)
        get_cmake_js_variable("${CMAKE_JS_CONF}" "NODE_RUNTIME" NODE_RUNTIME)
        get_cmake_js_variable("${CMAKE_JS_CONF}" "NODE_RUNTIMEVERSION" NODE_RUNTIMEVERSION)
        get_cmake_js_variable("${CMAKE_JS_CONF}" "NODE_ARCH" NODE_ARCH)
    else()
#        mac
        get_cmake_js_short_variable("${CMAKE_JS_CONF}" "G" CMAKE_GENERATOR)
        get_cmake_js_variable("${CMAKE_JS_CONF}" "CMAKE_JS_VERSION" CMAKE_JS_VERSION)
        get_cmake_js_variable("${CMAKE_JS_CONF}" "CMAKE_BUILD_TYPE" CMAKE_BUILD_TYPE)
        get_cmake_js_variable("${CMAKE_JS_CONF}" "CMAKE_LIBRARY_OUTPUT_DIRECTORY" CMAKE_LIBRARY_OUTPUT_DIRECTORY)
        get_cmake_js_variable("${CMAKE_JS_CONF}" "CMAKE_JS_INC" CMAKE_JS_INC)
        get_cmake_js_variable("${CMAKE_JS_CONF}" "CMAKE_JS_SRC" CMAKE_JS_SRC)
        get_cmake_js_variable("${CMAKE_JS_CONF}" "NODE_RUNTIME" NODE_RUNTIME)
        get_cmake_js_variable("${CMAKE_JS_CONF}" "NODE_RUNTIMEVERSION" NODE_RUNTIMEVERSION)
        get_cmake_js_variable("${CMAKE_JS_CONF}" "NODE_ARCH" NODE_ARCH)
        get_cmake_js_variable("${CMAKE_JS_CONF}" "CMAKE_CXX_FLAGS" CMAKE_CXX_FLAGS)
        get_cmake_js_variable("${CMAKE_JS_CONF}" "CMAKE_SHARED_LINKER_FLAGS" CMAKE_SHARED_LINKER_FLAGS)
    endif()

endif()

set(FXR_DIRS ${CMAKE_CURRENT_SOURCE_DIR}/hostfxr/bin)
find_library(FXR_LIBS nethost HINTS ${FXR_DIRS})

if(WIN32)

    add_compile_definitions(WINDOWS)

    if (CMAKE_BUILD_TYPE MATCHES "(D|d)ebug")
        file(COPY ${CMAKE_CURRENT_SOURCE_DIR}/hostfxr/bin/nethost.lib DESTINATION ${CMAKE_RUNTIME_OUTPUT_DIRECTORY}/Debug)
        file(COPY ${CMAKE_CURRENT_SOURCE_DIR}/hostfxr/bin/nethost.dll DESTINATION ${CMAKE_RUNTIME_OUTPUT_DIRECTORY}/Debug)
    else()
        file(COPY ${CMAKE_CURRENT_SOURCE_DIR}/hostfxr/bin/nethost.dll DESTINATION ${CMAKE_RUNTIME_OUTPUT_DIRECTORY}/Release)
    endif()
elseif((UNIX AND NOT APPLE) OR LINUX)

    add_compile_definitions(LINUX)
    add_compile_definitions(NON_WINDOWS_DEFINE)

    if (CMAKE_BUILD_TYPE MATCHES "(D|d)ebug")
        file(COPY ${CMAKE_CURRENT_SOURCE_DIR}/hostfxr/bin/libnethost.a DESTINATION ${CMAKE_LIBRARY_OUTPUT_DIRECTORY})
    endif()
    file(COPY ${CMAKE_CURRENT_SOURCE_DIR}/hostfxr/bin/libnethost.so DESTINATION ${CMAKE_LIBRARY_OUTPUT_DIRECTORY})
else()
    add_compile_definitions(OSX)
    add_compile_definitions(NON_WINDOWS_DEFINE)

    if (CMAKE_BUILD_TYPE MATCHES "(D|d)ebug")
        file(COPY ${CMAKE_CURRENT_SOURCE_DIR}/hostfxr/bin/libnethost.a DESTINATION ${CMAKE_LIBRARY_OUTPUT_DIRECTORY})
    endif()
    file(COPY ${CMAKE_CURRENT_SOURCE_DIR}/hostfxr/bin/libnethost.dylib DESTINATION ${CMAKE_LIBRARY_OUTPUT_DIRECTORY})
endif()

set(SOURCE_INCLUDE_DIRS ./src ./src/include)

include_directories(${CMAKE_JS_INC} ${CMAKE_BINARY_DIR} ${SOURCE_INCLUDE_DIRS} ${FXR_DIRS})
file(GLOB SOURCE_FILES "src/*.cc")
add_library(${PROJECT_NAME} SHARED ${SOURCE_FILES} ${CMAKE_JS_SRC})
set_target_properties(${PROJECT_NAME} PROPERTIES PREFIX "" SUFFIX ".node")
target_link_libraries(${PROJECT_NAME} ${CMAKE_JS_LIB} ${FXR_LIBS})

# Include Node-API wrappers
execute_process(COMMAND node -p "require('node-addon-api').include"
        WORKING_DIRECTORY ${CMAKE_SOURCE_DIR}
        OUTPUT_VARIABLE NODE_ADDON_API_DIR
        )
string(REGEX REPLACE "[\r\n\"]" "" NODE_ADDON_API_DIR ${NODE_ADDON_API_DIR})

target_include_directories(${PROJECT_NAME} PRIVATE ${NODE_ADDON_API_DIR})

# define NPI_VERSION
add_definitions(-DNAPI_VERSION=7)
#add_definitions(-DNAPI_EXPERIMENTAL)
add_definitions(-DNODE_ADDON_API_DISABLE_DEPRECATED)
add_definitions(-DNAPI_DISABLE_CPP_EXCEPTIONS)