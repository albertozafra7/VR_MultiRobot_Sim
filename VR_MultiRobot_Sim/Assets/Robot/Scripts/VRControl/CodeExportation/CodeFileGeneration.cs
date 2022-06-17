using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class CodeFileGeneration : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Targets;
    Target aux1;
    public Text text;
    public Canvas C1, C2;
    public GameObject pointer;
    public string path = "";
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
    }
    public void GenerateCode()
    {
        string content = "";
        //path of the file
        path = Application.dataPath + "/RobotProgram.script";
        //Debug.LogWarning("PATHH:" + Application.dataPath);
        //Create file if it doesn't exist
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "");
        }
        //content of the file
        for (int i = 0; i < Targets.transform.childCount; ++i)
        {
            aux1 = Targets.transform.GetChild(i).GetComponent<Target>();
            content = content + aux1.targetDef + "\n";
        }
        content = content + "\n";
        for (int i = 0; i < Targets.transform.childCount; ++i)
        {
            aux1 = Targets.transform.GetChild(i).GetComponent<Target>();
            content = content + aux1.instruction + "\n";
        }
        //add content
        File.WriteAllText(path, content);
    }
    public void AskToSaveYes()
    {
        C1.gameObject.SetActive(false);
        C2.gameObject.SetActive(true);
        GenerateCode();
        text.text = path;
    }
    public void AskToSaveCancel()
    {
        C1.gameObject.SetActive(false);
        C2.gameObject.SetActive(false);
        pointer.gameObject.SetActive(false);
    }
    public void OkPressed()
    {
        C1.gameObject.SetActive(false);
        C2.gameObject.SetActive(false);
        pointer.gameObject.SetActive(false);
    }
    public void SaveTab()
    {
        C1.gameObject.SetActive(true);
        pointer.gameObject.SetActive(true);
    }
}