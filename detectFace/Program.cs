using Emgu.CV;
using Emgu.CV.Cuda;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace detectFace
{
    class Program
    {
        static int Main(string[] args)
        {
            IImage image;

            //Read the files as an 8-bit Bgr image  

         //   image = new UMat(args[0], ImreadModes.Color); //UMat version
                                                          image = new Mat(args[0], ImreadModes.Color); //CPU version
            long detectionTime;
            List<Rectangle> faces = new List<Rectangle>();
            List<Rectangle> eyes = new List<Rectangle>();
           
            DetectFace.Detect(
              image, "haarcascade_frontalface_default.xml", "haarcascade_eye.xml",
              faces, eyes,
              out detectionTime);
            Console.WriteLine("{0},{1},{2},{3}", faces.Count, eyes.Count, detectionTime, args[0]);
         //   Console.ReadLine();
            if (faces.Count == 1 && eyes.Count == 2) return 1234;

            return 0;

            //foreach (Rectangle face in faces)
            //    CvInvoke.Rectangle(image, face, new Bgr(Color.Red).MCvScalar, 2);
            //foreach (Rectangle eye in eyes)
            //    CvInvoke.Rectangle(image, eye, new Bgr(Color.Blue).MCvScalar, 2);

            ////display the image 
            //using (InputArray iaImage = image.GetInputArray())
            //    ImageViewer.Show(image, String.Format(
            //       "Completed face and eye detection using {0} in {1} milliseconds",
            //       (iaImage.Kind == InputArray.Type.CudaGpuMat && CudaInvoke.HasCuda) ? "CUDA" :
            //       (iaImage.IsUMat && CvInvoke.UseOpenCL) ? "OpenCL"
            //       : "CPU",
            //       detectionTime));
        }
        bool HaveFace(Image<Bgr, Byte> fname)
        {
            long detectionTime;
            List<Rectangle> faces = new List<Rectangle>();
            List<Rectangle> eyes = new List<Rectangle>();
            DetectFace.Detect(
              fname, "haarcascade_frontalface_default.xml", "haarcascade_eye.xml",
              faces, eyes,
              out detectionTime);
            if (faces.Count == 1 && eyes.Count == 2) return true;

            return false;
        }
    }
}
