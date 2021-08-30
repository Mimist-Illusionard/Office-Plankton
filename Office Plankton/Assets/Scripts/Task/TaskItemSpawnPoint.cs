using System.Collections.Generic;
using UnityEngine;


public class TaskItemSpawnPoint : MonoBehaviour, ICarryObject
{
    public List<CarryType> PlaceTaskTypes;
    public List<CarryType> Type => PlaceTaskTypes;
}
