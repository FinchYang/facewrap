using System;
using System.Messaging;


namespace FaceServer
{
    public class MsmqOps
    {
        public static string sourceQueueName = ".\\private$\\" + "FaceSource";
        public static string resultQueueName = ".\\private$\\" + "FaceResult";
        public static string CreateNewQueue(string qname)
        {
            try
            {
                if (MessageQueue.Exists(qname))
                {
                    return (qname + "已经存在");
                }
                else
                {
                    var mq = MessageQueue.Create(qname);
                    mq.Label = qname;
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static string SendComplexMsg(string qname, string context, string id)
        {
            try
            {
                using (var mq = new MessageQueue(qname))
                {
                    var msg = new Message
                    {                        
                        Label = id,
                        Recoverable = true,
                        Body = context,
                        CorrelationId = id
                    };
                    mq.Send(msg);
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static mqreturn ReceiveById(string qname, string id)
        {
            var ret = new mqreturn { status = 0, content = string.Empty };
            try
            {
                using (var mq = new MessageQueue(qname))
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
            catch (Exception ex)
            {
                ret.status = -2;
                ret.content = ex.Message;
                return ret;
            }
        }

        public static mqreturn ReceiveByPeek(string qname)
        {
            var ret = new mqreturn { status = 0, content = string.Empty };
            try
            {
                using (var mq = new MessageQueue(qname))
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
