#ifndef _SDK_COMMON_H_
#define _SDK_COMMON_H_

#include <stdint.h>

#ifdef	__cplusplus
extern "C" {
#endif/*__cplusplus*/

typedef int32_t	version_t;

typedef enum analyzer_t {
	ta_age_gender,
	ta_blurriness,
	ta_eye_status,
	ta_minority,
	ta_mouth,
	ta_pose,
	ta_quality,
	ta_feature,
	ta_detector,
	ta_landmarker,
	ta_tracker,

	ta_pedestrian,
	ta_vehicle,
	ta_lisence,
	ta_flag,
} analyzer_t;									// unrealized

typedef enum analyzer_status_t {	
	tas_not_supported,							// the analyzer is currently not supported	
	tas_analyzed_qualified,						// the analyzed result passes the filter
	tas_analyzed_filtered,						// the analyzed result fails to pass the filter

	tas_timeout,								// the analyzer is not run due to timeout
	tas_worse_than_best,						// the analyzer is not run due to the policy of selecting the best

	tas_not_analyzed_filtered_by_pose,			// the analyzer is not run due to being filtered by previouse pose filter
	tas_not_analyzed_filtered_by_blurriness,	// the analyzer is not run due to being filtered by previouse blurriness filter

	tas_not_analyzed_timeout_by_pose,			// the analyzer is not run due to timeout in previouse pose filter
	tas_not_analyzed_timeout_by_blurriness,     // the analyzer is not run due to timeout in previouse blurriness filter

	tas_not_specified,							// the analyzer is not run due to unspecified in the image analyzing interface

	tas_unknown_status,							// an unknown status
} analyzer_status_t;							// unrealized

typedef enum model_file_t {
	tmf_file				= 'f',				// the model file should be load as file
	tmf_memory 				= 'm',				// the model file should be load as memory block
} model_file_t;

typedef enum device_t {
	td_cpu 					= 'c',				// the computation is performed on cpu
	td_gpu 					= 'g',				// the computation is performed on gpu
} device_t;

enum {
	decode_cpu_any = -1,		// any device for decoding using CPU
	decode_gpu_any = -2,		// any device for decoding using GPU
};

typedef enum sdk_error_t {
	ERROR_BEGIN = -1000,											// Start of sdk error
	ERROR_CREATE_ENGINE_MEGFACE_LIBRARY_NOT_FOUND,					// Can not find megface library for the engine. 
	ERROR_CREATE_ENGINE_MODEL_FILE_NOT_FOUND,						// Can not find the model file for the engine.
	ERROR_CREATE_ENGINE_INTERNAL_ERROR,								// Internal error in create engine.
	ERROR_CREATE_ENGINE_INVALID_ANALYZER_TYPE,						// Invalid analyzer's type.
	ERROR_CREATE_ENGINE_INVALID_MODEL_TYPE,							// Invalid model's type.
	ERROR_CREATE_ENGINE_INVALID_DEVICE,								// Invalid device's type.
	ERROR_CREATE_ENGINE_MULTIPLE_ENGINE,							// More than one engine is created. 
	ERROR_CREATE_ENGINE_INVALID_DECODE_DEVICE,						// Invalid decode device or no decode device is passed.
	ERROR_CREATE_STREAM_MAX_STREAM_REACHED,							// Can not create more stream due to the stream limitation.
	ERROR_CREATE_STREAM_CONFLICT_ID,								// Conflict ID.
	ERROR_INVALID_STREAM,											// Stream is invalid.
	ERROR_STREAM_STARTED,											// Stream has started.
	ERROR_STREAM_NOT_STARTED,										// Stream has not started.
	ERROR_STREAM_STOPPED,											// Stream has stopped.
	ERROR_STREAM_NOT_STOPPED,										// Stream has not stopped.
	ERROR_STREAM_CLOSED,											// Stream has been closed.
	ERROR_STREAM_EOF,												// Stream ends.
	ERROR_STREAM_INTERRUPT,											// Stream is interruptted.
	ERROR_STREAM_BAD_URL,											// The URL for the stream is bad.
	ERROR_STREAM_INCONSISTENT_FRAME,								// The width or height of frame is not consistent.
	ERROR_STREAM_INCONSISTENT_DECODER,								// The decoder of the stream is not consistent.
	ERROR_STREAM_BAD_FRAME,											// Not an legal frame. 
	ERROR_STREAM_BAD_BLOB,											// Not an legal blob.
	ERROR_STREAM_NEED_MORE_FRAME,									// Need more frames before wait_frame.
	ERROR_STREAM_STILL_EXISTS,										// Some stream still exists before destroy engine.
	ERROR_ENGINE_INVALID_ATTRIBUTE,									// Invalid attribute for engine
	ERROR_STREAM_INVALID_STATUS,									// Invalid status for stream
	ERROR_MEMORY_ALREADY_RELEASED,									// Memory is already released. 
	ERROR_INTERNAL,													// Internal error
	ERROR_INVALID_ENGINE,											// Engine is invalid
	ERROR_INVALID_FEATURE_VERSION,									// The version of the feature is invalid
	ERROR_CODEMETER_NOTFOUND,										// The CodeMeter is not found
	ERROR_CODEMETER_EXPIRED,										// The CodeMeter expired
	ERROR_CODEMETER_DATA,											// The CodeMeter data is not correct
	ERROR_END,														// End of sdk error
} sdk_error_t;

typedef enum engine_attribute_t {
	tea_landmark_count,							// Get the count of landmark map. Result written in `ival`
	tea_feature_size,							// Get the dimension of feature. Result written in `ival`
} engine_attribute_t;

typedef enum stream_status_t {
	tss_live_frames,							// Get the number of total living frames in the stream. Result written in `ival`
} stream_status_t;								// unrealized

#ifdef	__cplusplus
}
#endif/*__cplusplus*/

#endif/*_SDK_COMMON_H_*/
