//#pragma once
//
//// Including SDKDDKVer.h defines the highest available Windows platform.
//
//// If you wish to build your application for a previous Windows platform, include WinSDKVer.h and
//// set the _WIN32_WINNT macro to the platform you wish to support before including SDKDDKVer.h.
//
//#include <SDKDDKVer.h>
//
//#define WIN32_LEAN_AND_MEAN             // Exclude rarely-used stuff from Windows headers
//// Windows Header Files:
//#include <windows.h>

#include <dlib/dnn.h>
#include <dlib/gui_widgets.h>
#include <dlib/clustering.h>
#include <dlib/string.h>
#include <dlib/image_io.h>
#include <dlib/image_processing/frontal_face_detector.h>

extern "C" __declspec(dllexport) int __stdcall Addfunc(int n1, int n2);

int __stdcall Addfunc(int a, int b)
{
	return a + b;
}


using namespace dlib;
using namespace std;


template <template <int,template<typename>class,int,typename> class block, int N, template<typename>class BN, typename SUBNET>
using residual = add_prev1<block<N,BN,1,tag1<SUBNET>>>;

template <template <int,template<typename>class,int,typename> class block, int N, template<typename>class BN, typename SUBNET>
using residual_down = add_prev2<avg_pool<2,2,2,2,skip1<tag2<block<N,BN,2,tag1<SUBNET>>>>>>;

template <int N, template <typename> class BN, int stride, typename SUBNET> 
using block  = BN<con<N,3,3,1,1,relu<BN<con<N,3,3,stride,stride,SUBNET>>>>>;

template <int N, typename SUBNET> using ares      = relu<residual<block,N,affine,SUBNET>>;
template <int N, typename SUBNET> using ares_down = relu<residual_down<block,N,affine,SUBNET>>;

template <typename SUBNET> using alevel0 = ares_down<256,SUBNET>;
template <typename SUBNET> using alevel1 = ares<256,ares<256,ares_down<256,SUBNET>>>;
template <typename SUBNET> using alevel2 = ares<128,ares<128,ares_down<128,SUBNET>>>;
template <typename SUBNET> using alevel3 = ares<64,ares<64,ares<64,ares_down<64,SUBNET>>>>;
template <typename SUBNET> using alevel4 = ares<32,ares<32,ares<32,SUBNET>>>;

using anet_type = loss_metric<fc_no_bias<128,avg_pool_everything<
                            alevel0<
                            alevel1<
                            alevel2<
                            alevel3<
                            alevel4<
                            max_pool<3,3,2,2,relu<affine<con<32,7,7,2,2,
                            input_rgb_image_sized<150>
                            >>>>>>>>>>>>;


extern "C" __declspec(dllexport) char* __stdcall compare(char* file1, char* file2);

char * __stdcall compare(char* file1, char* file2) try
{
    frontal_face_detector detector = get_frontal_face_detector();
    // We will also use a face landmarking model to align faces to a standard pose:  (see face_landmark_detection_ex.cpp for an introduction)
    shape_predictor sp;
    deserialize("shape_predictor_5_face_landmarks.dat") >> sp;
    // And finally we load the DNN responsible for face recognition.
    anet_type net;
    deserialize("dlib_face_recognition_resnet_model_v1.dat") >> net;

    matrix<rgb_pixel> img1,img2;
    load_image(img1, file1);
	load_image(img2, file2);
    // Display the raw image on the screen
   // image_window win(img); 

    // Run the face detector on the image of our action heroes, and for each face extract a
    // copy that has been normalized to 150x150 pixels in size and appropriately rotated
    // and centered.
    std::vector<matrix<rgb_pixel>> faces;
    for (auto face : detector(img1))
    {
        auto shape = sp(img1, face);
        matrix<rgb_pixel> face_chip;
        extract_image_chip(img1, get_face_chip_details(shape,150,0.25), face_chip);
        faces.push_back(move(face_chip));
        // Also put some boxes on the faces so we can see that the detector is finding
        // them.
      //  win.add_overlay(face);
    }

    if (faces.size() == 0)
    {
      //  cout << "No faces found in image1!" << endl;
        return "No faces found in image1!";
    }

	std::vector<matrix<rgb_pixel>> faces2;
	for (auto face : detector(img2))
	{
		auto shape = sp(img2, face);
		matrix<rgb_pixel> face_chip;
		extract_image_chip(img2, get_face_chip_details(shape, 150, 0.25), face_chip);
		faces2.push_back(move(face_chip));
		// Also put some boxes on the faces so we can see that the detector is finding
		// them.
		//  win.add_overlay(face);
	}

	if (faces2.size() == 0)
	{
		//cout <<  << endl;
		return "No faces found in image2!";
	}
    // This call asks the DNN to convert each face image in faces into a 128D vector.
    // In this 128D vector space, images from the same person will be close to each other
    // but vectors from different people will be far apart.  So we can use these vectors to
    // identify if a pair of images are from the same person or from different people.  
    std::vector<matrix<float,0,1>> face_descriptors = net(faces);
	std::vector<matrix<float, 0, 1>> face_descriptors2 = net(faces2);
	double v = length(face_descriptors[0] - face_descriptors2[0]);
	double similarity = 1 - v * 0.6;
	/*if (v < 0.6) {
		cout << "hit enter to terminate" << endl;
	}
	else*/
	//	cout << "hit enter to terminate"<< similarity << endl;
	if (similarity > 0.7)
		return "";
	return "not one";
}
catch (std::exception& e)
{
  //  cout << e.what() << endl;
	return "error";
}

