// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AllorsMembershipProvider.cs" company="Allors bvba">
// Copyright 2002-2016 Allors bvba.
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Web.Security
{
    using System;
    using System.Web.Security;

    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    using Microsoft.AspNet.Identity;

    public class AllorsMembershipProvider : MembershipProvider
    {
        private const string Provider = "Allors";

        public override string ApplicationName { get; set; }

        public override bool ValidateUser(string userId, string password)
        {
            var database = Config.Default;
            using (ISession session = database.CreateSession())
            {
                var user = new Users(session).FindBy(M.User.UserName, userId);
                if (user != null)
                {
                    if (user.ExistUserPasswordHash)
                    {
                        var passwordHasher = new PasswordHasher();
                        return passwordHasher.VerifyHashedPassword(user.UserPasswordHash, password) != PasswordVerificationResult.Failed;
                    }

                    return false;
                }

                return false;
            }
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            var database = Config.Default;
            using (ISession session = database.CreateSession())
            {
                var user = (User)session.Instantiate(providerUserKey.ToString());
                if (user != null)
                {
                    return this.CreateMembershipUser(user);
                }

                session.Rollback();
            }

            return null;
        }

        public override MembershipUser GetUser(string userId, bool userIsOnline)
        {
            var database = Config.Default;
            using (ISession session = database.CreateSession())
            {
                var user = new Users(session).FindBy(M.User.UserName, userId);
                if (user != null)
                {
                    return this.CreateMembershipUser(user);
                }

                session.Rollback();
            }

            return null;
        }

        private MembershipUser CreateMembershipUser(User user)
        {
            var membershipUser = new MembershipUser(
                Provider,
                user.UserName,
                user.Strategy.ObjectId,
                null,
                null,
                null,
                true,
                false,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now);
            return membershipUser;
        }

        #region Not Supported

        public override bool EnablePasswordRetrieval
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public override bool EnablePasswordReset
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public override int PasswordAttemptWindow
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public override bool RequiresUniqueEmail
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public override int MinRequiredPasswordLength
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public override string PasswordStrengthRegularExpression
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotSupportedException();
        }

        public override MembershipUser CreateUser(
            string username,
            string password,
            string email,
            string passwordQuestion,
            string passwordAnswer,
            bool isApproved,
            object providerUserKey,
            out MembershipCreateStatus status)
        {
            throw new NotSupportedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(
            string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotSupportedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotSupportedException();
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotSupportedException();
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotSupportedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotSupportedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotSupportedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotSupportedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotSupportedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotSupportedException();
        }

        public override MembershipUserCollection FindUsersByName(
            string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotSupportedException();
        }

        public override MembershipUserCollection FindUsersByEmail(
            string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotSupportedException();
        }

        #endregion Not Supported
    }
}