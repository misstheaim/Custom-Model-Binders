using Custom_Model_Binders.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Custom_Model_Binders.infrastructure;

public class PersonBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null) throw new ArgumentNullException(nameof(bindingContext));

        string modelName = bindingContext.ModelName;

        var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

        if (valueProviderResult == ValueProviderResult.None) return Task.CompletedTask;

        string value = valueProviderResult.First();

        if (string.IsNullOrEmpty(value)) return Task.CompletedTask;

        byte[]? byteGuid;
        try
        {
            byteGuid = Convert.FromBase64String(value.Trim());
        }
        catch (Exception)
        {
            bindingContext.ModelState.TryAddModelError(modelName, "base64 encoded string is not correct");
            byteGuid = null;
        }
        
        if (byteGuid is null)
        {
            return Task.CompletedTask;
        }

        Guid guid = new Guid(byteGuid);

        Person? person = DataBase.People.FirstOrDefault(p => p.Id.Equals(guid));

        if (person is null)
        {
            return Task.CompletedTask;
        }

        bindingContext.Result = ModelBindingResult.Success(person);

        return Task.CompletedTask;
    }
}
