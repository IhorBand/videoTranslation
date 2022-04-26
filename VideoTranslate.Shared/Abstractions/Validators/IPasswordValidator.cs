using VideoTranslate.Shared.DTO;

namespace VideoTranslate.Shared.Abstractions.Validators
{
    public interface IPasswordValidator
    {
        // TODO: need to split this into 2 classes: PasswordHashValidator and PasswordBCryptValidator
        // so we will be able to remove boolean variable isPasswordHash
        bool ValidatePassword(User user, string password, bool isPasswordHash = false);
    }
}
