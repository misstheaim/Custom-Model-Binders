using Custom_Model_Binders.infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Custom_Model_Binders.Models;

[ModelBinder(binderType: typeof(PointBinder))]
public record class Point
{
    public int x { get; set; }
    public int y { get; set; }
    public int z { get; set; }
}
