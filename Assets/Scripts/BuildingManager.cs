using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] private Room logistics, engineering, barracks, misc;

    public Room Logistics { get => logistics;}
    public Room Engineering { get => engineering;}
    public Room Barracks { get => barracks;}
    public Room Misc { get => misc;}
}
