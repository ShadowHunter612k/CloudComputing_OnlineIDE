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
            string output = ExecCmd(commandText);
            Session["output"] = output;
            Session["language"] = "CSharp";
            return Content(output);
        }
        public ActionResult csharpB1()
        {
            StopContainer();
            Session["language"] = "CSharp";
            Session["exer"] = "csharpB1";
            Session["next"] = "csharpB2";
            return View();
        }
        public ActionResult csharpB2()
        {
            StopContainer();
            Session["language"] = "CSharp";
            Session["exer"] = "csharpB2";
            Session["next"] = "csharpB3";
            return View();
        }
        public ActionResult csharpB3()
        {
            StopContainer();
            Session["language"] = "CSharp";
            Session["exer"] = "csharpB3";
            Session["next"] = "csharpB1";
            return View();
        }
        public ActionResult check()
        {
            if (Session["exer"] == "csharpB1")
            {
                if (Session["output"].ToString() == "Hello World")
                {
                    Session["next"] = "csharpB2";
                    return Content("Correct");
                }
            }
            if (Session["exer"] == "csharpB2")
            {
                if (Convert.ToInt32(Session["output"]) == 30)
                {
                    Session["next"] = "csharpB3";
                    return Content("Correct");
                }
            }
            if (Session["exer"] == "csharpB3")
            {
                if(Convert.ToInt32(Session["output"]) == 60)
                {
                    Session["next"] = "csharpB4";
                    return Content("Correct");
                }
            }
            return Content("Incorrect");
        }
    }
}