using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building
{

    Vector3 location;
    public Vector3 Location { get => location; }

    Vector2Int size;
    public Vector2Int Size { get => size; }

    List <Floor> floors;
    public List<Floor> Floors { get => floors; }

    public Building(Vector3 location, Vector2Int size, List<Floor> floors){
        this.floors = floors;
        this.size = size;
        this.location = location;
    }

    public override string ToString() {

        string building = "Building: (" + floors.Count + ")\n";

        foreach (Floor f in floors)
        {
            building += "\t" + f.ToString() + "\n";
        }

        return building;
    }

}
