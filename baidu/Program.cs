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
            if (args[0] == "purify")// purify sourcefile sourcepath index 2>>err.log
            {
                var to1 = AccessToken.getAccessToken(int.Parse(args[3]));
                if (!to1.ok) {
                    Console.WriteLine("get token error");
                    return; }
                  //  var files2 = new DirectoryInfo(args[1]).GetFiles("*_2.jpg");
                var ffiles = File.ReadAllLines(args[1]);
                foreach (var f in ffiles)
                   // foreach (var f in files2)
                    {
                    var f2 = Path.Combine(args[2], f+ "_1.jpg");
                    var f3 = Path.Combine(args[2], f + "_2.jpg");
                    if (File.Exists(f2)&& File.Exists(f3))
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
                            image = Convert.ToBase64String(File.ReadAllBytes(f3)),
                            face_type = "LIVE",
                            image_type = "BASE64",
                            quality_control = "NONE",
                            liveness_control = "NONE",
                        });
                       
                        haha: try
                        {
                            var rm = FaceMatch.match(to1.access_token, JsonConvert.SerializeObject(rreq));
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
                                    Console.Error.WriteLine(f + "--" + rret.result.score + "-");
                                    // File.AppendText("")
                                }
                            }
                            else
                            {
                                Console.WriteLine(f + rret.error_code + rret.error_msg);
                                Console.Error.WriteLine(f + "--" + rret.error_code + rret.error_msg);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(f + ex.Message);
                            Thread.Sleep(1100);
                            goto haha;
                        }
                    }
                    else
                    {
                        //   Console.WriteLine(f.Name + rret.error_code + rret.error_msg);
                        Console.Error.WriteLine(f + "--no 1.jpg or 2.jpg");
                    }
                }
                return;
            }
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
                    var allsear = 0;
                    var oksear = 0;
                    var exp = 0;
                    foreach (var ond in new DirectoryInfo(args[2]).GetDirectories())
                    {
                        var files = new DirectoryInfo(ond.FullName).GetFiles("*_2.jpg");
                        
                        foreach (var f in files)
                        {
                            allsear++;
                            onsafe: try
                            {
                                var oneofnreq = new oneofnreq
                                {
                                    image = Convert.ToBase64String(File.ReadAllBytes(f.FullName)),
                                    image_type = "BASE64",
                                    group_id_list = args[1],
                                };
                                var retsear = FaceSearch.search(to.access_token, JsonConvert.SerializeObject(oneofnreq));
                                var onres = JsonConvert.DeserializeObject<faceregres>(retsear);
                                if (onres.error_code != 0)
                                {
                                    Console.WriteLine(f.FullName + "onbatch error" + retsear);
                                    Thread.Sleep(500);
                                    goto onsafe;
                                }
                                if (retsear.Contains(f.Name.Replace("_2.jpg", "_1")))
                                {
                                    oksear++;
                                    Console.WriteLine("true---{0},{1},{2}", oksear, allsear, f.Name);
                                }
                                else
                                {
                                    Console.WriteLine("false---{0},{1},{2}", oksear, allsear, f.Name);
                                    Console.Error.WriteLine("{0}--not one-,return={1}", f.Name, retsear);
                                }
                            }
                            catch (Exception ex)
                            {
                                exp++;
                                Console.WriteLine("exeption---{0},{1},{2}", f.Name, exp, ex.Message);
                                Thread.Sleep(500);
                                goto onsafe;
                            }
                            //   Thread.Sleep(500);
                        }
                    }
                    Console.WriteLine("result---all:{0},ok:{1},exp:{2}", allsear,oksear, exp);
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
                    foreach (var d in new DirectoryInfo(args[2]).GetDirectories())
                    {
                        var files = new DirectoryInfo(d.FullName).GetFiles("*_1.jpg");
                        foreach (var f in files)
                        {
                            safeadd: try
                            {
                                var faceregreq = new faceregreq
                                {
                                    image = Convert.ToBase64String(File.ReadAllBytes(f.FullName)),
                                    image_type = "BASE64",
                                    group_id = args[1],
                                    user_id = f.Name.Replace(".jpg", ""),
                                };
                                var fret = FaceAdd.add(to.access_token, JsonConvert.SerializeObject(faceregreq));
                                var res = JsonConvert.DeserializeObject<faceregres>(fret);
                                if (res.error_code != 0)
                                {
                                    if (res.error_code == 223105)
                                    {
                                        Console.Error.WriteLine(f.Name + "--exist error" + fret);
                                        continue;
                                    }
                                    Console.WriteLine(f.Name + "reg error" + fret);
                                    Thread.Sleep(100);
                                    goto safeadd;
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("{0}--exeption-{1},", f.Name, ex.Message);
                                Thread.Sleep(100);
                                goto safeadd;
                            }
                            // Thread.Sleep(200);
                        }
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
                //if (args[0] == "compbatch")
                //{
                //    foreach(var f in File.ReadAllLines(args[1]))
                //    {
                //        var reg=
                //        var creq = new List<matchreq>();
                //        creq.Add(new matchreq
                //        {
                //            image = Convert.ToBase64String(File.ReadAllBytes(args[0])),
                //            face_type = "IDCARD",
                //            image_type = "BASE64",
                //            quality_control = "NONE",
                //            liveness_control = "NONE",
                //        });
                //        creq.Add(new matchreq
                //        {
                //            image = Convert.ToBase64String(File.ReadAllBytes(args[1])),
                //            face_type = "LIVE",
                //            image_type = "BASE64",
                //            quality_control = "NONE",
                //            liveness_control = "NONE",
                //        });
                //        var cm = FaceMatch.match(to.access_token, JsonConvert.SerializeObject(creq));
                //        var cret = JsonConvert.DeserializeObject<matchresponse>(cm);
                //        if (cret.error_code == 0)
                //        {
                //            if (cret.result.score > 80)
                //            {
                //                Console.WriteLine("ok" + cret.result.score);
                //            }
                //            else
                //            {
                //                Console.WriteLine("not ok-" + cret.result.score);
                //            }
                //        }
                //        else
                //        {
                //            Console.WriteLine(cret.error_code +cret.error_msg);
                //        }
                //    }
                //    return;
                //}
               

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
