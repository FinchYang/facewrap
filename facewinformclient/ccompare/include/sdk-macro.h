#pragma once

#ifdef __cplusplus
    #define MGV_API_BEGIN extern "C" {
    #define MGV_API_END }
#else
    #define MGV_API_BEGIN
    #define MGV_API_END
#endif

#if MGV_LIBRARY
    #if defined(_WIN32)
        #if MGV_DLLEXPORT
            #define MGV_API __declspec(dllexport)
        #else
            #define MGV_API __declspec(dllimport)
        #endif
    #else
        #if MGV_DLLEXPORT
            #define MGV_API __attribute__((visibility("default")))
        #else
            #define MGV_API
        #endif
    #endif
#else
    #define MGV_API
#endif

#ifdef __GNUC__
    #define MGV_DEPRECATED __attribute__((deprecated))
#elif defined(_MSC_VER)
    #define MGV_DEPRECATED __declspec(deprecated)
#else
    #define MGV_DEPRECATED
#endif
