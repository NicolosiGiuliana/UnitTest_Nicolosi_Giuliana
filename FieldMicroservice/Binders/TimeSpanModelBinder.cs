using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;

namespace FieldMicroservice.Binders
{
    public class TimeSpanModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult != ValueProviderResult.None)
            {
                var value = valueProviderResult.FirstValue;

                if (TimeSpan.TryParseExact(value, @"hh\:mm", CultureInfo.InvariantCulture, out var timeSpan))
                {
                    bindingContext.Result = ModelBindingResult.Success(timeSpan);
                    return Task.CompletedTask;
                }

                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "Invalid time format. Please use 'hh:mm'.");
            }

            return Task.CompletedTask;
        }
    }

}
