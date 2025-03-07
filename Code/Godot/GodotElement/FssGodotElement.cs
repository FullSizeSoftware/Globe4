
using Godot;

// A FssGodotPlatformElement corresponds to a FssElement, but has the Godot Node3D as a base class
// and is dedicated to display functionality in the Godot scene graph.

// "Elements" are specifically data driven, and managed to match data in the FssElement class structure.

public partial class FssGodotPlatformElement : Node3D
{
    // Overridable type for the element
    public virtual string ElemType {set; get; } = "Unknown";

    // A virtual functino for all element child classes to output a one-line report of their contents.
    public virtual string Report()
    {
        return $"Element: {Name} ({ElemType})";
    }
}