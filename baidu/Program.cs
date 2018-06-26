using com.baidu.ai;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace baidu
{
    class Program
    {
        static void Main(string[] args)
        {
            var to = AccessToken.getAccessToken();
            if (to.ok)
            {
                if (args[0] == "group")
                {
                    GroupAdd.groupAdd(to.access_token, args[1]);
                    return;
                }
                if (args[0] == "oneofnbatch")
                {
                    var files = new DirectoryInfo(args[2]).GetFiles("*_2.jpg");
                    var allsear = 0;
                    var oksear = 0;
                    var exp = 0;
                    foreach (var f in files)
                    {
                        allsear++;
                        try
                        {
                            var oneofnreq = new oneofnreq
                            {
                                image = Convert.ToBase64String(File.ReadAllBytes(f.FullName)),
                                image_type = "BASE64",
                                group_id_list = args[1],
                            };
                            var retsear = FaceSearch.search(to.access_token, JsonConvert.SerializeObject(oneofnreq));
                           
                            if (retsear.Contains(f.Name.Replace("_2.jpg", "_1")))
                            {
                                oksear++;
                                Console.WriteLine("true---{0},{1},{2}", oksear, allsear, f.Name);
                            }
                            else
                            {
                                Console.WriteLine("false---{0},{1},{2}", oksear, allsear, f.Name);
                            }
                        }
                        catch(Exception ex)
                        {
                            exp++;
                            Console.WriteLine("exeption---{0},{1},{2}", f.Name, exp, ex.Message);
                        }
                     //   Thread.Sleep(500);
                    }
                    return;
                }
                if (args[0] == "oneofn")
                {
                    var oneofnreq = new oneofnreq
                    {
                        image = Convert.ToBase64String(File.ReadAllBytes(args[2])),
                        image_type = "BASE64",
                        group_id_list = args[1],
                    };
                    FaceSearch.search(to.access_token, JsonConvert.SerializeObject(oneofnreq));
                    return;
                }
                if (args[0] == "getgroups")
                {
                    GroupGetlist.Getlist(to.access_token);
                    return;
                }
                if (args[0] == "getusers")
                {
                    GroupGetusers.getUsers(to.access_token, args[1]);
                    return;
                }
                if (args[0] == "faceregbatch")
                {
                    var files = new DirectoryInfo(args[2]).GetFiles("*_1.jpg");
                    foreach (var f in files)
                    {
                        try
                        {
                            var faceregreq = new faceregreq
                        {
                            image = Convert.ToBase64String(File.ReadAllBytes(f.FullName)),
                            image_type = "BASE64",
                            group_id = args[1],
                            user_id = f.Name.Replace(".jpg", ""),
                        };
                        FaceAdd.add(to.access_token, JsonConvert.SerializeObject(faceregreq));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("exeption---{0},{1},", f.Name,  ex.Message);
                        }
                        Thread.Sleep(200);
                    }

                    return;
                }
                if (args[0] == "facereg")
                {
                    var faceregreq = new faceregreq
                    {
                        image = Convert.ToBase64String(File.ReadAllBytes(args[2])),
                        image_type = "BASE64",
                        group_id = args[1],
                        user_id = args[3],
                    };
                    FaceAdd.add(to.access_token, JsonConvert.SerializeObject(faceregreq));
                    return;
                }

                if (args[0] == "purify")
                {
                    var files2 = new DirectoryInfo(args[1]).GetFiles("*_2.jpg");
                    var stop = new Stopwatch();
                    var timeindex = 0;
                    foreach (var f in files2)
                    {
                        var f2 = Path.Combine(args[2], f.Name.Replace("_2.jpg", "_1.jpg"));
                        if (File.Exists(f2))
                        {
                            var rreq = new List<matchreq>();
                            rreq.Add(new matchreq
                            {
                                image = Convert.ToBase64String(File.ReadAllBytes(f2)),
                                face_type = "IDCARD",
                                image_type = "BASE64",
                                quality_control = "NONE",
                                liveness_control = "NONE",
                            });
                            rreq.Add(new matchreq
                            {
                                image = Convert.ToBase64String(File.ReadAllBytes(f.FullName)),
                                face_type = "LIVE",
                                image_type = "BASE64",
                                quality_control = "NONE",
                                liveness_control = "NONE",
                            });
                            if (timeindex < 1)
                            {
                                stop.Restart();
                                timeindex++;
                            }
                            else
                            {
                                if (timeindex > 1)
                                {
                                    stop.Stop();
                                    if (stop.ElapsedMilliseconds < 1100)
                                    {
                                        Console.WriteLine("sleep! {0}", 1100 - stop.ElapsedMilliseconds);
                                        Thread.Sleep(1100 - (int)stop.ElapsedMilliseconds);
                                    }
                                    else Console.WriteLine("elapsed! {0}", stop.ElapsedMilliseconds);
                                    timeindex = 1;
                                    stop.Restart();
                                }
                                else
                                {
                                    timeindex++;
                                }
                            }
                            haha: try
                            {
                               var rm = FaceMatch.match(to.access_token, JsonConvert.SerializeObject(rreq));
                                var rret = JsonConvert.DeserializeObject<matchresponse>(rm);
                                if (rret.error_code == 0)
                                {
                                    if (rret.result.score > 80)
                                    {
                                        Console.WriteLine("ok" + rret.result.score);
                                    }
                                    else
                                    {
                                        //  Console.WriteLine();
                                        Console.Error.WriteLine(f.Name + "--" + rret.result.score + "-");
                                        // File.AppendText("")
                                    }
                                }
                                else
                                {
                                    Console.WriteLine(f.Name + rret.error_code + rret.error_msg);
                                    Console.Error.WriteLine(f.Name + "--" + rret.error_code + rret.error_msg);
                                }
                            }
                            catch(Exception ex)
                            {
                                Console.WriteLine(f.Name + ex.Message);
                                Thread.Sleep(1100 );
                                goto haha;
                            }
                        }
                        else
                        {
                         //   Console.WriteLine(f.Name + rret.error_code + rret.error_msg);
                            Console.Error.WriteLine(f.Name + "--no 1.jpg" );
                        }
                    }
                    return;
                }

                var req = new List<matchreq>();
                req.Add(new matchreq
                {
                    image = Convert.ToBase64String(File.ReadAllBytes(args[0])),
                    face_type = "IDCARD",
                    image_type = "BASE64",
                    quality_control = "NONE",
                    liveness_control = "NONE",
                });
                req.Add(new matchreq
                {
                    image = Convert.ToBase64String(File.ReadAllBytes(args[1])),
                    face_type = "LIVE",
                    image_type = "BASE64",
                    quality_control = "NONE",
                    liveness_control = "NONE",
                });
                var m = FaceMatch.match(to.access_token, JsonConvert.SerializeObject(req));
                var ret = JsonConvert.DeserializeObject<matchresponse>(m);
                if (ret.error_code == 0)
                {
                    if (ret.result.score > 80)
                    {
                        Console.WriteLine("ok" + ret.result.score);
                    }
                    else
                    {
                        Console.WriteLine("not ok-" + ret.result.score);
                    }
                }
                else
                {

                    Console.WriteLine(ret.error_code + ret.error_msg);
                }
            }
            else
            {
                Console.WriteLine("getAccessToken error");
            }
        }
    }
}
