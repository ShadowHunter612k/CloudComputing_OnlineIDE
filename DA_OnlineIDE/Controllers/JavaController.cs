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
            string output = ExecCmd(commandText);
            Session["output"] = output;
            Session["language"] = "Java";
            return Content(output);
        }

        public ActionResult javaB1()
        {
            StopContainer();
            Session["language"] = "Java";
            Session["exer"] = "javaB1";
            Session["next"] = "javaB2";
            return View();
        }
        public ActionResult javaB2()
        {
            StopContainer();
            Session["language"] = "Java";
            Session["exer"] = "javaB2";
            Session["next"] = "javaB3";
            return View();
        }
        public ActionResult javaB3()
        {
            StopContainer();
            Session["language"] = "Java";
            Session["exer"] = "javaB3";
            Session["next"] = "javaB1";
            return View();
        }
        public ActionResult check()
        {
            if (Session["exer"] == "javaB1")
            {
                if (Session["output"].ToString() == "Hello World")
                {
                    Session["next"] = "javaB2";
                    return Content("Correct");
                }
            }
            if (Session["exer"] == "javaB2")
            {
                if (Convert.ToInt32(Session["output"]) == 30)
                {
                    Session["next"] = "javaB3";
                    return Content("Correct");
                }
            }
            if (Session["exer"] == "javaB3")
            {
                if (Convert.ToInt32(Session["output"]) == 60)
                {
                    Session["next"] = "javaB4";
                    return Content("Correct");
                }
            }
            return Content("Incorrect");
        }
    }
}