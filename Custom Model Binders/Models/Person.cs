using Custom_Model_Binders.infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Custom_Model_Binders.Models;

[ModelBinder(binderType: typeof(PersonBinder))]
public record class Person
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public int Age { get; set; }
}
