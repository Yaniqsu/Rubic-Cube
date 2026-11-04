using UnityEngine;
using YNQ.InputField;

public class InputField_Vector2Spinner : InputField_ValueSpinner<Vector2>
{
    protected override string FormatValueDisplay(Vector2 value)
        => $"[ {value.x} x {value.y} ]";
}
