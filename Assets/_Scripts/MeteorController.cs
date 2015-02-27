using UnityEngine;
using System.Collections.Generic;

public class MeteorController : MonoBehaviour
{
    public static MeteorController Instance { get; set; }

    List<Meteor> Meteors;

    void Awake()
    {
        Instance = this;
        Meteors = new List<Meteor>();
    }

    public void Reset()
    {

    }
}
