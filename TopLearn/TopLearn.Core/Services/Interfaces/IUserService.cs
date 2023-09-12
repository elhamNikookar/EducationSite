using System;
using System.Collections.Generic;
using System.Text;
using TopLearn.Core.DTOs.UserViewModel;
using TopLearn.DataLayer.Entities.User;
using TopLearn.DataLayer.Entities.Wallet;

namespace TopLearn.Core.Services.Interfaces
{
    public interface IUserService
    {

        #region Base Method

        bool IsExistUserName(string userName);
        bool IsExistEmail(string email);
        int AddUser(User user);
        User LoginUser(LoginViewModel login);
        User GetUserByEmail(string email);
        User GetUserById(int userId);
        User GetUserByUserName(string userName);
        User GetUserByActiveCode(string activeCode);
        void UpdateUser(User user);
        bool ActiveAccount(string activeCode);
        int GetUserIdbyUserName(string userName);
        void DeleteUser(int userId);

        #endregion


        #region UserPanel

        InformationUserViewModel GetUserInformation(string userName);
        InformationUserViewModel GetUserInformation(int userId);
        SideBarUserPanelViewModel GetSidebarUserPanelData(string userName);
        EditProfileViewModel GetDataForEditProfileUser(string userName);
        void EditProfile(string userName, EditProfileViewModel profile);
        bool CompareOldePassword(string userName, string password);
        void ChangeUserPassword(string userName, string newPassword);

        #endregion


        #region Wallet

        int BalanceUserWallet(string userName);
        public List<WalletViewModel> GetWalletUser(string userName);
        int ChargeWallet(string userName, int amount, string description, bool isPay = false);
        int AddWallet(Wallet wallet);
        Wallet GetWalletByWalletId(int walletId);
        void UpdateWallet(Wallet wallet);
        #endregion


        #region AdminPanel

        UsersForAdminViewModel GetUsers(int pageId = 1, string filterEmail = "", string filterUserName = "");
        UsersForAdminViewModel GetDeleteUsers(int pageId = 1, string filterEmail = "", string filterUserName = "");

        int CreateNewUserFromAdmin(CreateUserViewModel user);
        EditUserViewModel GetUserForShowInEditMode(int userId);
        void EditUserFromAdmin(EditUserViewModel editUser);


        #endregion
    }
}
