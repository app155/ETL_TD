using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PathPreviewButton : MonoBehaviour
{
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();

        _button.onClick.AddListener(() => StartCoroutine(SpawnManager.instance.SpawnPathNotificatorRoutine()));
    }
}
