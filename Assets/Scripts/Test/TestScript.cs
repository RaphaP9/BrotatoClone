using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestScript : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            ScenesManager.Instance.FadeLoadTargetScene(SceneManager.GetActiveScene().name);

        }
    }
}
