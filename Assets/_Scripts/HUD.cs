using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour 
{
    public Text AmmoText;

    void Start()
    {
    }

    public void UpdateAmmo(int current, int max)
    {
        AmmoText.text = current.ToString();
    }
}
