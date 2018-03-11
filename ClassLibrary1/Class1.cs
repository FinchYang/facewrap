using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class Class1
    {
       static string name = ".\\private$\\" + "test";
        public static string CreateNewQueue()
        {
            try
            {
                if (MessageQueue.Exists(name))
                {
                    return (name + "已经存在");
                }
                else
                {
                    var mq = MessageQueue.Create(name);
                    mq.Label = name;
                    return string.Empty;
                }
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }
        private string SendComplexMsg(string context, string id)
        {
            try
            {
                using (var mq = new MessageQueue(name))
                {
                    var msg = new Message
                    {
                        Label = "[face]",
                        Recoverable = true,
                        Body = context,
                        CorrelationId = id
                    };
                    mq.Send(msg);
                    return string.Empty;
                }
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public class mqreturn
        {
            public int status { get; set; }
            public string content { get; set; }
            public string id { get; set; }
        }
        public static mqreturn ReceiveById(string id)
        {
            var ret = new mqreturn { status = 0, content = string.Empty };
            try
            {
                using (var mq = new MessageQueue(name))
                {

                    var m = mq.PeekByCorrelationId(id, TimeSpan.FromSeconds(5));
                    if (m != null)
                    {
                        ret.content = m.Body.ToString();
                    }
                    else ret.status = -1;
                }
                return ret;
            }
            catch(Exception ex)
            {
                ret.status = -2;
                ret.content = ex.Message;
                return ret;
            }
        }

        public static mqreturn ReceiveByPeek()
        {
            var ret = new mqreturn { status = 0, content = string.Empty };
            try
            {
                using (var mq = new MessageQueue(name))
                {

                    var m = mq.Peek();
                    if (m != null)
                    {
                        ret.content = m.Body.ToString();
                        ret.id = m.CorrelationId;
                    }
                    else ret.status = -1;
                }
                return ret;
            }
            catch (Exception ex)
            {
                ret.status = -2;
                ret.content = ex.Message;
                return ret;
            }
        }
    }
}
