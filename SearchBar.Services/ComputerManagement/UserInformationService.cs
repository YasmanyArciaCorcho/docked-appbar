using System;

namespace Services.ComputerManagement
{
    public class UserInformationService : IActiveUserInformation
    {
        public string UsersDisplayName()
        {
            return GetUserName();
        }

        private static string GetUserName()
        {
            try
            {
                var userInformation = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                userInformation = userInformation.Contains("\\")
                    ? userInformation.Substring(userInformation.IndexOf("\\", StringComparison.Ordinal) + 1)
                    : userInformation;
                if (!string.IsNullOrEmpty(userInformation)) 
                    return userInformation;
                return System.DirectoryServices.AccountManagement.UserPrincipal.Current.DisplayName;
            }
            catch
            {
            }
            return string.Empty;
        }
    }
}