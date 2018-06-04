using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TwoCaptcha.Models;

namespace TwoCaptcha
{
    public class Solve
    {
        /// <summary>
        /// How to obtain the parameters? https://2captcha.com/2captcha-api#solving_recaptchav2_new
        /// </summary>
        public async Task<Models.TwoCaptcha> ReCaptchaV2Async(string twoCaptchaKey, string googleKey, string pageUrl, int softId = 5287317)
        {
            var client = new HttpClient();

            var json = await client.GetStringAsync($"http://2captcha.com/in.php?key={twoCaptchaKey}&method=userrecaptcha&json=1&soft_id={softId}&googlekey={googleKey}&pageurl={pageUrl}&here=now");

            var twoCaptcha = Models.TwoCaptcha.FromJson(json);
            var idRequest = twoCaptcha.Request;

            if (twoCaptcha.Status == 1)
            {
                do
                {
                    // Wait time before checking if the CAPTCHA has been resolved 
                    await Task.Delay(5000);

                    json = await client.GetStringAsync($"http://2captcha.com/res.php?key={twoCaptchaKey}&action=get&id={idRequest}&json=1");
                    twoCaptcha = Models.TwoCaptcha.FromJson(json);

                    if (twoCaptcha.Request == "ERROR_CAPTCHA_UNSOLVABLE")
                    {
                        break;
                    }

                } while (twoCaptcha.Request == "CAPCHA_NOT_READY");
            }

            return twoCaptcha;
        }

        public async static Task<Models.TwoCaptcha> NormalCaptchaAsync(string key2Captcha, string imgCaptchaBase64, 
            NumericEnum numeric = NumericEnum.NotSpecified, byte minLength = 0, byte maxLength = 0, RegSenseEnum regSense = RegSenseEnum.CaptchaInNotCaseSensitive,
            PhraseEnum phrase = PhraseEnum.CaptchaContainsOneWord, CalcEnum calc = CalcEnum.NotSpecified, LanguageEnum language = LanguageEnum.NotSpecified,
            string textInstructions = null, int softId = 5287317)
        {
            var client = new HttpClient();

            var fields = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("key", key2Captcha),
                new KeyValuePair<string, string>("json", "1"),
                new KeyValuePair<string, string>("numeric", ((int)numeric).ToString()),
                new KeyValuePair<string, string>("min_len", minLength.ToString()),
                new KeyValuePair<string, string>("max_len", maxLength.ToString()),
                new KeyValuePair<string, string>("regsense", ((int)regSense).ToString()),
                new KeyValuePair<string, string>("phrase", ((int)phrase).ToString()),
                new KeyValuePair<string, string>("calc", ((int)calc).ToString()),
                new KeyValuePair<string, string>("language", ((int)calc).ToString()),
                new KeyValuePair<string, string>("method", "base64"),
                new KeyValuePair<string, string>("body", imgCaptchaBase64),
                new KeyValuePair<string, string>("soft_id", softId.ToString()),
            };

            if (textInstructions != null)
            {
                fields.Add(new KeyValuePair<string, string>("textinstructions", ""));
            }

            var form = new FormUrlEncodedContent(fields);
            var response = await client.PostAsync("http://2captcha.com/in.php", form);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var twoCaptcha = Models.TwoCaptcha.FromJson(json);
            var idRequest = twoCaptcha.Request;

            if (twoCaptcha.Status == 1)
            {
                do
                {
                    // Wait time before checking if the CAPTCHA has been resolved 
                    await Task.Delay(5000);

                    json = await client.GetStringAsync($"http://2captcha.com/res.php?key={key2Captcha}&action=get&id={idRequest}&json=1");
                    twoCaptcha = Models.TwoCaptcha.FromJson(json);

                    if (twoCaptcha.Request == "ERROR_CAPTCHA_UNSOLVABLE")
                    {
                        break;
                    }

                } while (twoCaptcha.Request == "CAPCHA_NOT_READY");
            }

            return twoCaptcha;
        }
    }
}