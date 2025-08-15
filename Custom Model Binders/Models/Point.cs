using Custom_Model_Binders.infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Custom_Model_Binders.Models;

[ModelBinder(binderType: typeof(PointBinder))]
public record class Point
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }
}
