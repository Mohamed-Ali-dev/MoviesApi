using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace MoviesApi.Helpers
{
    public class TypeBinder<T>(ILogger<TypeBinder<T>> logger) : IModelBinder
    {
        private readonly ILogger logger = logger;

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
           
            // 1. Retrieve the name of the property being bound.
            var propertyName = bindingContext.ModelName;
            // 2. Attempt to get the value for the property from the value provider.
            var value = bindingContext.ValueProvider.GetValue(propertyName);

            // 3. Check if the value provider returned no result (i.e., the value is not present).
            if (value == ValueProviderResult.None)
            {
                // 4. If no value is found, return a completed task without setting any result.
                return Task.CompletedTask;
            }
            else
            {

                try
                {
                    // 5. If a value is found, attempt to deserialize it into the generic type T.
                    var deserializedValue = JsonConvert.DeserializeObject<T>(value.FirstValue);
                    // 6. If deserialization is successful, set the result of the binding context to the deserialized value.
                    bindingContext.Result = ModelBindingResult.Success(deserializedValue);
                }
                catch(Exception ex) 
                {
                    logger.LogError(ex, "Exception at binding");
                    // 7. If an exception occurs during deserialization, add an error to the model state.
                    bindingContext.ModelState.TryAddModelError(propertyName, "The given value is not of the correct type");
                }
                return Task.CompletedTask;
            }
        }
    }
}
