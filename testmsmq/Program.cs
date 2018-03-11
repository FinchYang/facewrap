using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace testmsmq
{
    class Program
    {
        static void Main(string[] args)
        {
            // Console.WriteLine(MsmqOps.SendComplexMsg(MsmqOps.sourceQueueName, "haha", "hehe"));
            var guid = Guid.NewGuid();
            using (var mq = new MessageQueue(MsmqOps.sourceQueueName))
            {
                var msg = new Message
                {
                    Body = "hdsfaf",
                    ConnectorType=guid,
                   // CorrelationId=guid.ToString("N")
                };
                Console.WriteLine("messsage id="+msg.Id);
                mq.Send(msg);
                Console.WriteLine("after mqsend");
                //var ret= mq.ReceiveByCorrelationId();
                //Console.WriteLine("recv content"+ret.content);
            }
           
            Console.ReadLine();
        }
        public class mqreturn
        {
            public int status { get; set; }
            public string content { get; set; }
            public string id { get; set; }
        }
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
}
