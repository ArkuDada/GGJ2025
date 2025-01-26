using System;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class Arcade : MonoBehaviour
{
    public Transform FeverMeter;
    public List<QuestButtonMesh> mesh;

    public List<Material> iconMats;

    private void Awake()
    {
        foreach(var questButtonMesh in mesh)
        {
            questButtonMesh.BorderMesh.material = new Material(questButtonMesh.BorderMesh.material);
        }

        for(int i = 0; i < 4; i++)
        {
            SetButtonIcon(i,Objects.ObjectType.End);
        }
    }
    
    private void SetButtonIcon(int index,Objects.ObjectType objectType)
    {
        mesh[index].IconMesh.material = new Material(iconMats[(int)objectType]);
    }
    
    
    //fill 0 - 1.f
    private void SetBorderFill(int index,float fill)
    {
        fill = (1.0f - Mathf.Clamp(fill, 0.0f, 1.0f)) * 360.0f;
        mesh[index].BorderMesh.material.SetFloat("_Arc2", fill);
    }
}

[Serializable]
public struct QuestButtonMesh
{
    public MeshRenderer IconMesh;
    public MeshRenderer BorderMesh;
}
