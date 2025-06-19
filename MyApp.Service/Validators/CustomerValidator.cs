using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MyAppCore.Entities;

namespace MyAppService.Validators
{
    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator() {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("İsim boş olamaz.")
                .MinimumLength(2).WithMessage("Karakter sayısı 2 den fazla olmalı");

            RuleFor(x=> x.Sname)
                .NotEmpty().WithMessage("Soyisim boş olamaz")
                .MinimumLength(2).WithMessage("Soyisim en az 2 karakter olmalı.");
        }
    }
}
