using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogContent", menuName = "Scriptable Objects / Dialog Content")]
public class DialogContent : ScriptableObject
{
    [field: SerializeField]
    public string Header { get; private set; }
    
    [field: SerializeField, TextArea(5, 10)]
    public string[] Slides { get; private set; }

    [field: SerializeField] 
    public bool UseNegativeButton { get; private set; } = false;

    [field: SerializeField] 
    public string NextButtonText { get; private set; } = "Next";

    [field: SerializeField, EnableIf(nameof(UseNegativeButton))]
    public string NegativeButtonText { get; private set; } = "Cancel";

    [field: SerializeField] 
    public string PositiveButtonText { get; private set; } = "OK";

}
