﻿using System;
using System.IO;
using System.Net;
using System.Text;

namespace watchinternet
{
    public class NoidInput
        {
            public string id { get; set; }
            public byte[] pic { get; set; }
        }
    class Program
    {
        private static string FtpServerIP = "localhost";//, 
        private static string FtpUserID = "user";//FtpUserID, FtpPassword
        private static string FtpPassword = "pass";//FtpUserID, FtpPassword
        static void Main(string[] args)
        {
            do
            {
                var flist = GetFileList("*.*");
                foreach (var f in flist)
                {
                    if (FtpDownload(f, f, true))
                    {
                        Delete(f);
                        var lines = File.ReadAllLines(f);
                        foreach (var l in lines)
                        {
                            var input=Newtonsoft.Json. JsonConvert.DeserializeObject<NoidInput>(l);
                            //invoke domicile interface
                            //invoke face++ compare
                            var oneperson=true;
                            if(oneperson){

                            }
                            //db process
                            var temp=Path.GetTempFileName();
                            File.WriteAllText(temp,string.Format("{0},{1}",input.id,1));
                            FtpUploadFile(temp);
                            //return internet
                        }
                    }
                }
                System.Threading.Thread.Sleep(1000 * 10);
            } while (true);
        }
/// <summary>
        /// 上传文件到FTP服务器
        /// </summary>
        /// <param name="localFullPath">本地带有完整路径的文件名</param>
        /// <param name="updateProgress">报告进度的处理(第一个参数：总大小，第二个参数：当前进度)</param>
        /// <returns>是否下载成功</returns>
        public static bool FtpUploadFile(string localFullPathName, Action<int, int> updateProgress = null)
        {
            FtpWebRequest reqFTP;
            Stream stream = null;
            FtpWebResponse response = null;
            FileStream fs = null;
            try
            {
                FileInfo finfo = new FileInfo(localFullPathName);
                if (FtpServerIP == null || FtpServerIP.Trim().Length == 0)
                {
                    throw new Exception("ftp上传目标服务器地址未设置！");
                }
                Uri uri = new Uri("ftp://" + FtpServerIP + "/" + finfo.Name);
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(uri);
                reqFTP.KeepAlive = false;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(FtpUserID, FtpPassword);//用户，密码
                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;//向服务器发出下载请求命令
                reqFTP.ContentLength = finfo.Length;//为request指定上传文件的大小
                response = reqFTP.GetResponse() as FtpWebResponse;
                reqFTP.ContentLength = finfo.Length;
                int buffLength = 1024;
                byte[] buff = new byte[buffLength];
                int contentLen;
                fs = finfo.OpenRead();
                stream = reqFTP.GetRequestStream();
                contentLen = fs.Read(buff, 0, buffLength);
                int allbye = (int)finfo.Length;
                //更新进度 
                if (updateProgress != null)
                {
                    updateProgress((int)allbye, 0);//更新进度条 
                }
                int startbye = 0;
                while (contentLen != 0)
                {
                    startbye = contentLen + startbye;
                    stream.Write(buff, 0, contentLen);
                    //更新进度 
                    if (updateProgress != null)
                    {
                        updateProgress((int)allbye, (int)startbye);//更新进度条 
                    }
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                stream.Close();
                fs.Close();
                response.Close();
                return true;

            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
                if (stream != null)
                {
                    stream.Close();
                }
                if (response != null)
                {
                    response.Close();
                }
            }
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileName"></param>
        public static bool Delete(string fileName)
        {
            try
            {
                string uri = "ftp://" + FtpServerIP + "/" + fileName;
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));

                reqFTP.Credentials = new NetworkCredential(FtpUserID, FtpPassword);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;

                string result = String.Empty;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                long size = response.ContentLength;
                Stream datastream = response.GetResponseStream();
                StreamReader sr = new StreamReader(datastream);
                result = sr.ReadToEnd();
                sr.Close();
                datastream.Close();
                response.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }
        /// <summary>
        /// 从FTP服务器下载文件，指定本地路径和本地文件名
        /// </summary>
        /// <param name="remoteFileName">远程文件名</param>
        /// <param name="localFileName">保存本地的文件名（包含路径）</param>
        /// <param name="ifCredential">是否启用身份验证（false：表示允许用户匿名下载）</param>
        /// <param name="updateProgress">报告进度的处理(第一个参数：总大小，第二个参数：当前进度)</param>
        /// <returns>是否下载成功</returns>
        public static bool FtpDownload(string remoteFileName, string localFileName, bool ifCredential, Action<int, int> updateProgress = null)
        {
            FtpWebRequest reqFTP, ftpsize;
            Stream ftpStream = null;
            FtpWebResponse response = null;
            FileStream outputStream = null;
            try
            {

                outputStream = new FileStream(localFileName, FileMode.Create);
                if (FtpServerIP == null || FtpServerIP.Trim().Length == 0)
                {
                    throw new Exception("ftp下载目标服务器地址未设置！");
                }
                Uri uri = new Uri("ftp://" + FtpServerIP + "/" + remoteFileName);
                ftpsize = (FtpWebRequest)FtpWebRequest.Create(uri);
                ftpsize.UseBinary = true;

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(uri);
                reqFTP.UseBinary = true;
                reqFTP.KeepAlive = false;
                if (ifCredential)//使用用户身份认证
                {
                    ftpsize.Credentials = new NetworkCredential(FtpUserID, FtpPassword);
                    reqFTP.Credentials = new NetworkCredential(FtpUserID, FtpPassword);
                }
                ftpsize.Method = WebRequestMethods.Ftp.GetFileSize;
                FtpWebResponse re = (FtpWebResponse)ftpsize.GetResponse();
                long totalBytes = re.ContentLength;
                re.Close();

                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                response = (FtpWebResponse)reqFTP.GetResponse();
                ftpStream = response.GetResponseStream();

                //更新进度 
                if (updateProgress != null)
                {
                    updateProgress((int)totalBytes, 0);//更新进度条 
                }
                long totalDownloadedByte = 0;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];
                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    totalDownloadedByte = readCount + totalDownloadedByte;
                    outputStream.Write(buffer, 0, readCount);
                    //更新进度 
                    if (updateProgress != null)
                    {
                        updateProgress((int)totalBytes, (int)totalDownloadedByte);//更新进度条 
                    }
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }
                ftpStream.Close();
                outputStream.Close();
                response.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
            finally
            {
                if (ftpStream != null)
                {
                    ftpStream.Close();
                }
                if (outputStream != null)
                {
                    outputStream.Close();
                }
                if (response != null)
                {
                    response.Close();
                }
            }
        }

        /// <summary>
        /// 获取当前目录下文件列表(仅文件)
        /// </summary>
        /// <returns></returns>
        public static string[] GetFileList(string mask)
        {
            string[] downloadFiles;
            StringBuilder result = new StringBuilder();
            FtpWebRequest reqFTP;
            try
            {
                Uri ftpURI = new Uri("ftp://" + FtpServerIP + "/");
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(ftpURI);
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(FtpUserID, FtpPassword);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);

                string line = reader.ReadLine();
                while (line != null)
                {
                    if (mask.Trim() != string.Empty && mask.Trim() != "*.*")
                    {

                        string mask_ = mask.Substring(0, mask.IndexOf("*"));
                        if (line.Substring(0, mask_.Length) == mask_)
                        {
                            result.Append(line);
                            result.Append("\n");
                        }
                    }
                    else
                    {
                        result.Append(line);
                        result.Append("\n");
                    }
                    line = reader.ReadLine();
                }
                result.Remove(result.ToString().LastIndexOf('\n'), 1);
                reader.Close();
                response.Close();
                return result.ToString().Split('\n');
            }
            catch (Exception ex)
            {
                downloadFiles = null;
                throw ex;
            }
        }
    }
}
