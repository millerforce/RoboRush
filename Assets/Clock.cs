using TMPro;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField]
    public TMP_Text clockText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        clockText.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
