using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("Disappear", 1.0f);
    }

    void Disappear()
    {
        gameObject.SetActive(false);
    }
}
