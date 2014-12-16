using UnityEngine;
using System.Collections;

public class MeteorController : MonoBehaviour
{
    public static MeteorController Instance { get; set; }

    void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
