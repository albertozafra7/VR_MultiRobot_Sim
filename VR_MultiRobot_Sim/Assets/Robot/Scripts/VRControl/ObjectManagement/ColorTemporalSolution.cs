using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTemporalSolution : MonoBehaviour
{
    [Header("ABB IRB6640 Colors")]
    public List<Renderer> ABB_IRB_6640_meshes;
    public List<Color> ABB_IRB_6640_Color;

    [Header("UR10 Colors")]
    public List<Renderer> UR10_meshes;
    public List<Color> UR10_Color;

    [Header("Conveyor Colors")]
    public List<Renderer> Conveyor_meshes;
    public List<Color> Conveyor_Color;

    [Header("Table Colors")]
    public List<Renderer> Table_meshes;
    public List<Color> Table_Color;

    public Queue<Color> getOriginalColors(string Desiredname){

        if(Desiredname == "abb_irb6640_185_280")
            return list2QUeue(ABB_IRB_6640_Color);
        
        if(Desiredname == "ur10")
            return list2QUeue(UR10_Color);

        if(Desiredname == "Conveyor")
            return list2QUeue(Conveyor_Color);

        if(Desiredname == "Working_Table")
            return list2QUeue(Table_Color);

        return null;
    }

    public Queue<Color> list2QUeue(List<Color> desiredColors){
        Queue<Color> originalColors = new Queue<Color>();

        foreach(Color deColor in desiredColors)
            originalColors.Enqueue(deColor);
        return originalColors;

    }
    public Queue<Color> getColors(List<Renderer> desiredRenders){

        Queue<Color> originalColors = new Queue<Color>();

        foreach (Renderer mesh_Renderer in desiredRenders) {
            for(int i = 0; i < mesh_Renderer.materials.Length; i++){
                    originalColors.Enqueue(mesh_Renderer.materials[i].color);
            }
        }
        return originalColors;
    }
}
