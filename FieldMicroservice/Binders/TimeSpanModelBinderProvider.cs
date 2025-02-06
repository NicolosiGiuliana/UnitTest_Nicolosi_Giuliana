using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace FieldMicroservice.Binders
{
    public class TimeSpanModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(TimeSpan) || context.Metadata.ModelType == typeof(TimeSpan?))
            {
                return new TimeSpanModelBinder();
            }

            return null;
        }
    }
}
