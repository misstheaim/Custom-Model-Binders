using Custom_Model_Binders.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Custom_Model_Binders.infrastructure;

public class PointBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null) throw new ArgumentNullException(nameof(bindingContext));

        string modelName = bindingContext.ModelName;

        var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

        if (valueProviderResult == ValueProviderResult.None) return Task.CompletedTask;

        string value = valueProviderResult.First();

        if (string.IsNullOrEmpty(value)) return Task.CompletedTask;

        int[] coordArray = new int[3];

        int errorsCountInInput = 0;

        coordArray = value.Split(',').Select(c => {
            int result;
            errorsCountInInput = int.TryParse(c.Trim(), out result) ? errorsCountInInput : ++errorsCountInInput;
            return result;
        }).ToArray();

        if (errorsCountInInput != 0 || coordArray.Length != 3)
        {
            bindingContext.ModelState.TryAddModelError(modelName, "Incorrect input. Coords should be numbers in the amount of 3.");
            return Task.CompletedTask;
        }

        Point point = new Point() { X = coordArray[0], Y = coordArray[1], Z = coordArray[2] };

        bindingContext.Result = ModelBindingResult.Success(point);

        return Task.CompletedTask;
    }
}
