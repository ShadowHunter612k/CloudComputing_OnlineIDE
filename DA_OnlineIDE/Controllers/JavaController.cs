using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DA_OnlineIDE.Controllers
{
    public class JavaController : Controller
    {
        // GET: Java
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

        private void RunContainerJavaCompile()
        {
            string commandText = @"/c docker run --rm -it -d -v c:/OnlineCompilerIDE/java:/data --name onlineCompile ubuntu:18.04";
            ExecCmd(commandText);
        }
        private void StopContainer()
        {
            string commandText = @"/c docker stop onlineCompile";
            ExecCmd(commandText);
        }
        public ActionResult CompileCode(string code, string lang)
        {
            RunContainerJavaCompile();
            string commandText = @"";
            String filepathWrite = @"C:/OnlineCompilerIDE/java/Main.java";
            using (StreamWriter sw = new StreamWriter(filepathWrite))
            {
                sw.WriteLine(code);
                sw.Close();
            }
            commandText = @"/c docker exec onlineCompile bash /data/compile.sh ";
            return Content(ExecCmd(commandText));
        }

        public ActionResult javaB1()
        {
            StopContainer();
            return View();
        }
        public ActionResult javaB2()
        {
            StopContainer();
            return View();
        }
        public ActionResult javaB3()
        {
            StopContainer();
            return View();
        }
    }
}