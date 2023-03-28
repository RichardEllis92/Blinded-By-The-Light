using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class ResponseHandler : MonoBehaviour
{
    [SerializeField] private RectTransform responseBox;
    [SerializeField] private RectTransform responseButtonTemplate;
    [SerializeField] private RectTransform responseContainer;

    private DialogueUI _dialogueUI;
    private ResponseEvent[] _responseEvents;

    private List<GameObject> _tempResponseButtons = new List<GameObject>();

    private void Start()
    {
        _dialogueUI = GetComponent<DialogueUI>();
    }

    public void AddResponseEvents(ResponseEvent[] responseEvents)
    {
        this._responseEvents = responseEvents;
    }
        

    public void ShowResponses(Response[] responses)
    {
        float responseBoxHeight = 0;

        for (int i = 0; i < responses.Length; i++)
        {
            Response response = responses[i];
            int responseIndex = i;

            GameObject responseButton = Instantiate(responseButtonTemplate.gameObject, responseContainer);
            responseButton.gameObject.SetActive(true);
            responseButton.GetComponent<TMP_Text>().text = response.ResponseText;
            responseButton.GetComponent<Button>().onClick.AddListener(() => OnPickedResponse(response, responseIndex));

            _tempResponseButtons.Add(responseButton);

            responseBoxHeight += responseButtonTemplate.sizeDelta.y;
        }

        responseBox.sizeDelta = new Vector3(responseBox.sizeDelta.x, responseBoxHeight);
        responseBox.gameObject.SetActive(true);
    }

    private void OnPickedResponse(Response response, int responseIndex)
    {
        responseBox.gameObject.SetActive(false);

        foreach(GameObject button in _tempResponseButtons)
        {
            Destroy(button);
        }
        _tempResponseButtons.Clear();

        if(_responseEvents != null && responseIndex <= _responseEvents.Length)
        {
            _responseEvents[responseIndex].OnPickedResponse?.Invoke();
        }

        _responseEvents = null;

        if (response.DialogueObject)
        {
            _dialogueUI.ShowDialogue(response.DialogueObject);
        }
        else
        {
            _dialogueUI.CloseDialogueBox();
        }
        
    }
}
