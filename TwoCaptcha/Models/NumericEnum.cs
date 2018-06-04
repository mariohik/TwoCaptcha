namespace TwoCaptcha.Models
{
    public enum NumericEnum
    {
        NotSpecified = 0,
        CaptchaContainsOnlyNumbers = 1,
        CaptchaContainsOnlyLetters = 2,
        CaptchaContainsOnlyNumbersOrOnlyLetters = 3,
        CaptchaContainsBothNumbersAndLetters = 4
    }
}
