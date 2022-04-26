using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VideoTranslate.Shared.Abstractions.Validators;
using VideoTranslate.Shared.DTO;

namespace VideoTranslate.Service.Validators
{
    public class PasswordValidator : IPasswordValidator
    {
        public bool ValidatePassword(User user, string password, bool isPasswordHash = false)
        {
            string hashPassword = Encoding.UTF8.GetString(Convert.FromBase64String(user.Password));

            // TODO: need to split this into 2 classes: PasswordHashValidator and PasswordBCryptValidator
            // so we will be able to remove boolean variable isPasswordHash
            if (isPasswordHash)
            {
                return string.Equals(password, hashPassword);
            }
            else if (this.IsBCrypt(hashPassword))
            {
                return this.ValidateBCrypt(password, hashPassword);
            }

            return false;
        }

        private bool IsBCrypt(string hash)
        {
            var regex = new Regex(@"^\$[1256][abxy]\$[0-9][0-9]\$.{53}");

            if (!string.IsNullOrEmpty(hash))
            {
                return regex.IsMatch(hash);
            }

            return false;
        }

        private bool ValidateBCrypt(string phrase, string hash)
        {
            // default Bcrypt.Net-Next hash validator
            return BCrypt.Net.BCrypt.Verify(phrase, hash);
        }
    }
}
