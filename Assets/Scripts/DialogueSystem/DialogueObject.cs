using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialogueObject")]
public class DialogueObject : ScriptableObject
{
    [SerializeField] [TextArea] public string[] dialogue;
    [SerializeField] private Response[] responses;

    public new string name;
    public string[] Dialogue => dialogue;
    
    public bool HasResponses => Responses != null && Responses.Length > 0;

    public Response[] Responses => responses;
    public string DialogueName { get; set; }
}
