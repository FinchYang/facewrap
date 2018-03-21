#ifndef _FACE_SDK_H_
#define _FACE_SDK_H_

#include "sdk-macro.h"

#ifdef	__cplusplus
extern "C" {
#endif/*__cplusplus*/

#define MGV_ERR							-1
#define MGV_MALLOC_ERR					-2
#define MGV_IMAGE_FORMAT_ERR			-3
#define MGV_PARA_ERR					-4
#define MGV_IMAGE_OUT_OF_RANGE			-5
#define MGV_NO_FACE_DETECTED			-6
#define MGV_MULTIPLE_FACES_DETECTED		-7

MGV_API int InitSDK(const char* parser, int length);

MGV_API int DeInitSDK();

MGV_API int SetLogLevel(int level);

MGV_API int GetFeatureFromJpeg(unsigned char *jpeg, int len_jpeg, unsigned char *feature_buf, int len_buf);

MGV_API float CalcFeatureSimilarity(unsigned char *feat1, int len1, unsigned char *feat2, int len2);

#ifdef	__cplusplus
}
#endif/*__cplusplus*/

#endif