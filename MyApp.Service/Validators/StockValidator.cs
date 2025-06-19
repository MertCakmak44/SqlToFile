using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MyAppCore.Entities;

namespace MyAppService.Validators
{
    public class StockValidator:AbstractValidator<Stock>
    {
        public StockValidator() {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ürün ismi boş olamaz");

            RuleFor(x => x.Price)
                .LessThan(0).WithMessage("Sıfırdan düşük değer olamaz");
        }
    }
}
