using UnityEngine;
using YNQ.InputField;

public class InputFieldRandomizeSpinnerOption : MonoBehaviour
{
    public void RandomizeOption(InputField_TextSpinner spinner)
    {
        spinner.SetIndex(Random.Range(0, spinner.Values.Length));
    }
}
