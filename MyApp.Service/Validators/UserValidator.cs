using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MyAppCore.Entities;
using MyAppService.Services;
using MyAppCore.Interfaces;


namespace MyAppService.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator(IUserService userService)  
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Kullanıcı adı boş olamaz. Lütfen geçerli kullanıcı adı giriniz.")
                .MustAsync(async (username, cancellation) =>
                {
                    var existingUser = await userService.GetByUsernameAsync(username);
                    return existingUser == null;
                }).WithMessage("Bu kullanıcı adı zaten kayıtlı.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre boş olamaz. Lütfen geçerli bir şifre giriniz.");
        }
    }
}
