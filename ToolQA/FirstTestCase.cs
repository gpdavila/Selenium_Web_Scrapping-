using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.IO;

namespace ToolQA
{
    public enum OperationMode
    {
        Like = 0,
        Dislike,
    }

    class FirstTestCase
    {
        static void Main(string[] args)
        {
            IWebDriver driver = new ChromeDriver();
            List<String> FinalLinks = new List<String>();
            String final_link;
            IWebElement teste;
            String buffer;
            IWebElement loginButton;
            String userName;
            String passWord;
            String targetAccount;
            String URL;

            int mode = (int)OperationMode.Like;   //Operation Mode

            IReadOnlyCollection<IWebElement> userInfo;
            IReadOnlyCollection<IWebElement> links;
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            var Login_Accounts = new StreamReader(@"Login_Accounts.csv");
            var Target_Accounts = new StreamReader(@"Like_Accounts.csv");

            while (!Login_Accounts.EndOfStream)
            {
                var line = Login_Accounts.ReadLine();
                var values = line.Split(';');
                userName = values[0];
                passWord = values[1];

                while (!Target_Accounts.EndOfStream)
                {
                    line = Target_Accounts.ReadLine();
                    values = line.Split(';');
                    targetAccount = values[0];
             
                    URL = "https://www.instagram.com/" + targetAccount;

                    driver.Navigate().GoToUrl(URL);   // URL also could be a command line parameter

                    //  < button class="_0mzm- sqdOP  L3NKy       " type="button">Entrar</button>   
                    loginButton = driver.FindElement(By.XPath(".//*[@class='sqdOP  L3NKy       ']")); // Blank spaces are necessary 
                    loginButton.Click();            // Clicking on Login button
                    Thread.Sleep(5000);            // Wait page to load

                    userInfo = driver.FindElements(By.XPath(".//*[@class='_2hvTZ pexuQ zyHYP']"));
                    // TODO Cryptograph password
                    Console.WriteLine("Performing Login !");
                    userInfo.ElementAt(0).SendKeys(userName);       // User Name                                                                
                    userInfo.ElementAt(1).SendKeys(passWord);       // Password 
                    loginButton = driver.FindElement(By.XPath(".//*[@class='_0mzm- sqdOP  L3NKy       ']")); // Blank spaces are necessary 
                    loginButton.Click();

                    // Another possible solution to get User and password fields
                    #region
                    /*
                    IWebElement userName;
                    IWebElement userPassword;
                    //< input class="_2hvTZ pexuQ zyHYP" id="f6b433402d8f64" aria-label="Número de telefone, nome de usuário ou email" aria-required="true" autocapitalize="off" autocorrect="off" maxlength="75" name="username" type="text" value="">
                    // Id change every time page is access
                    userName = driver.FindElements(By.XPath(".//*[@class='_2hvTZ pexuQ zyHYP']"));
                    userName.SendKeys("gabrielpiscoya");

                    //< input class="_2hvTZ pexuQ zyHYP" id="f39d4b74f27fdc" aria-label="Senha" aria-required="true" autocapitalize="off" autocorrect="off" name="password" type="password" value="">
                    userPassword = driver.FindElement(By.XPath(".//*[@class='_2hvTZ pexuQ zyHYP']"));
                    //userPassword = driver.FindElement(By.Name("password"));
                    userPassword.SendKeys("a");
                    */
                    #endregion
                    Thread.Sleep(1000);
                    driver.Navigate().GoToUrl(URL);          // Already Logged on Account

                    /*Scroll Down to charge more pictures*/
                    #region 

                    //IJavaScriptExecutor js = driver as IJavaScriptExecutor;
                    js.ExecuteScript("window.scrollBy(0,1000);");
                    Thread.Sleep(1000);
                    js.ExecuteScript("window.scrollBy(0,2000);");
                    Thread.Sleep(1000);
                    js.ExecuteScript("window.scrollBy(0,3000);");
                    Thread.Sleep(1000);
                    js.ExecuteScript("window.scrollBy(0,4000);");
                    Thread.Sleep(1000);
                    js.ExecuteScript("window.scrollBy(0,5000);");
                    Thread.Sleep(3000);
                   
                    #endregion
                    // Getting Photo Direct Links
                    links = driver.FindElements(By.XPath(".//*[@class='v1Nh3 kIKUG  _bz0w']/a"));
                    foreach (IWebElement link in links)
                    {
                        final_link = link.GetAttribute("href");
                        Console.WriteLine(final_link);
                        FinalLinks.Add(final_link);
                    }

                    Console.WriteLine("Photo Direct Links" + links.Count());
                    foreach (String link in FinalLinks)
                    {
                        driver.Navigate().GoToUrl(link);
                        try
                        {
                            Thread.Sleep(1000); // Wait 1s to load page info
                            teste = driver.FindElement(By.XPath(".//*[@class='dCJp8 afkep coreSpriteHeartOpen _0mzm-']/span")); // Getting Heart Logo
                           //teste = driver.FindElement(By.XPath(".//*[@class='coreSpriteHeartOpen _0mzm- dCJp8']/span")); // Podese definir uma constante ?

                            buffer = teste.GetAttribute("aria-label").ToString(); // Get Heart Status

                            // Verifies whether the photo is liked or not
                            if (buffer.Equals("Me Gusta", StringComparison.CurrentCultureIgnoreCase) || buffer.Equals("Curtir", StringComparison.CurrentCultureIgnoreCase) || buffer.Equals("Like", StringComparison.CurrentCultureIgnoreCase))
                            {
                                if (mode == (int)OperationMode.Like)
                                {
                                    driver.FindElement(By.XPath(".//*[@class='dCJp8 afkep coreSpriteHeartOpen _0mzm-']")).Click();
                                    Console.WriteLine("Like!");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Picture already liked");
                                if (mode == (int)OperationMode.Dislike)
                                {
                                    driver.FindElement(By.XPath(".//*[@class='dCJp8 afkep coreSpriteHeartOpen _0mzm-']")).Click();
                                    Console.WriteLine("Dislike!");
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("An error occurred: '{0}'", e);
                        }
                    }

                }
            }
            #region
            //driver.Quit();

            //myField.SendKeys("Gabriel");
            //myField = driver.FindElement(By.Name("lastname"));
            //myField.SendKeys("Piscoya");
            //myField = driver.FindElement(By.Id("submit"));
            //myField.Submit();
            /*
            myField = driver.FindElement(By.PartialLinkText("Partial"));
            myField.Click();
            String buttom = driver.FindElement(By.TagName("button")).ToString();
            Console.WriteLine(buttom);
            myField = driver.FindElement(By.LinkText("Link Test"));
            myField.Click();
           
           IWebElement myField = driver.FindElement(By.Id("account"));
            myField.Click();
            driver.FindElement(By.Id("log")).SendKeys("gabriel");
            myField.SendKeys("Gabriel Piscoya");
            Thread.Sleep(500);
            myField.SendKeys(Keys.Enter);
            driver.Navigate().Back();
            driver.FindElement(By.)
            */
            #endregion   
        }
    }
}
