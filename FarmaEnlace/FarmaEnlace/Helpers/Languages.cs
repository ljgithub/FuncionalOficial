namespace FarmaEnlace.Helpers
{
    using Xamarin.Forms;
    using FarmaEnlace.Interfaces;
    using Resources;

    public static class Languages
    {
        static Languages()
        {
            var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resource.Culture = ci;
            DependencyService.Get<ILocalize>().SetLocale(ci);
        }

        public static string Accept
        {
            get { return Resource.Accept; }
        }

        public static string EmailValidation
        {
            get { return Resource.EmailValidation; }
        }

        public static string Error
        {
            get { return Resource.Error; }
        }

        public static string ErrorConection
        {
            get { return Resource.ErrorConection; }
        }

        public static string ErrorService
        {
            get { return Resource.ErrorService; }
        }
        public static string Link
        {
            get { return Resource.Link; }
        }
        public static string Share
        {
            get { return Resource.Share; }
        }
        public static string VirtualCard
        {
            get { return Resource.VirtualCard; }
        }

        public static string ProductCatalog
        {
            get { return Resource.ProductCatalog; }
        }
        public static string CellPhone
        {
            get { return Resource.CellPhone; }
        }
        public static string Email
        {
            get { return Resource.Email; }
        }
        public static string Update
        {
            get { return Resource.Update; }
        }
        public static string FindUs
        {
            get { return Resource.FindUs; }
        }
        public static string PharmaciesOnDuty
        {
            get { return Resource.PharmaciesOnDuty; }
        }
        public static string SeeList
        {
            get { return Resource.SeeList; }
        }
        public static string ShowOnMap
        {
            get { return Resource.ShowOnMap; }
        }
        public static string Search
        {
            get { return Resource.Search; }
        }
        public static string NearbyPharmacies
        {
            get { return Resource.NearbyPharmacies; }
        }
        public static string TwentyFourHoursPharmacies
        {
            get { return Resource.TwentyFourHoursPharmacies; }
        }
        public static string PharmacyFinder
        {
            get { return Resource.PharmacyFinder; }
        }
        public static string Call
        {
            get { return Resource.Call; }
        }
        public static string ProductDetail
        {
            get { return Resource.ProductDetail; }
        }
        public static string SearchStock
        {
            get { return Resource.SearchStock; }
        }
        public static string Return
        {
            get { return Resource.Return; }
        }
        public static string ForgotYourPassword
        {
            get { return Resource.ForgotYourPassword; }
        }
        public static string SignIn
        {
            get { return Resource.SignIn; }
        }
        public static string Login
        {
            get { return Resource.Login; }
        }
        public static string RememberMe
        {
            get { return Resource.RememberMe; }
        }
        public static string EnterPassword
        {
            get { return Resource.EnterPassword; }
        }
        public static string FarmaEnlace
        {
            get { return Resource.FarmaEnlace; }
        }
        public static string ID
        {
            get { return Resource.ID; }
        }
        public static string EnterID
        {
            get { return Resource.EnterID; }
        }
        public static string Password
        {
            get { return Resource.Password; }
        }
        public static string CurrentPassword
        {
            get { return Resource.CurrentPassword; }
        }
        public static string NewPassword
        {
            get { return Resource.NewPassword; }
        }
        public static string ConfirmNewPassword
        {
            get { return Resource.ConfirmNewPassword; }
        }
        public static string IdType
        {
            get { return Resource.IdType; }
        }
        public static string ChooseOption
        {
            get { return Resource.ChooseOption; }
        }
        public static string Name
        {
            get { return Resource.Name; }
        }
        public static string EnterName
        {
            get { return Resource.EnterName; }
        }
        public static string EnterCellPhone
        {
            get { return Resource.EnterCellPhone; }
        }
        public static string GeneratedCode
        {
            get { return Resource.GeneratedCode; }
        }
        public static string Info
        {
            get { return Resource.Info; }
        }
        public static string NoInformation
        {
            get { return Resource.NoInformation; }
        }
        public static string NoProductStock
        {
            get { return Resource.NoProductStock; }
        }
        public static string ResultSearch
        {
            get { return Resource.ResultSearch; }
        }
        public static string EnterEmail
        {
            get { return Resource.EnterEmail; }
        }
        public static string PasswordValidation
        {
            get { return Resource.PasswordValidation; }
        }
        public static string PasswordChangeSuccessful
        {
            get { return Resource.PasswordChangeSuccessful; }
        }
        public static string NewPasswordConfirmNotMatch
        {
            get { return Resource.NewPasswordConfirmNotMatch; }
        }
        public static string PasswordConfirmValidation
        {
            get { return Resource.PasswordConfirmValidation; }
        }
        public static string IdentificationValidation
        {
            get { return Resource.IdentificationValidation; }
        }
        public static string RegisteredSuccessfully
        {
            get { return Resource.RegisteredSuccessfully; }
        }
        public static string IdentificationTypeValidation
        {
            get { return Resource.IdentificationTypeValidation; }
        }
        public static string EmailErrorSent
        {
            get { return Resource.EmailErrorSent; }
        }
        public static string PasswordSentEmail
        {
            get { return Resource.PasswordSentEmail; }
        }

        public static string IDValidation
        {
            get { return Resource.IDValidation; }
        }
        public static string RucValidation
        {
            get { return Resource.RucValidation; }
        }
        public static string PasaportValidation
        {
            get { return Resource.PasaportValidation; }
        }
        public static string CellPhoneValidation
        {
            get { return Resource.CellPhoneValidation; }
        }
        public static string ErrorUserNotExist
        {
            get { return Resource.ErrorUserNotExist; }
        }
        



    }
}