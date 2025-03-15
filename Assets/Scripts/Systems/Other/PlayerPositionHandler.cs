using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionHandler : MonoBehaviour
{
    public static PlayerPositionHandler Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private Transform player;

    public Transform Player => player;

    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one PlayerPositionHandler instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }
}
