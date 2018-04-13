using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace face.model
{
    public class UpdateInfo
    {
        public string Name { get; set; }
        public string Date { get; set; }
        public byte[] FileContent { get; set; }
        public UpdateInfo()
        {
        }
    }
    public class result
    {
        public bool ok { get; set; }
        public float score { get; set; }
        public CompareStatus status { get; set; }
        public mgverror errcode { get; set; }
    }
    public class NoidResultInput
    {
        public string businessnumber { get; set; }
    }
    public class NoidResult
    {
        public string id { get; set; }
        public CompareResult status { get; set; }
    }
    public class NoidInput
    {
        public string id { get; set; }
        public string name { get; set; }
        public byte[] pic1 { get; set; }
        public byte[] pic2 { get; set; }
        public byte[] pic3 { get; set; }
    }
    public enum mgverror
    {
        unkown,
        MGV_ERR = -1,
        MGV_MALLOC_ERR = -2,
        MGV_IMAGE_FORMAT_ERR = -3,
        MGV_PARA_ERR = -4,
        MGV_IMAGE_OUT_OF_RANGE = -5,
        MGV_NO_FACE_DETECTED = -6,
        MGV_MULTIPLE_FACES_DETECTED = -7,
    }
    public struct FaceFile
    {
        public byte[] fcontent;
        public int flen;
    }
    public enum CompareStatus
    {
        unkown, success, failure, uncertainty
    }
    public enum CompareResult
    {
        unknown, success, failure, uncertainty
    }
    public class ComparedInfo
    {
        public string id { get; set; }
        public string name { get; set; }
        public string nation { get; set; }
        public string nationality { get; set; }
        public string address { get; set; }
        public string idaddress { get; set; }
        public string operatingagency { get; set; }
        public string issuer { get; set; }
        public string gender { get; set; }
        public string birthday { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public byte[] idphoto { get; set; }
        public byte[] capturephoto { get; set; }
        //public string idphotofile { get; set; }
        //public string capturephotofile { get; set; }
    }
}
