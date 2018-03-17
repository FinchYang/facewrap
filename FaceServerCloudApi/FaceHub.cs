using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using log4net;
using Microsoft.AspNet.SignalR;

namespace FaceServer
{
    public class FaceHub : Hub
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public class SignalrClient
        {
            public string UserName { set; get; }
            public string ConnectId { set; get; }
        }
        private static List<SignalrClient> _scList = new List<SignalrClient>();
        public void Login(string username, string cid)
        {
            var found = false;
            foreach (SignalrClient signalrClient in _scList)
            {
                if (cid == signalrClient.ConnectId)
                {
                    found = true;
                }
            }
            if (!found)
            {
                _scList.Add(new SignalrClient { ConnectId = cid, UserName = username });
                Log.Info(string.Format("CscecPushHub {0},{1},{2}  Login,Context.ConnectionId={3}", username, cid, _scList.Count, Context.ConnectionId));
            }
        }
        public void PushMsg(List<string> mt)
        {
            foreach (string userName in mt)
            {
                foreach (SignalrClient signalrClient in _scList)
                {
                    if (signalrClient.UserName == userName)
                    {
                        Clients.Client(signalrClient.ConnectId).NewMsg(mt);
                        //  break;
                    }
                }
            }
        }
        public void CheckMfilesConnect(string username)
        {
            Log.Info(string.Format("CheckMfilesConnect,username= {0}, {1},={2},", username, Context.ConnectionId, _scList.Count));
            var found = false;
            foreach (SignalrClient signalrClient in _scList)
            {
                if (signalrClient.UserName == username)
                {
                    //Log.Info(string.Format("CheckMfilesConnect {0},user , {1},", username, signalrClient.ConnectId));
                    //Clients.Client(signalrClient.ConnectId).CheckMfilesConnect(guid);
                    found = true;
                    break;
                }
            }
            //if (!found)
            //{
            Clients.Client(Context.ConnectionId).haha(found.ToString(), "请运行通知中心Notice，待Notice自动创建项目后进入项目库");
            //Log.Info(string.Format("111 {0},guid = {1},cid={2},", username, guid, Context.ConnectionId));
            //  Clients.Caller.haha("false", "Caller");
            //Log.Info(string.Format("222 {0},guid = {1},cid={2},", username, guid, Context.ConnectionId));
            //Clients.All.haha("true", " Clients.All请运行通知中心Notice，待Notice自动创建项目后进入项目库");
            //Log.Info(string.Format("CheckMfilesConnect,after notice client username= {0},guid = {1},cid={2},", username, guid, Context.ConnectionId));
            //   Clients.Caller.warning("true", "Caller warning");
            //Log.Info(string.Format("333 {0},guid = {1},cid={2},", username, guid, Context.ConnectionId));
            //Clients.All.warning("true", "All warning");
            //Log.Info(string.Format("444 {0},guid = {1},cid={2},", username, guid, Context.ConnectionId));
            //   Clients.Client(Context.ConnectionId).warning("false", "ConnectionId warning");
            //Log.Info(string.Format("555 {0},guid = {1},cid={2},", username, guid, Context.ConnectionId));
            //  }
        }
        public void PushNoticeUpdatePackage(string updateInfo)
        {
            Clients.All.NoticeUpdate(updateInfo);
        }
    }
}