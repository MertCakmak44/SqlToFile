using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MyAppCore.Entities;

namespace MyAppService.Validators
{
    public class UserValidator:AbstractValidator<User>
    {
        public UserValidator() {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Kullanıcı adı boş olamaz Lütfen geçerli kullanıcı adını yazınız");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre boş olamaz Lütfen şifrenizi yazın");
            
        }
    }
}
