using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Linq;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using OpenQA.Selenium.Interactions;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing.Imaging;

namespace Selenium_Web_Scrapping
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Hello World!");

                // We can create a function and call it and pass the inputs

                string firstSeat = "21A";
                string secondSeat = "19H";

                string firstSeatNumber = new string(firstSeat.Where(x => Char.IsDigit(x)).ToArray());
                string firstSeatLetter = new string(firstSeat.Where(x => Char.IsLetter(x)).ToArray());

                string secondSeatNumber = new string(secondSeat.Where(x => Char.IsDigit(x)).ToArray());
                string secondSeatLetter = new string(secondSeat.Where(x => Char.IsLetter(x)).ToArray());

                string URL = "https://static.gordiansoftware.com/";

                IWebDriver driver = new ChromeDriver();

                IReadOnlyCollection<IWebElement> availableHalls;
                IReadOnlyCollection<IWebElement> availableSeatColumns;

                IWebElement correctHall;
                IReadOnlyCollection <IWebElement> seatSelectList;
                IWebElement seatSelectButton;
                IWebElement nextButton;
                IWebElement acceptExitRegulationsButton;


                driver.Navigate().GoToUrl(URL);
                driver.Manage().Window.Maximize();


                availableHalls = driver.FindElements(By.XPath("//*[@class='row-group gr-flex gr-justify-center gr-items-center gr-py-3px sm:gr-p-2'and ./ancestor::div[@class='row-"+ firstSeatNumber + " gr-flex gr-justify-between gr-items-center gr-px-4 sm:gr-border-l-8 sm:gr-border-r-8 gr-border-gray-300']]"));

                correctHall = availableHalls.ElementAt(getColumnGroup(firstSeatLetter));
                availableSeatColumns = correctHall.FindElements(By.XPath("./button"));
                availableSeatColumns.ElementAt(getColumnSeat(firstSeatLetter)).Click();

                acceptExitRegulationsButton = FindElementSafe(driver, By.Id("accept_exit_regulations"));

                if (acceptExitRegulationsButton != null){ acceptExitRegulationsButton.Click();}

                seatSelectList = driver.FindElements(By.Id("select-seat"));
                seatSelectButton = seatSelectList.ElementAt(0);
                seatSelectButton.Click();

                nextButton = driver.FindElement(By.Id("next-button"));
                nextButton.Click();

                // select second seat 

                availableHalls = driver.FindElements(By.XPath("//*[@class='row-group gr-flex gr-justify-center gr-items-center gr-py-3px sm:gr-p-2'and ./ancestor::div[@class='row-" + secondSeatNumber + " gr-flex gr-justify-between gr-items-center gr-px-4 sm:gr-border-l-8 sm:gr-border-r-8 gr-border-gray-300']]"));

                correctHall = availableHalls.ElementAt(getColumnGroup(secondSeatLetter));
                availableSeatColumns = correctHall.FindElements(By.XPath("./button"));
                availableSeatColumns.ElementAt(getColumnSeat(secondSeatLetter)).Click();

                acceptExitRegulationsButton = FindElementSafe(driver, By.Id("accept_exit_regulations"));
                if (acceptExitRegulationsButton != null) { acceptExitRegulationsButton.Click(); }

                seatSelectList = driver.FindElements(By.Id("select-seat"));
                seatSelectButton = seatSelectList.ElementAt(0);
                seatSelectButton.Click();
                nextButton.Click();

                Thread.Sleep(3000);

                IWebElement seatSelection = driver.FindElement(By.Id("gordian-select-seat-button-container"));
                Actions actions = new Actions(driver);
                actions.MoveToElement(seatSelection);
                actions.Perform();

                

                // take screenshot
            }
            catch (Exception ex)
            {

                throw;
            }

            //WaitFor(driver,By.Id("show-upsell"), SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable, TimeSpan.FromSeconds(25));


            // scroll down 

            // print

            //secondSeatLine = driver.FindElement(By.XPath("//*[@class='row-" + secondSeatNumber + " gr-flex gr-justify-between gr-items-center gr-px-4 sm:gr-border-l-8 sm:gr-border-r-8 gr-border-gray-300']"));

        }

        private static int getColumnGroup(string letter)
        {
            switch (letter)
            {
                case "A":
                case "B":
                case "C":
                    return 0;

                case "D":
                case "E":
                case "F":
                case "G":
                    return 1;

                case "H":
                case "J":
                case "K":
                    return 2;
                default:
                    return -1;

            }
        }

        private static int getColumnSeat(string letter)
        {
            switch (letter)
            {
                case "A":
                    return 0;
                case "B":
                    return 1;
                case "C":
                    return 2;

                case "D":
                    return 0;
                case "E":
                    return 1;
                case "F":
                    return 2;
                case "G":
                    return 3;
                case "H":
                    return 0;
                case "J":
                    return 1;
                case "K":
                    return 2;
                default:
                    return -1;

            }
        }

        private static IWebElement WaitFor(IWebDriver driver, By by, Func<By, Func<IWebDriver, IWebElement>> conditions, TimeSpan time)
        {

            try
            {
                var wait = new WebDriverWait(driver, time);
                var element = wait.Until(conditions(by));
                return element;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static IWebElement FindElementSafe(IWebDriver driver, By by)
        {
            try
            {
                return driver.FindElement(by);
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        }

        /*
        public static Bitmap GetElementScreenShot(IWebDriver driver, IWebElement element)
        {
            Screenshot sc = ((ITakesScreenshot)driver).GetScreenshot();
            var img = Image.FromStream(new MemoryStream(sc.AsByteArray)) as Bitmap;
            return img.Clone(new Rectangle(element.Location, element.Size), img.PixelFormat);
        }
        */
    }
}
