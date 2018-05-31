using Emgu.CV;
using Emgu.CV.Cuda;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using FaceDetection;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace detectface
{
    class Program
    {
        static void Main(string[] args)
        {
            var dirs = new DirectoryInfo(args[0]).GetDirectories();
            foreach(var p in dirs)
            {
               var files= p.GetFiles();
                foreach(var f in files)
                {
                    if (!Run(f.FullName))
                    {
                        Directory.Move(p.FullName, Path.Combine(args[1], p.Name));
                        Console.WriteLine("move--" + p.Name);
                        break;
                    }
                }
            }
        }
        static bool Run(string file)
        {
            IImage image;

            //Read the files as an 8-bit Bgr image  

            image = new UMat(file, ImreadModes.Color); //UMat version
                                                             //image = new Mat("lena.jpg", ImreadModes.Color); //CPU version

            long detectionTime;
            List<Rectangle> faces = new List<Rectangle>();
            List<Rectangle> eyes = new List<Rectangle>();

            DetectFace.Detect(
              image, "haarcascade_frontalface_default.xml", "haarcascade_eye.xml",
              faces, eyes,
              out detectionTime);
            if (faces.Count == 1 && eyes.Count == 2) { return true; }
            else
            {
                Console.WriteLine("no face--" + file);
                return false;
            }
            /*
                        foreach (Rectangle face in faces)
                            CvInvoke.Rectangle(image, face, new Bgr(Color.Red).MCvScalar, 2);
                        foreach (Rectangle eye in eyes)
                            CvInvoke.Rectangle(image, eye, new Bgr(Color.Blue).MCvScalar, 2);

                        //display the image 
                        using (InputArray iaImage = image.GetInputArray())
                            ImageViewer.Show(image, String.Format(
                               "Completed face and eye detection using {0} in {1} milliseconds",
                               (iaImage.Kind == InputArray.Type.CudaGpuMat && CudaInvoke.HasCuda) ? "CUDA" :
                               (iaImage.IsUMat && CvInvoke.UseOpenCL) ? "OpenCL"
                               : "CPU",
                               detectionTime));
                               */
        }
    }
}
