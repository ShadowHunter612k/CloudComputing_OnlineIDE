using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using System.Diagnostics;

namespace DA_OnlineIDE.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            StopContainer();
            return View();
        }

        public ActionResult compileCSharp()
        {
            StopContainer();
            return View();
        }
        // goi cmd de thuc thi lenh
        private string ExecCmd(String cmd)
        {
            System.Diagnostics.ProcessStartInfo process = new System.Diagnostics.ProcessStartInfo("cmd", cmd);
            process.RedirectStandardOutput = true;
            process.UseShellExecute = false;
            process.CreateNoWindow = true;
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.StartInfo = process;
            proc.Start();
            string notif = proc.StandardOutput.ReadToEnd();
            return notif;
        }

        private void RunContainerJavaCompile()
        {
            string commandText = @"/c docker run --rm -it -d -v c:/OnlineCompilerIDE/java:/data --name onlineCompile ubuntu:18.04";
            ExecCmd(commandText);
        }
        private void RunContainerCsharp()
        {
            string commandText = @"/c docker run --rm -it -d -v c:/OnlineCompilerIDE/csharp:/data --name onlineCompile ubuntu:18.04";
            ExecCmd(commandText);
        }
        private void StopContainer()
        {
            string commandText = @"/c docker stop onlineCompile";
            ExecCmd(commandText);
        }

        [HttpPost]
        public ActionResult CompileCode(string code, string lang)
        {
            string commandText = @"";
            if (lang == "Java")
            {
                RunContainerJavaCompile();
                String filepathWrite = @"C:/OnlineCompilerIDE/java/Main.java";
                using (StreamWriter sw = new StreamWriter(filepathWrite))
                {
                    sw.WriteLine(code);
                    sw.Close();
                }
                commandText = @"/c docker exec onlineCompile bash /data/compile.sh ";
            }
            else
            {
                if (lang == "C#")
                {
                    RunContainerCsharp();
                    String filepathWrite = @"C:/OnlineCompilerIDE/csharp/main.cs";
                    using (StreamWriter sw = new StreamWriter(filepathWrite))
                    {
                        sw.WriteLine(code);
                        sw.Close();
                    }
                    commandText = @"/c docker exec onlineCompile bash /compile_csharp.sh ";
                }
                else
                {
                    //Dùng để compile thêm ngôn ngữ
                }
            }
            return Content(ExecCmd(commandText));
        }
    }
}