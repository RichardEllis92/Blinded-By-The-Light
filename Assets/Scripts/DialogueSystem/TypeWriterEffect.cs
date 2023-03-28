using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypeWriterEffect : MonoBehaviour
{
    [SerializeField] private float writingSpeed = 5f;

    public bool IsRunning { get; private set; }

    private readonly List<Punctuation> _punctuations = new List<Punctuation>()
    {
        new Punctuation(new HashSet<char>(){'.', '!', '?'}, 0.6f)
    };

    private Coroutine _typingCoroutine;
 
    public void Run(string textToType, TMP_Text textLabel)
    {
        //AudioManager.instance.PlaySFX(14);
        _typingCoroutine = StartCoroutine(TypeText(textToType, textLabel));
    }

    public void Stop()
    {
        StopCoroutine(_typingCoroutine);
        IsRunning = false;
    }

    private IEnumerator TypeText(string textToType, TMP_Text textLabel)
    {
        IsRunning = true;

        textLabel.text = string.Empty;
        
        //yield return new WaitForSeconds(2);
        
        float t = 0;
        int charIndex = 0;

        while (charIndex < textToType.Length)
        {
            
            int lastCharIndex = charIndex;

            t += Time.deltaTime * writingSpeed;
            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);

            for(int i = lastCharIndex; i < charIndex; i++)
            {
                //AudioManager.instance.PlaySFX(14);
                bool isLast = i >= textToType.Length - 1;
                
                textLabel.text = textToType.Substring(0, i + 1);

                if (IsPunctuation(textToType[i], out var waitTime) && !isLast && !IsPunctuation(textToType[i + 1], out _))
                {
                    AudioManager.Instance.StopSfx(14);
                }
                
            }
            yield return null;
        }
        AudioManager.Instance.StopSfx(14);
        IsRunning = false;
    }

    private bool IsPunctuation(char character, out float waitTime)
    {
        foreach(Punctuation punctuationCategory in _punctuations)
        {
            if (punctuationCategory.Punctuations.Contains(character))
            {
                waitTime = punctuationCategory.WaitTime;
                return true;
            }
        }

        waitTime = default;
        return false;
    }

    private readonly struct Punctuation
    {
        public readonly HashSet<char> Punctuations;
        public readonly float WaitTime;

        public Punctuation(HashSet<char> punctuations, float waitTime)
        {
            Punctuations = punctuations;
            WaitTime = waitTime;
        }
    }
}
