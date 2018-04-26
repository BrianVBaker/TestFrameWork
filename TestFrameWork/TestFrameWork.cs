using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Firefox;
using System.Threading;
using OpenQA.Selenium.IE;
using log4net;
using OpenQA.Selenium.Interactions;
using System.Collections.Specialized;
using System.Net;

namespace GUITests.PageObjects
{
    [TestClass]
    public class GUITesting
    {
        GUIPages Pages = new GUIPages();
        IWebDriver driver;
        string oldtest = null;


        [TestMethod]
        public bool GUITests(string test, string browser, string Page, string URL, string load, string reload, string Elem, By str, string disp, string act, string val, string Exp, int pause, int waitParm)
        {
            bool Rtn = true;
            ILog log = LogManager.GetLogger(typeof(GUITesting));

            if (driver == null && browser != "")
            {
                log.Info("*** Choosing Browser : " + browser + " ***");
                switch (browser)
                {
                    case "Firefox":
                        driver = new FirefoxDriver();
                        break;
                    case "Chrome":
                        driver = new ChromeDriver();
                        break;
                    case "IE":
                        driver = new InternetExplorerDriver();
                        break;

                }
            }

            driver.Manage().Window.Maximize();


            if (test != "Close")
            {
                if (test != oldtest)
                {
                    if (oldtest != null)
                    {
                        log.Info("**** Test : " + oldtest + " completed **** ");
                        log.Info("=========================================================");
                    }
                    log.Info(" ***** Running test " + test + " : GUI Page: " + Page + " *****");
                    oldtest = test;
                }
            }
            else
            {
                if (driver != null) driver.Quit();
                log.Info("**** Test Set completed ****");
                return Rtn;
            }

            if (act == "HttpPOST")
            {
                string[] p = val.Split('^');
                NameValueCollection pairs = new NameValueCollection();
                for (int i = 0; i < p.Length; i++)
                {
                    pairs.Add(string.Format(p[i]), string.Format(p[i + 1]));
                    i++;
                }

                var response = Http.Post(URL, new NameValueCollection() { pairs });

                log.Info("Http Post response : " + response);

            }

            WebDriverWait timer = new WebDriverWait(driver, new TimeSpan(0, 0, waitParm));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            js.ExecuteScript("document.body.style.zoom='100%';");


            if (load == "Yes")
                driver.Navigate().GoToUrl(URL);
            if (reload == "Yes")
                driver.Navigate().GoToUrl(driver.Url);

            if (disp == "Yes")
            {
                if (!Pages.RunTest(str, timer, Elem))
                {
                    log.Error("Error finding element " + Elem);
                    return false;
                }
            }


            if (act == "None")
                return Rtn;
            else
                if (val != "")
                log.Info("Element " + Elem + " action = " + act + " (Sendkeys value = " + val + " )");
            else
                log.Info("Element " + Elem + " action = " + act);

            if (act == "SendKeys")
            {
                driver.FindElement(str).SendKeys(val);
                return Rtn;
            }

            if (act == "Click")
            {
                if (Pages.ObjectIsClickable(str, timer))
                {
                    driver.FindElement(str).Click();

                }
                else
                {
                    log.Error("Error clicking " + Elem);
                    return false;
                }
            }


            if (act == "Submit")
                driver.FindElement(str).Submit();

            if (act == "Compare Text")
            {
                var eText = driver.FindElement(str).Text;
                if (eText != Exp)
                {
                    log.Warn("Element Text : " + eText + " : Not equal to expected : " + Exp);

                }
                else
                    log.Info("Text content :'" + eText + "' was expected");

            }

            if (act == "Hover")
            {
                Actions builder = new Actions(driver);
                IWebElement element = driver.FindElement(str);
                builder.MoveToElement(element).Perform();

            }

            if (act == "JavaClick")
            {
                if (!Pages.ObjectIsClickable(str, timer))
                {
                    log.Error("Element is not clickable : " + str);
                    return false;
                }

                IWebElement btn = driver.FindElement(str);
                js.ExecuteScript("arguments[0].click(); ", btn);
            }


            if (pause > 0)
                Thread.Sleep(pause);

            return Rtn;

        }

    }
    public static class Http
    {
        public static byte[] Post(string uri, NameValueCollection pairs)
        {
            byte[] response = null;
            using (WebClient client = new WebClient())
            {
                response = client.UploadValues(uri, pairs);
            }
            return response;
        }
    }

    public class GUIPages
    {
        static ILog log = LogManager.GetLogger(typeof(GUIPages));

        public bool RunTest(By str, WebDriverWait wait, string elem)
        {
            bool Rtn = false;

            if (!ObjectDisplayedOK(str, wait))
            {
                log.Error("Error displaying Element " + elem);
                Rtn = false;
            }
            else
            {
                log.Info("Element " + elem + " displayed ok");
                Rtn = true;
            }


            return Rtn;
        }



        public bool ObjectDisplayedOK(By Str, WebDriverWait wait)
        {
            bool Rtn = false;
            try
            {

                var elem = wait.Until(ExpectedConditions.ElementIsVisible(Str));

                Rtn = elem.Displayed;
                if (!Rtn)
                    log.Error("Timeout for object : " + Str + "  : Timer value = " + wait.Timeout);

            }

            catch (InvalidSelectorException x)
            {
                log.Debug("Selector exception:   " + x);
            }
            catch (NoSuchElementException Ne)
            {
                log.Debug("Web exception:   " + Ne);
            }

            catch (Exception Exp)
            {
                log.Debug(Exp);
            }

            return Rtn;

        }

        public bool ObjectIsClickable(By Str, WebDriverWait wait)
        {
            bool Rtn = false;
            try
            {

                var elem = wait.Until(ExpectedConditions.ElementToBeClickable(Str));
                Rtn = true;
            }



            catch (Exception Exp)
            {
                log.Debug(Exp);
                Rtn = false;
            }

            return Rtn;

        }

    }

}

