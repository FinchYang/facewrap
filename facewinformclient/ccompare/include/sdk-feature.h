#ifndef _SDK_FEATURE_H_
#define _SDK_FEATURE_H_

#include "sdk-common.h"

#ifdef	__cplusplus
extern "C" {
#endif/*__cplusplus*/

// metadata records the version and analyzer status
typedef struct FaceAnalyzeMetadata {
	version_t			version;
	analyzer_status_t	status;
} FaceAnalyzeMetadata;

typedef struct Feature {
	FaceAnalyzeMetadata	metadata;
	uint64_t			size;				// The size of data byte array
	const void			*data;				// The data byte array
} Feature;

#ifdef	__cplusplus
}
#endif/*__cplusplus*/

#endif/*_SDK_FEATURE_H_*/
