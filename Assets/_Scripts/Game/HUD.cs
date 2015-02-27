using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour 
{
    public static HUD Instance { get; set; }

    public Text AmmoText;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
    }

    public void UpdateAmmo(int current, int max)
    {
        AmmoText.text = current.ToString();
    }
}
