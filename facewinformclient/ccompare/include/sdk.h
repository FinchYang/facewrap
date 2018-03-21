#ifndef _SDK_H_
#define _SDK_H_

#include "sdk-macro.h"
#include "sdk-common.h"
#include "sdk-feature.h"

#ifdef	__cplusplus
extern "C" {
#endif/*__cplusplus*/

typedef struct Engine Engine;
typedef struct Stream Stream;
typedef struct AnalyzerDevice {
	analyzer_t				analyzer_type;		// the analyzer's type
	device_t				device_type;		// the analyzer's device type
	int64_t					device_parameter;	// If device_type == td_cpu, the parameter is the number of CPUs. 
												// If device_type == td_gpu, the parameter is the id of GPU.
} AnalyzerDevice;

typedef struct Filter {
	float					pose_roll_upper_threshold;
	float					pose_yaw_upper_threshold;
	float					pose_pitch_upper_threshold;
	float					blurriness_upper_threshold;
} Filter;

extern Filter filter_disabled;

typedef struct Face {
	struct Age {
		FaceAnalyzeMetadata	metadata;
		float				value;
	}						age;				//unrealized

	struct Gender {
		FaceAnalyzeMetadata	metadata;
		int					value;
		float				male_prob;
		float				female_prob;
	}						gender;				//unrealized

	struct Blurriness {
		FaceAnalyzeMetadata	metadata;
		float				value;
	}						blurriness;
	
	struct EyeStatus {
		FaceAnalyzeMetadata	metadata;
		int32_t				left;
		int32_t				right;
	}						eye;				//unrealized

	struct Minority {
		FaceAnalyzeMetadata	metadata;
		float				minority_prob;
	}						minority;			//unrealized

	struct Mouth {
		FaceAnalyzeMetadata	metadata;
		float				mask_prob;
		float				close_prob;
		float				open_prob;
		float				other_prob;
	}						mouth;				//unrealized

	struct Pose {
		FaceAnalyzeMetadata	metadata;
		float				roll;
		float				yaw;
		float				pitch;
	}						pose;

	struct Quality {
		FaceAnalyzeMetadata	metadata;
		float				value;
	}						quality;			//unrealized

	struct Detection {
		FaceAnalyzeMetadata	metadata;
		uint64_t			track_id;
		float				confidence;
		struct Rect4I {
			int             left;
			int             top;
			int             right;
			int             bottom;
		}					rect;
	}						detection;

	struct Landmark {
		FaceAnalyzeMetadata	metadata;
		const int32_t 		*tags;
		const struct Point2F {
			float			x;
			float			y;
		} 					*points;
		uint32_t			size;
	}						landmark;

	Feature					feature;
} Face;

#if	defined(__cplusplus)

#elif defined(__cgo__)
typedef struct Point2F		Point2F;
#else
#endif/*__cplusplus*/

struct Frame;
typedef struct Blob {
	const void				*data;				// Data of the blob
	uint32_t				size;				// Size of the blob
	void					*release_handler;	// The handler for releaing the blob.
} Blob;

typedef struct Frame {
	uint64_t				frameid;			// The index of the frame
	uint64_t				time;				// The absolute capturing time of the frame (in microseconds)
	uint64_t				time_duration;		// The duration between the capturing time and the stream's creation time (in microseconds)
	uint32_t				pts;				// The decoded pts
	uint32_t				key_frame;			// 1 - the frame is key frame. 0 - otherwise
	uint32_t				width;				// The height of the Frame.
	uint32_t				height;				// The width of the Frame.

	const Face				*face_list;			// The face list
	uint64_t				face_size;			// The size of the face list

	Blob					packet;				// unrealized
	Blob					gray;
	Blob					bgr;
} Frame;

/// @param[in] level: if level == 1, display log on stdout. if level == 0, display no log.
MGV_API void mgv_set_log(int level);

/// @param[in] data_path: the path of the input model file
/// @param[in] analyzer_device_list: the list of computing device
/// @param[in] analyzer_devlce_list_size: the number of devices. If zero, the default device configuration is selected, and `analyzer_device_list` is not used.
/// @param[in] decode_gpu_list: the list of decode device. Set NULL if decoding is not within the engine.
/// @param[in] decode_gpu_list_size: the number of decode devices
/// @param[out] pengine: point to a pointer of the engine
/// @return: return 0 if everything is fine. Return negative when any error happens.
MGV_API int mgv_create_engine(const char *model_path, Engine **pengine);

/// @param[in] engine: the target engine
/// @param[in] attr: the desired attribute
/// @param[out] ival: an integral attribute
/// @param[out] sval: a textual attribute
/// @param[out] fval: a floating-point attribute
/// @return: return 0 if everything is fine. Return negative when any error happens.
/// @note: returning which value depends on attr.
/// @see engine_attribute_t
MGV_API int mgv_get_engine_attribute(Engine *engine, engine_attribute_t attr, int64_t *ival, const char **sval, double *fval);

/// @param[in] engine: the target engine to be destroyed.
/// @return: return 0 if everything is fine. Return negative when any error happens.
MGV_API int mgv_destroy_engine(Engine *engine);

/// @param[in] engine: the engine for face tracking & detection
/// @param[in] id: the unique ID for identify the stream
/// @param[in] filter: the filter for correct the face. If filter == NULL, then we use default filter parameter. To disable filter, please pass &filter_disabled.
/// @param[in] expiration_drop: the tolerable avaiable delay. If expiration_drop == NULL, then we use default dispatch expiration. If *expiration_drop == 0, then there is no expiration. 
/// @param[in] expiration_dispatch: the tolerable dispatch delay. If expiration_dispatch == NULL, then we use default drop expiration. If *expiration_dispatch == 0, then there is no expiration.
/// @param[out] pstream: the created stream
/// @return: return 0 if everything is fine. Return negative when any error happens.
MGV_API int mgv_create_stream(Engine *engine, const char *id, Filter *filter, int32_t *expiration_drop, int32_t *expiration_dispatch, Stream **pstream);

/// @param[in] stream: the target stream
/// @param[in] status: the status wanted
/// @param[out] ival:
/// @param[out] sval:
/// @param[out] fval:
/// @return: return 0 if everything is fine. Return negative when any error happens.
/// @note: returning which value depends on status.
/// @see stream_status_t
MGV_API int mgv_get_stream_status(Stream *stream, stream_status_t status, int64_t *ival, const char **sval, double *fval);

/// @param[in] stream: the target stream to be stopped.
/// @return: return 0 if everything is fine. Return negative when any error happens.
MGV_API int mgv_stop_stream(Stream *stream);

/// @param[in] stream: the target stream to be closed.
/// @return: return 0 if everything is fine. Return negative when any error happens.
MGV_API int mgv_close_stream(Stream *stream);

/// @param[in] stream: the target stream
/// @param[in] url: the desired URL.
/// @return: return 0 if everything is fine. Return negative when any error happens.
MGV_API int mgv_start_url(Stream *stream, const char *url);

/// @param[in] stream: the target stream
/// @param[in] bgr: the frame in bgr format
/// @param[in] gray: the frame in gray format
/// @param[in] width: the width of the frame
/// @param[in] height: the height of the frame. `bgr` == NULL && `gray` == NULL && `width` == 0 && `height` == 0 indicates `mgv_put_frame` finishes.
/// @return: return 0 if everything is fine. Return negative when any error happens. 
/// @note: when `mgv_put_frame` is invoked too frequently, we compute in 25FPS.
MGV_API int mgv_put_frame(Stream *stream, void *bgr, void *gray, int32_t width, int32_t height);

/// @param[in] stream: the target stream
/// @param[in] packet: the raw H.264 packet
/// @param[in] size: the size of the packet
/// @param[in] width: the width of the frame
/// @param[in] height: the height of the frame. `packet` == NULL && `size` == 0 && `width` == 0 && `height` == 0 indicates `mgv_put_packet` finishes.
/// @param[in] decode_device: positive(the index of the decoding GPU); `decode_gpu_any`: let the engine to determine which GPU; `decode_cpu_any`: use a CPU to decode. 
///		`decode_device` in the first calling of `mgv_put_packet` is used. 
/// @return: return 0 if everything is fine. Return negative when any error happens. 
/// @note: when `mgv_put_packet` is invoked too frequently, we compute in 25 FPS.
MGV_API int mgv_put_packet(Stream *stream, void *packet, int32_t size, int32_t width, int32_t height);

/// @param[in] stream: the target stream
/// @param[in] block: 1: wait_frame blocks until any error happens or next frame is ready. 0: no block.
/// @param[out] pframe: a pointer that points to the analyzed frame
/// @return: 0(normal), EOF(end of stream), other negative number(other error)
MGV_API int mgv_wait_frame(Stream *stream, int block, Frame **pframe);

/// @param[in] engine: the engine
/// @parma[in] bgr: the frame in bgr format
/// @param[in] gray: the frame in gray format
/// @param[in] width: the width of the frame
/// @param[in] height: the height of the frame
/// @param[in] filter: the filter that if the face can pass the filter, we analyze the face, including detection, pose, blurriness, age, gender, minority, eye, mouth, and feature.
/// @param[in] mask: a bitwise mask of analyzer that determines which analyzing is performed. Only the following can be combined. Note that detection is always performed.
///		* ta_age_gender
///		* ta_blurriness
///		* ta_eye_status
///		* ta_minority
///		* ta_mouth
///		* ta_pose
///		* ta_quality
///		* ta_feature
/// For exampke, (1 << ta_age_gender) | (1 << ta_blurriness) denotes that age, gender and blurriness are analyzed.
/// @param[out] pframe: a pointer that points to the analyzed frame, including detection, pose, blurriness, age, gender, minority, eye, mouth, and feature.
/// @return: 0(normal), other negative number(other error)
MGV_API int mgv_analyze_image(Engine *engine, void *bgr, void *gray, int32_t width, int32_t height, Filter *filter, uint32_t mask, Frame **pframe);

/// @param[in] engine: the engine
/// @param[in] fm: the left face
/// @param[in] fn: the right face
/// @param[out] result: a pointer that points to the result
/// @return: 0(normal), other negative number (other error)
MGV_API int mgv_compare_face(Engine *engine, Face *fm, Face *fn, float *result);

/// @param[in] engine: the engine
/// @param[in] fm: the left feature
/// @param[in] fn: the right feature
/// @param[out] result: a pointer that points to the result
/// @return: 0(normal), other negative number (other error)
MGV_API int mgv_compare_face_feature(Engine *engine, Feature *fm, Feature *fn, float *result);

/// @param[in] engine: the engine
/// @param[in] fm: a pointer that points to the array of the left M features
/// @param[in] m: the size of the left M features
/// @param[in] fn: a pointer that points to the array of the right N features
/// @param[in] n: the size of the right N features
/// @param[out] result: a pointer that points to an big enough memory that can store (m x n) floats. result[i x n + j] denotes the comparison result between the i-th feature in fm and the j-th feature in fn.
/// @return: 0(normal), other negative number (other error).
MGV_API int mgv_compare_face_feature_mn(Engine *engine, Feature *fm, int32_t m, Feature *fn, int32_t n, float *result);

/// @param[in] frame: the target frame to be released.
/// @return: return 0 if everything is fine. Return negative when any error happens.
/// @note: this function will utilimately everything in the `frame`, including the blobs that are not freed in the `frame`.
MGV_API int mgv_release_frame(Frame *frame);

/// @param[in] blob: the address of the blob
/// @return: return 0 if everything is fine. Return negative when any error happens. release_blob should set data & release_handler to be NULL.
MGV_API int mgv_release_blob(Blob *blob);

/// @param[in] err: the number of the error
/// @return: return NULL if the error is not a defined error; valid string if the error is a defined error.
MGV_API const char *mgv_get_error_str(int err);

MGV_API int tool_draw_rect_yuv_image(char *addr, int height, int width, int left, int top, int right, int bottom, int color);
MGV_API int tool_cut_rect_image_bgr_to_rgb(char *addr, int height, int width, int left, int top, int right, int bottom, char* dst_addr, int*dst_height, int*dst_width);
#ifdef	__cplusplus
}
#endif/*__cplusplus*/

#endif/*_SDK_H_*/

