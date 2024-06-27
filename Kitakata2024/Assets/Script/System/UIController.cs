using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    [SerializeField] private UIDocument sakeAmountBar;
    private ProgressBar _bar;
    private void Start()
    {
        _bar = sakeAmountBar.rootVisualElement.Q<ProgressBar>("SakeAmount");
    }

    public void UpdateSakeAmount(float v)
    {
        _bar.value = v;
    }
}
