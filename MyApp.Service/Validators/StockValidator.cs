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
                .GreaterThanOrEqualTo(0).WithMessage("Ürün fiyatı 0'dan küçük olamaz");
            
            RuleFor(x => x.Amount)
                .GreaterThanOrEqualTo(0).WithMessage("Miktar 0'dan küçük olamaz");

        }
    }
}
