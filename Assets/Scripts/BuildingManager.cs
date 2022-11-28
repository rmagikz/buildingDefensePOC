using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] private Room logistics, engineering, barracks, misc;
    private Vector3 buildingPosition;

    public Room Logistics { get => logistics;}
    public Room Engineering { get => engineering;}
    public Room Barracks { get => barracks;}
    public Room Misc { get => misc;}
    public Vector3 BuildingPosition { get => buildingPosition; set => buildingPosition = value; }

    void Start() {
        buildingPosition = transform.position;
    }
}
