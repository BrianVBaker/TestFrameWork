﻿using log4net;
using System;
using System.IO;
using System.Windows.Forms;

namespace BatchRunner
{
    class BatchRunner
    {
        static void Main(string[] args)
        {
            Config.DIR = args[0];
            Config.Browser = args[1];
            Config.Delim = args[2];
            Config.Time = Convert.ToInt32(args[3]);
            Config.GUIurl = args[4];
            Config.BaseUrl = args[5];
            Config.LogFile = args[6];

            
            GlobalContext.Properties["LogName"] = Config.LogFile;
            ILog log = LogManager.GetLogger(typeof(BatchRunner));

            log.Info("Initialising........");
            var csvFiles = Directory.EnumerateFiles(Config.DIR, "*.csv", SearchOption.AllDirectories);
            int filect = 0;
            foreach (string CSV in csvFiles)
            {
                filect++;
                log.Info(filect + ". " + CSV);
            }

            foreach (string currentFile in csvFiles)
            {

                //RunTesting Testlines = new RunTesting();
                log.Info("Processing file : " + currentFile);

                string[] TestSteps;

                using (StreamReader sr = new StreamReader(currentFile))
                {
                    string TestFile = sr.ReadToEnd();
                    TestSteps = TestFile.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                }



                if (Config.Browser != "None")
                    TestRun.Driver = SeleniumActions.GetDriver(Config.Browser);

                RunTesting.Steps(TestSteps, TestRun.Driver);

                
                int passct = 0;
                int failct = 0;
                int totalct = 0;

                foreach (var item in RunTesting.Results.Keys)
                {
                    totalct++;
                    if (RunTesting.Results[item] == true)
                        passct++;
                    else
                        failct++;
                }
                log.Info("******* Stored values ********");

                foreach (var Key in RunTesting.StepValues.Keys)
                {
                    foreach (var InnerKey in RunTesting.StepValues[Key].Keys)
                    {
                        log.Info("For step " + Key + ", Key : " + InnerKey + ", Value : " + RunTesting.StepValues[Key][InnerKey]);
                    }
                }
                log.Info("******************************");

                log.Info("Testing Summary for file " + currentFile);
                log.Info("==================");

                log.Info("Total passes = " + passct);
                log.Info("Total fails  = " + failct);
                log.Info("Total Tests  = " + totalct);
                log.Info("==================");
                log.Info("Asta la vista!");
            }


        }
    }
}

