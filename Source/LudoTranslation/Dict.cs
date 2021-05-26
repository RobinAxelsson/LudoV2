

using System.Reflection;

namespace LudoTranslation {
    public record Dict
    {
        #region Email
        public string Email_Subtitle { get; private set; }
        public string Email_Title { get; private set; }
        public string Email_Subject { get; private set; }
        #endregion

        #region LoginPage
        public string Login_BodyTitle { get; private set; }
        public string Login_AccountName { get; private set; }
        public string Login_Password { get; private set; }
        public string Login_LoginButton { get; private set; }
        public string Login_PageHeaderTitle { get; private set; }
        
        public string Login_IncorrectCredentialsLabel { get; private set; }

        #endregion
     
        #region RegisterPage
        public string Register_BodyTitle { get; private set; }
        public string Register_AccountName { get; private set; }
        public string Register_Password { get; private set; }
        public string Register_RetypePassword { get; private set; }
        public string Register_RegisterButton { get; private set; }
        public string Register_PageHeaderTitle { get; private set; }
        public string Register_Email { get; private set; }
        public string Register_EmailPlaceholderText { get; private set; }
        public string Register_PasswordPlaceholderText { get; private set; }
        public string Register_RetypePasswordPlaceholderText { get; private set; }
        public string Register_AccountNamePlaceholderText { get; private set; }
        public string Register_ChooseLanguage { get; private set; }
        public string Register_EnterValidEmailLabel { get; private set; } //Referenced in JS
        public string Register_SelectLanguageLabel { get; private set; } //Referenced in JS
        public string Register_PasswordStrengthLengthLabel { get; private set; } //Referenced in JS
        public string Register_PasswordStrengthUppercaseLabel { get; private set; } //Referenced in JS
        public string Register_PasswordMatchLabel { get; private set; } //Referenced in JS
        public string Register_PasswordMatchOkLabel { get; private set; } //Referenced in JS
        public string Register_AccountNameLengthLabel { get; private set; } //Referenced in JS
        public string Register_AccountNameSpecialsLabel { get; private set; } //Referenced in JS
        
        #endregion
        public string GetPropertyValue(string input)
        {
            return GetType().GetProperty(input)?.GetValue(this, null)?.ToString();
        }
    }
}