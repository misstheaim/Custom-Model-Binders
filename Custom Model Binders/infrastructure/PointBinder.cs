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

        bool isInputCorrect = true;

        coordArray = value.Split(',').Select(c => {
            int result;
            isInputCorrect = int.TryParse(c.Trim(), out result);
            return result;
        }).ToArray();

        if (!isInputCorrect || coordArray.Length != 3) return Task.CompletedTask;

        Point point = new Point() { x = coordArray[0], y = coordArray[1], z = coordArray[2] };

        bindingContext.Result = ModelBindingResult.Success(point);

        return Task.CompletedTask;
    }
}
