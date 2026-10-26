using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;

namespace GestionArticles.Data
{
    public class DecimalModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            var value = valueProviderResult.FirstValue;
            if (string.IsNullOrWhiteSpace(value))
            {
                if (bindingContext.ModelType == typeof(decimal?))
                {
                    bindingContext.Result = ModelBindingResult.Success(null);
                }
                return Task.CompletedTask;
            }

            // Nettoyer les espaces (y compris insécables) et convertir la virgule en point
            var cleanedValue = value
                .Replace(" ", "")
                .Replace("\u00A0", "")
                .Replace(',', '.');

            if (decimal.TryParse(cleanedValue, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
            {
                bindingContext.Result = ModelBindingResult.Success(result);
            }
            else
            {
                bindingContext.ModelState.TryAddModelError(
                    bindingContext.ModelName,
                    $"La valeur '{value}' n'est pas un nombre valide."
                );
            }

            return Task.CompletedTask;
        }
    }

    public class DecimalModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(decimal) || context.Metadata.ModelType == typeof(decimal?))
            {
                return new DecimalModelBinder();
            }
            return null;
        }
    }
}
