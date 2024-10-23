
# Sketchfab Account Generator

This is an automated account registration tool for Sketchfab that uses Selenium and the 2Captcha service to bypass Google's invisible reCAPTCHA during the signup process. The project is written in C# and uses the `TwoCaptcha` library for solving reCAPTCHA challenges.

## Features
- **Account Generation**: Automatically generates a random username, password, and email to create new accounts on Sketchfab.
- **CAPTCHA Solving**: Uses 2Captcha's API to solve reCAPTCHA challenges (v3).
- **Selenium Automation**: Automates the process of filling out the registration form using Selenium.
- **Credential Storage**: Saves generated account credentials in a local text file (`accounts.txt`).

## Requirements

### Tools and Libraries:
- [Selenium WebDriver](https://www.selenium.dev/)
- [Google Chrome](https://www.google.com/chrome/) and the [ChromeDriver](https://sites.google.com/chromium.org/driver/) executable installed on your machine.
- [TwoCaptcha](https://2captcha.com/) for CAPTCHA solving.
- [Newtonsoft.Json](https://www.newtonsoft.com/json) for JSON handling.

### NuGet Packages:
The following NuGet packages must be installed for the project:
1. **Selenium.WebDriver**  
2. **Selenium.WebDriver.ChromeDriver**  
3. **TwoCaptcha.Captcha**  
4. **Newtonsoft.Json**

To install the required packages, you can run:

```bash
Install-Package Selenium.WebDriver
Install-Package Selenium.WebDriver.ChromeDriver
Install-Package TwoCaptcha.Captcha
Install-Package Newtonsoft.Json
```

## Usage

### 1. Configure Your 2Captcha API Key
You will need a 2Captcha account and API key. Replace the `ApiKey` in the script with your 2Captcha API key.

```csharp
private static string ApiKey = "YOUR_2CAPTCHA_API_KEY";
```

### 2. Run the Script
Once the required packages are installed and your API key is configured, simply run the C# script. It will:
- Navigate to Sketchfab's signup page.
- Generate random credentials.
- Solve the reCAPTCHA.
- Submit the form.
- Save the credentials in `accounts.txt`.

```bash
dotnet run
```

## Important Notes
- Ensure that you are using the correct version of the ChromeDriver that matches your installed version of Google Chrome.
- This tool is for educational purposes only. Be sure to comply with the terms and policies of the website you're interacting with.

## License
This project is licensed under the MIT License.
