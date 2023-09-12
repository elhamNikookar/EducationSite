using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.Core.Convertors;
using TopLearn.Core.DTOs.UserViewModel;
using TopLearn.Core.Generator;
using TopLearn.Core.Security;
using TopLearn.Core.Services.Interfaces;
using TopLearn.DataLayer.Context;
using TopLearn.DataLayer.Entities.User;
using TopLearn.DataLayer.Entities.Wallet;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TopLearn.Core.Services.Implementations
{


    public class UserService : IUserService
    {

        #region Constructor

        private readonly TopLearnContext _context;

        public UserService(TopLearnContext context)
        {
            _context = context;
        }

        #endregion


        #region Base Method

        public bool IsExistEmail(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public bool IsExistUserName(string userName)
        {
            return _context.Users.Any(u => u.UserName == userName);
        }

        public int AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user.UserId;
        }

        public User LoginUser(LoginViewModel login)
        {
            string hashPassword = login.Password.EncodePasswordMd5();
            string email = login.Email.FixEmail();

            return _context.Users.SingleOrDefault(u => u.Email == email && u.Password == hashPassword);
        }

        public User GetUserByEmail(string email)
        {
            return _context.Users.SingleOrDefault(u => u.Email == email);
        }

        public User GetUserById(int userId)
        {
            return _context.Users.Find(userId);
        }

        public User GetUserByUserName(string userName)
        {
            return _context.Users.SingleOrDefault(u => u.UserName == userName);
        }

        public User GetUserByActiveCode(string activeCode)
        {
            return _context.Users.SingleOrDefault(u => u.ActiveCode == activeCode);
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public bool ActiveAccount(string activeCode)
        {
            var user = _context.Users.SingleOrDefault(u => u.ActiveCode == activeCode);

            if (user == null || user.IsActive)
                return false;

            user.IsActive = true;
            user.ActiveCode = NameGenerator.GeneratorUniqCode();
            _context.SaveChanges();

            return true;
        }

        public int GetUserIdbyUserName(string userName)
        {
            return _context.Users.SingleOrDefault(u => u.UserName == userName).UserId;
        }


        #endregion


        #region UserPanel

        public InformationUserViewModel GetUserInformation(string userName)
        {
            var user = GetUserByUserName(userName);

            InformationUserViewModel information = new InformationUserViewModel();
            information.UserName = user.UserName;
            information.Email = user.Email; ;
            information.RegisterDate = user.RegisterDate;
            information.Wallet = BalanceUserWallet(userName);

            return information;
        }

        public InformationUserViewModel GetUserInformation(int userId)
        {
            var user = GetUserById(userId);

            InformationUserViewModel information = new InformationUserViewModel();
            information.UserName = user.UserName;
            information.Email = user.Email; ;
            information.RegisterDate = user.RegisterDate;
            information.Wallet = BalanceUserWallet(user.UserName);

            return information;
        }


        public SideBarUserPanelViewModel GetSidebarUserPanelData(string userName)
        {
            return _context.Users.Where(u => u.UserName == userName)
                .Select(u => new SideBarUserPanelViewModel()
                {
                    UserName = u.UserName,
                    ImageName = u.UserAvatar,
                    RegisterDate = u.RegisterDate
                }).Single();
        }

        public EditProfileViewModel GetDataForEditProfileUser(string userName)
        {
            return _context.Users.Where(u => u.UserName == userName)
                .Select(u => new EditProfileViewModel()
                {
                    AvatarName = u.UserAvatar,
                    Email = u.Email,
                    UserName = u.UserName
                }).Single();
        }

        public void EditProfile(string userName, EditProfileViewModel profile)
        {
            if (profile.UserAvatar != null)
            {
                string imagePath = "";
                if (profile.UserName != "Defalut.jpg")
                {
                    imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", profile.AvatarName);
                    if (File.Exists(imagePath))
                        File.Delete(imagePath);
                }
                profile.AvatarName = NameGenerator.GeneratorUniqCode() + Path.GetExtension(profile.UserAvatar.FileName);
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", profile.AvatarName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    profile.UserAvatar.CopyTo(stream);
                }
            }

            var user = GetUserByUserName(userName);

            user.UserName = profile.UserName;
            user.Email = profile.Email;
            user.UserAvatar = profile.AvatarName;

            UpdateUser(user);
        }

        public bool CompareOldePassword(string userName, string password)
        {
            string hashOldPassword = password.EncodePasswordMd5();
            return _context.Users.Any(u => u.UserName == userName && u.Password == hashOldPassword);
        }

        public void ChangeUserPassword(string userName, string newPassword)
        {
            var user = GetUserByUserName(userName);
            user.Password = newPassword.EncodePasswordMd5();

            UpdateUser(user);
        }

        public void DeleteUser(int userId)
        {
            User deleteUser = GetUserById(userId);
            deleteUser.IsDelete = true;
            UpdateUser(deleteUser);
        }

        #endregion


        #region Wallet

        public int BalanceUserWallet(string userName)
        {
            int userId = GetUserIdbyUserName(userName);
            var get = _context.Wallets
                    .Where(w => w.UserId == userId && w.TypeId == 1 && w.IsPay)
                    .Select(w => w.Amount)
                    .ToList();

            var pay = _context.Wallets
               .Where(w => w.UserId == userId && w.TypeId == 2)
               .Select(w => w.Amount)
               .ToList();

            return (get.Sum() - pay.Sum());
        }

        public List<WalletViewModel> GetWalletUser(string userName)
        {
            int userId = GetUserIdbyUserName(userName);

            return _context.Wallets
                .Where(w => w.IsPay && w.UserId == userId)
                .Select(w => new WalletViewModel()
                {
                    Amount = w.Amount,
                    DateTime = w.CreateDate,
                    Description = w.Description,
                    Type = w.TypeId
                }).ToList();
        }

        public int ChargeWallet(string userName, int amount, string description, bool isPay = false)
        {
            Wallet wallet = new Wallet()
            {
                Amount = amount,
                CreateDate = DateTime.Now,
                Description = description,
                IsPay = isPay,
                TypeId = 1,
                UserId = GetUserIdbyUserName(userName)
            };

            return AddWallet(wallet);

        }

        public int AddWallet(Wallet wallet)
        {
            _context.Wallets.Add(wallet);
            _context.SaveChanges();

            return wallet.WalletId;
        }

        public Wallet GetWalletByWalletId(int walletId)
        {
            return _context.Wallets.Find(walletId);
        }

        public void UpdateWallet(Wallet wallet)
        {
            _context.Wallets.Update(wallet);
            _context.SaveChanges();
        }

        #endregion


        #region AdminPanel

        public UsersForAdminViewModel GetUsers(int pageId = 1, string filterEmail = "", string filterUserName = "")
        {
            IQueryable<User> result = _context.Users;

            if (!string.IsNullOrEmpty(filterEmail))
                result = result.Where(u => u.Email.Contains(filterEmail));

            if (!string.IsNullOrEmpty(filterUserName))
                result = result.Where(u => u.UserName.Contains(filterUserName));

            //Show Item In Page
            int take = 20;
            int skip = (pageId - 1) * take;

            UsersForAdminViewModel list = new UsersForAdminViewModel();
            list.CurrentPage = pageId;
            list.PageCount = result.Count() / take;
            list.Users = result.OrderBy(u => u.RegisterDate).Skip(skip).Take(take).ToList();

            return list;
        }

        public UsersForAdminViewModel GetDeleteUsers(int pageId = 1, string filterEmail = "", string filterUserName = "")
        {
            IQueryable<User> result = _context.Users.IgnoreQueryFilters().Where(u => u.IsDelete);

            if (!string.IsNullOrEmpty(filterEmail))
                result = result.Where(u => u.Email.Contains(filterEmail));

            if (!string.IsNullOrEmpty(filterUserName))
                result = result.Where(u => u.UserName.Contains(filterUserName));

            //Show Item In Page
            int take = 20;
            int skip = (pageId - 1) * take;

            UsersForAdminViewModel list = new UsersForAdminViewModel();
            list.CurrentPage = pageId;
            list.PageCount = result.Count() / take;
            list.Users = result.OrderBy(u => u.RegisterDate).Skip(skip).Take(take).ToList();

            return list;
        }

        public int CreateNewUserFromAdmin(CreateUserViewModel user)
        {
            User newUser = new User();

            newUser.Password = user.Password.EncodePasswordMd5();
            newUser.UserName = user.UserName;
            newUser.Email = user.Email;
            newUser.RegisterDate = DateTime.Now;
            newUser.ActiveCode = NameGenerator.GeneratorUniqCode();
            newUser.IsActive = true;

            #region Save Avatar

            if (user.UserAvatar != null)
            {
                string imagePath = "";
                newUser.UserAvatar = NameGenerator.GeneratorUniqCode() + Path.GetExtension(user.UserAvatar.FileName);
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", newUser.UserAvatar);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    user.UserAvatar.CopyTo(stream);
                }
            }

            #endregion

            return AddUser(newUser);
        }

        public EditUserViewModel GetUserForShowInEditMode(int userId)
        {
            return _context.Users.Where(u => u.UserId == userId)
                .Select(u => new EditUserViewModel()
                {
                    UserId = u.UserId,
                    AvatarName = u.UserAvatar,
                    UserName = u.UserName,
                    UserRoles = u.UserRoles.Select(r => r.RoleId).ToList(),
                    Email = u.Email

                }).Single();
        }

        public void EditUserFromAdmin(EditUserViewModel editUser)
        {
            User user = GetUserById(editUser.UserId);
            user.Email = editUser.Email;
            if (!string.IsNullOrEmpty(editUser.Password))
            {
                user.Password = editUser.Password.EncodePasswordMd5();
            }

            if (editUser.UserAvatar != null)
            {
                //Delete old Image
                if (editUser.AvatarName != "Defult.jpg")
                {
                    string deletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", editUser.AvatarName);
                    if (File.Exists(deletePath))
                    {
                        File.Delete(deletePath);
                    }
                }

                //Save New Image
                user.UserAvatar = NameGenerator.GeneratorUniqCode() + Path.GetExtension(editUser.UserAvatar.FileName);
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", user.UserAvatar);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    editUser.UserAvatar.CopyTo(stream);
                }
            }

            _context.Users.Update(user);
            _context.SaveChanges();
        }

        #endregion

    }
}
