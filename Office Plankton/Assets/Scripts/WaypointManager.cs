using UnityEngine;


public class WaypointManager : MonoBehaviour
{
    public GameObject WaypointPrefab;

    public static WaypointManager Singleton { get; private set; }

    private void Awake()
    {
        Singleton = this;
    }

    public Waypoint CreateWaypoint(Transform transform)
    {
        var createdWaypoint = Instantiate(WaypointPrefab, GameObject.FindObjectOfType<Canvas>().transform);
        var waypoint = createdWaypoint.GetComponent<Waypoint>();

        waypoint.SetTarget(transform);

        return waypoint;
    }
}
