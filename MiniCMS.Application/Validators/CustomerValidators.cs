using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MiniCMS.Application.DTOs;

namespace MiniCMS.Application.Validators
{
    public class CreateCustomerDtoValidator : AbstractValidator<CreateCustomerDto>
    {
        public CreateCustomerDtoValidator()
        {
            RuleFor(x => x.CustomerCode)
                .NotEmpty().WithMessage("Mã khách hàng không được để trống")
                .MaximumLength(20).WithMessage("Mã khách hàng không quá 20 ký tự.");

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Họ và tên không được để trống.")
                .MaximumLength(100).WithMessage("Họ và tên không quá 100 ký tự.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email không được để trống.")
                .EmailAddress().WithMessage("Địa chỉ email không đúng định dạng.")
                .MaximumLength(100).WithMessage("Email không quá 100 ký tự.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Số điện thoại không được để trống.")
                .Matches(@"^0\d{9}$").WithMessage("Số điện thoại Việt Nam phải có 10 chữ số và bắt đầu bằng số 0.");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Ngày sinh không được để trống.")
                .Must(BeAtLeast15YearsOld).WithMessage("Khách hàng cá nhân phải từ đủ 15 tuổi trở lên.");
        }

        private bool BeAtLeast15YearsOld(DateOnly dob)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var age = today.Year - dob.Year;

            // Kiểm tra qua sinh nhật chưa
            if (dob > today.AddYears(-age))
            {
                age--;
            }
            return age >= 15;
        }
    }

    public class UpdateCustomerDtoValidator : AbstractValidator<UpdateCustomerDto>
    {
        public UpdateCustomerDtoValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Họ và tên không được để trống.")
                .MaximumLength(100).WithMessage("Họ và tên không quá 100 ký tự.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email không được để trống.")
                .EmailAddress().WithMessage("Địa chỉ email không đúng định dạng.")
                .MaximumLength(100).WithMessage("Email không quá 100 ký tự.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Số điện thoại không được để trống.")
                .Matches(@"^0\d{9}$").WithMessage("Số điện thoại Việt Nam phải có 10 chữ số và bắt đầu bằng số 0.");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Ngày sinh không được để trống.")
                .Must(BeAtLeast15YearsOld).WithMessage("Khách hàng cá nhân phải từ đủ 15 tuổi trở lên.");
        }

        private bool BeAtLeast15YearsOld(DateOnly dob)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var age = today.Year - dob.Year;

            // Kiểm tra qua sinh nhật chưa
            if (dob > today.AddYears(-age))
            {
                age--;
            }
            return age >= 15;
        }
    }
}
