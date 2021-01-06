using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DA_OnlineIDE.Controllers
{
    public class CSharpController : Controller
    {
        // GET: CSharp
        public ActionResult Index()
        {
            StopContainer();
            return View();
        }
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
        public ActionResult CompileCode(string code, string lang)
        {
            RunContainerCsharp();
            string commandText = @"";
            String filepathWrite = @"C:/OnlineCompilerIDE/csharp/main.cs";
            using (StreamWriter sw = new StreamWriter(filepathWrite))
            {
                sw.WriteLine(code);
                sw.Close();
            }
            commandText = @"/c docker exec onlineCompile bash /compile_csharp.sh ";
            return Content(ExecCmd(commandText));
        }
        public ActionResult csharpB1()
        {
            StopContainer();
            return View();
        }
        public ActionResult csharpB2()
        {
            StopContainer();
            return View();
        }
        public ActionResult csharpB3()
        {
            StopContainer();
            return View();
        }
    }
}