using log4net;
using log4net.Config;
using OpenQA.Selenium;
using System;
using System.IO;
using TestFramework.TestFramework;

namespace TestFrameWork.TestFramework
{
    class GUIRunnerCode
    {
                
        static void Main(string[] args)
        {
            
            ILog log = LogManager.GetLogger(typeof(GUIRunnerCode));

            log.Info("Initialising........");

            Config.CSVfile = args[0];
            Config.Browser = args[1];
            Config.Delim = args[2];
            Config.Time = Convert.ToInt32(args[3]);
            Config.GUIurl = args[4];
            Config.BaseUrl = args[5];
            Config.LogFile = args[6];              
                        
            //RunTesting Testlines = new RunTesting();
            
            string[] TestSteps;

            using (StreamReader sr = new StreamReader(args[0]))
            {
                string TestFile = sr.ReadToEnd();
                TestSteps = TestFile.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            }
                      

           
      
           
            if (Config.Browser != "None")
               TestRun.Driver = SeleniumActions.GetDriver(Config.Browser);

            RunTesting.Steps(TestSteps, TestRun.Driver);
                                        
       
         
       
    
        }

    }
}

