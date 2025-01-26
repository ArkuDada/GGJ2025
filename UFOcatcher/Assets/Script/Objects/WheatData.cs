using NCC.Utility.Objects;
using UnityEngine;

[RequireComponent(typeof(GrowingObject))]
public class WheatData : MonoBehaviour
{
    public bool beingEaten => cowEating != null;
    public CowData cowEating = null; // Cow that's eating this wheat
}
