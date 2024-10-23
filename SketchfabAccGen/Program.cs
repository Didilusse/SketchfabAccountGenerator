using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support;
using TwoCaptcha.Captcha;
using System.Linq;

namespace TwoCaptcha
{
    class SketchfabAccGen
    {

        private static string filePath = "accounts.txt";
        private static Random random = new Random();
        private static string ApiKey = "KEY"; // Replace with your 2Captcha API key

        static void Main(string[] args)
        {
            // Step 1: Generate random credentials
            string username = GenerateRandomString(8);
            string password = GenerateRandomString(10);
            string email = username + "-sketchfab@eastarcti.ca";

            // Step 2: Register the account on Sketchfab
            Register(username, password, email);
        }
        
        // Generates a random alphanumeric string
        static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] stringChars = new char[length];

            for (int i = 0; i < length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new string(stringChars);
        }

        // Saves the generated account to a file
        static void SaveAccount(string username, string password)
        {
            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                sw.WriteLine($"Username: {username}, Password: {password}, Email: {username}-domain.com");
            }

            Console.WriteLine("Account credentials saved to file.");
        }
        
       
        // Uses Selenium to register the account on Sketchfab
        static void Register(string username, string password, string email)
        {
            IWebDriver driver = new ChromeDriver();
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            string captchaID = "";
            string captchaToken = "";
            TwoCaptcha solver = new TwoCaptcha(ApiKey);
            ReCaptcha captcha = new ReCaptcha();
            captcha.SetSiteKey("6LdBKGIpAAAAAKQ4ZpF-I7Sxprlid6DquLVm7sl0");
            captcha.SetUrl("https://sketchfab.com/signup");
            captcha.SetDomain("recaptcha.net");
            captcha.SetVersion("v3");
            captcha.SetAction("signup");
            captcha.SetEnterprise(false);
            captcha.SetScore(0.5);
            try
            {
                driver.Navigate().GoToUrl("https://sketchfab.com/signup");
                var _cookies = driver.Manage().Cookies.AllCookies;
                string csrftoken = "";
                foreach(Cookie cookie in _cookies)
                {
                    if (cookie.Name == "sb_csrftoken")
                    {
                        csrftoken = cookie.Value;
                    }
                }
                // IWebElement emailField = driver.FindElement(By.Name("email"));
                // IWebElement usernameField = driver.FindElement(By.Name("username"));
                // IWebElement passwordField = driver.FindElement(By.Name("password"));
                // IWebElement termsCheckbox = driver.FindElement(By.ClassName("c-checkbox__actor"));
                // IWebElement submitButton = driver.FindElement(By.CssSelector("[data-selenium='submit-button']"));
                //
                // emailField.SendKeys(email);
                // usernameField.SendKeys(username);
                // passwordField.SendKeys(password);
                // termsCheckbox.Click();
                try
                {
                    solver.Solve(captcha).Wait();
                    
                    //Console.WriteLine("Captcha solved: " + captcha.Code);
                    captchaToken = captcha.Id;
                    captchaToken = captcha.Code;
                    
                }
                catch (AggregateException e)
                {
                    Console.WriteLine("Error occurred: " + e.InnerExceptions.First().Message);
                }
                
                string res  = (string)js.ExecuteAsyncScript(@"fetch(""https://sketchfab.com/i/users"", {
  ""headers"": {
    ""accept"": ""application/json, text/plain, */*"",
    ""accept-language"": ""en-US,en;q=0.9"",
    ""content-type"": ""application/json;charset=UTF-8"",
    ""x-csrftoken"": """ + csrftoken + @""",
    ""x-requested-with"": ""XMLHttpRequest""
  },
  ""referrer"": ""https://sketchfab.com/?logged_out=1"",
  ""referrerPolicy"": ""strict-origin-when-cross-origin"",
  ""body"": ""{\""username\"":\""" + username + @"\"",\""email\"":\""" + email + @"\"",\""password\"":\""" + password + @"\"",\""tos_version\"":5,\""newsletter_consent\"":false,\""recaptcha\"":\""" + captchaToken +@"\""}"",
            ""method"": ""POST"",
            ""mode"": ""cors"",
            ""credentials"": ""include""
        }).then(req => req.text()).then(arguments[0])");
                Console.WriteLine("res: " + res);
                if (res.Contains("reCAPTCHA challenge failed"))
                {
                    solver.Report(captcha.Id, false).Wait();
                    Console.WriteLine("Captcha challenge failed.");
                }
                else
                {
                    solver.Report(captcha.Id, true).Wait();
                    Console.WriteLine("Captcha didn't fail :)");
                }
                
                // Submit the form again if necessary
                //submitButton.Click();
                Console.WriteLine("Attempted to register on Sketchfab.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error occurred: {e.Message}");
            }
            finally
            {
                driver.Quit(); // Close the browser
                SaveAccount(username, password);
            }
        }
    }
}
