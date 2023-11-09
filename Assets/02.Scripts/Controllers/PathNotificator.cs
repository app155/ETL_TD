using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNotificator : MonoBehaviour
{
    public float _offTimer;
    [SerializeField] private float _offTime;

    private void OnEnable()
    {
        _offTimer = 0.0f;
    }

    void Update()
    {
        _offTimer += Time.deltaTime;

        if (_offTimer > _offTime)
        {
            gameObject.SetActive(false);
        }
    }
}
