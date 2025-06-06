using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueSentence
{
    public int localID;
    public DialogueSpeakerSO dialogueSpeakerSO;
    [TextArea(3,10)] public string sentenceText;
    [Space]
    public bool speakerOnRight;
    public bool triggerSentenceTransition;
    [Space]
    public AudioClip audioClip;
}
