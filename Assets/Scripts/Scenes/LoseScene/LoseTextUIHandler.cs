using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoseTextUIHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI loseText;

    private void Start()
    {
        SetLoseText();
    }

    private void SetLoseText()
    {
        loseText.text = $"Has sobrevivido durante {GameManager.LastWave} oleada(s) al Yacuruna";
    }
}
