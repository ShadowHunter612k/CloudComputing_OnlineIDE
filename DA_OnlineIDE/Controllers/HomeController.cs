using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;

namespace DA_OnlineIDE.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        // goi cmd de thuc thi lenh
        private string ExecCmd(String cmd)
        {
            System.Diagnostics.Process.Start("CMD.exe", cmd);
            System.Diagnostics.ProcessStartInfo process = new System.Diagnostics.ProcessStartInfo("cmd", cmd);
            process.RedirectStandardOutput = true;
            process.UseShellExecute = false;
            process.CreateNoWindow = true;
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo = process;
            proc.Start();
            string notif = proc.StandardOutput.ReadToEnd();
            return notif;
        }

        private void RunContainer()
        {
            string commandText = @"/c docker run --rm -it -d -v c:/java:/data --name javacompile ubuntu:18.04";
            ExecCmd(commandText);
        }

        private void StopContainer()
        {
            string commandText = @"/c docker stop javacompile";
            ExecCmd(commandText);
        }

        [HttpPost]
        public ActionResult CompileJavaCode(string javaCode)
        {
            RunContainer();
            String filepathWrite = @"C:/java/Main.java";
            using (StreamWriter sw = new StreamWriter(filepathWrite))
            {
                sw.WriteLine(javaCode);
                sw.Close();
            }
            string commandText = @"/c docker exec javacompile bash /data/compile.sh ";
            return Content(ExecCmd(commandText));
        }
    }
}