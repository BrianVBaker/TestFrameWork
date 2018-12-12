using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C4CDataLoad
{
    class DataLoad
    {
        public static string[] Lines { get; set; }
        public static string Id { get; set; }
        public static string Pass { get; set; }
        public static string Category { get; set; }
        public static string Authority { get; set; }
        public static string Customer { get; set; }
        public static string SubCategory { get; set; }
        public static string SummaryText { get; set; }
        public static string AreaText { get; set; }
        public static string ScriptNo { get; set; }
        static void Main(string[] args)
        {
            string[] DataRows;
            using (StreamReader sr = new StreamReader(args[0]))
            {
                string DataFile = sr.ReadToEnd();
                DataRows = DataFile.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            }
            int ct = 0;
            foreach (var DataRow in DataRows)
            {
                if (ct > 0)
                  if (DataRow != null && DataRow != "")
                    using (StreamWriter sw = new StreamWriter(args[1] + "\\" + ct.ToString() + "C4CData.csv", false))
                    {
                        // write strCurrentLine to the new file
                        sw.WriteLine(CreateCSV(DataRow));
                        sw.Close();
                    }
                ct++;
            }
        }
        public static string CreateCSV(string DataRow)
        {
            Lines = new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            string[] fields = DataRow.Split('|');

            ScriptNo = fields[0];
            Id = fields[1];
            Pass = fields[2];
            Customer = fields[3];
            Authority = fields[4];
            Category = fields[5];
            SubCategory = fields[6];
            SummaryText = fields[7];
            AreaText = fields[8];

            SetLines();
                                
            string output = "";
            
            foreach (string line in Lines)
            {
               if (line != "")
                  output = output + line + Environment.NewLine;
            }

            return output;
        }
        
        public static void SetLines()
        {
            
            Lines[0] = "Test Id| Dep Test | Type | Page | URL Modifier | Load | Reload | HTML Element | ByType | By Identifier | Is Displayed | Action | Data Value | Expected Value | Pause(ms) | API User | API Pass | Expected Result";
            Lines[1] = ScriptNo + "001||GUI|C4C login| https://my324316.crm.ondemand.com|Yes|No|C4C Web logon id|Id|USERNAME_FIELD-inner|Yes|SendKeys|"+ Id +"|||||Pass";
            Lines[2] = ScriptNo + "002||GUI|C4C login||No|No|C4C Web password|Id|PASSWORD_FIELD-inner|Yes|SendKeys|"+ Pass +"|||||Pass";
            Lines[3] = ScriptNo + "003||GUI|C4C login||No|No|C4CWeb login button|Id|LOGIN_LINK|Yes|Click||||||Pass";
            Lines[4] = ScriptNo + "004||GUI|C4C login||No|No|C4CWeb login button|Id|__button0|Yes|Click|||2000|||Fail";
            Lines[5] = ScriptNo + "005||GUI|C4C landing page ||No|No| C4CWeb Server tab|Xpath|//img[contains(@src, 'ticket_QC.png')]|Yes|Click|||4000|||Pass";
            Lines[6] = "!" + ScriptNo + "006||GUI|C4C Service||No|No|C4CWeb New Ticket|Xpath|//a[contains(@title, 'New Ticket')]|Yes|Click|||2000|||Pass";
            Lines[7] = ScriptNo + "007||GUI|C4C New ticket ||No|No|C4C Web customer|Xpath|//input[@title='Select Customer']|Yes|SendKeys|"+ Customer +"|||||Pass";
            Lines[8] = ScriptNo + "008||GUI|C4C New ticket ||No|No|C4C Web customer|Xpath|//input[@title='Select Customer']|Yes|Enter|||2000|||Pass";
            if (Authority != "")
            {
                Lines[9] = ScriptNo + "009||GUI|C4C New ticket ||No|No|C4C Web Authority|Xpath|//div[contains(@id, 'dropdownlistboxbc7af237ca414ab4b49f6f1aeb7ac440') and @class='sapUiTfComboIcon']|Yes|Click||||||Pass";
                Lines[10] = ScriptNo + "010||GUI|C4C New ticket ||No|No|C4C Web Authority|Xpath|//span[contains(text(), '"+ Authority +"')]|Yes|Click||||||Pass";
            }
            else
            {
                Lines[9] = "";
                Lines[10] = "";
            }
            Lines[11] = ScriptNo + "011||GUI|C4C New ticket ||No|No|C4C Web Category|Xpath|//input[@title='Select Service Category']|Yes|SendKeys|"+ Category +"|||||Pass";
            Lines[12] = ScriptNo + "012||GUI|C4C New ticket ||No|No|C4C Web Category|Xpath|//input[@title='Select Service Category']|Yes|Enter|||2000|||Pass";
            Lines[13] = ScriptNo + "013||GUI|C4C New ticket ||No|No|C4C Web Sub-Category |Xpath|//input[contains(@id, 'objectvalueselectorrEySmU7i14sBp2YR0VHlcm')]|Yes|SendKeys|"+ SubCategory +"|||||Pass";
            Lines[14] = ScriptNo + "014||GUI|C4C New ticket ||No|No|C4C Web Sub-Category |Xpath|//input[contains(@id, 'objectvalueselectorrEySmU7i14sBp2YR0VHlcm')]|Yes|Enter|||2000|||Pass";
            Lines[15] = ScriptNo + "015||GUI|C4C New ticket ||No|No|C4C Web Short text |Xpath|//input[contains(@id,'inputfieldZGToPcpuQ4g_FZzF6qOSfm')]|Yes|SendKeys| "+ SummaryText +" |||||Pass";
            Lines[16] = ScriptNo + "016||GUI|C4C New ticket ||No|No|C4C Web Long Text |Xpath|//textarea[contains(@id,'area')]|Yes|SendKeys| "+ AreaText +" |||||Pass";
            Lines[17] = ScriptNo + "017||GUI|C4C New ticket ||No|No|C4C Web Save ticket|Xpath|//button[text()='Save and Open']|Yes|Click||||||Pass";
            Lines[18] = ScriptNo + "019||GUI|C4C New ticket ||No|No|C4C Web inpector close|Xpath|//button[text()='Save']|Yes|Click|||2000|||Pass";
            Lines[19] = ScriptNo + "018||GUI|C4C New ticket ||No|No|C4C Web get C4C ticket|Xpath|//span[contains(@class,'sapUiUx3TVTitleFirst') and not(contains(@title,'" + Id.Substring(3) + "'))]|Yes|GetText||||||Pass";
            Lines[20] = ScriptNo + "020||GUI|C4C New ticket ||No|No|C4C Web logout|Id|main-logout|Yes|Click||||||Pass";
            Lines[21] = ScriptNo + "021||GUI|C4C New ticket ||No|No|C4C Web logout|Xpath|//button(contains(@id, 'main-logout')]|No|Enter||||||Pass";
            Lines[22] = "Close|||||||||||Close||||||";
        }
    }

}

    
    

