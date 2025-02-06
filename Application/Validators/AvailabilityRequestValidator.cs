using Application.DTOS.Request;
using FluentValidation;
using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class AvailabilityRequestValidator : AbstractValidator<AvailabilityRequest>
    {
        public AvailabilityRequestValidator()
        {
            RuleFor(x => x.OpenHour)
           .Must(BeValidTime)
           .WithMessage("OpenHour must be a valid time (between 00:00 and 23:59).")
           .LessThan(x => x.CloseHour)
           .WithMessage("OpenHour must be earlier than CloseHour.");

            RuleFor(x => x.CloseHour)
                .Must(BeValidTime)
                .WithMessage("CloseHour must be a valid time (between 00:00 and 23:59).");

            RuleFor(x => x.Day)
                .NotEmpty().WithMessage("Day is required.")
                .Must(BeAValidDay).WithMessage("Day must be a valid day of the week.");
        }

        private bool BeValidTime(TimeSpan time)
        {
            return time.Hours >= 0 && time.Hours < 24 && time.Minutes >= 0 && time.Minutes < 60;
        }

        private bool BeAValidDay(string day)
        {
            RemoveDiacritics(day);
            var validDays = CultureInfo.CurrentCulture.DateTimeFormat.DayNames;
            return validDays.Contains(day, StringComparer.OrdinalIgnoreCase);
        }


        private static string RemoveDiacritics(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
