using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TwoCaptcha
{
    public class Solve
    {
        /// <summary>
        /// How to obtain the parameters? https://2captcha.com/2captcha-api#solving_recaptchav2_new
        /// </summary>
        public async Task<Models.TwoCaptcha> ReCaptchaV2Async(string twoCaptchaKey, string googleKey, string pageUrl)
        {
            var client = new HttpClient();
            var json = await client.GetStringAsync($"http://2captcha.com/in.php?key={twoCaptchaKey}&method=userrecaptcha&json=1&googlekey={googleKey}&pageurl={pageUrl}&here=now");
            var twocaptcha = Models.TwoCaptcha.FromJson(json);
            var idRequest = twocaptcha.Request;

            if (twocaptcha.Status == 1)
            {
                do
                {
                    await Task.Delay(5000);

                    json = await client.GetStringAsync($"http://2captcha.com/res.php?key={twoCaptchaKey}&action=get&id={idRequest}&json=1");
                    twocaptcha = Models.TwoCaptcha.FromJson(json);

                } while (twocaptcha.Request == "CAPCHA_NOT_READY");
            }

            return twocaptcha;
        }
    }
}
