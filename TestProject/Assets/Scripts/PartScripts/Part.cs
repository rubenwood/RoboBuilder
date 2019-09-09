using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/**Billboard
 * http://wiki.unity3d.com/index.php/CameraFacingBillboard
 */

public class Part : MonoBehaviour
{
    public bool allowShowMods; ///if true this parts mods can be shown
    
    [SerializeField]
    private int maxProgress; ///used in PartSelector
    [SerializeField]
    private string partName; ///used in robobay
    [SerializeField]
    private List<string> mods;

    private GameObject textMesh;
    private bool canBB;

    // Start is called before the first frame update
    void Start()
    {
        canBB = false;
        if(allowShowMods)
        {
            GameObject textMeshPro = GameObject.Find("TextMeshPro");
            textMesh = Instantiate(textMeshPro, transform.position, transform.rotation, transform);
            textMesh.SetActive(false);
            textMesh.GetComponent<TextMeshPro>().text = partName;
            ///if this part has mods also add those
            if(mods.Count > 0)
            {
                textMesh.GetComponent<TextMeshPro>().text = textMesh.GetComponent<TextMeshPro>().text + "\n" + string.Join("\n", mods);
            }

            canBB = true;
        }
    }

    void billboard() ///Causes text to always face camera
    {
        ///yield return new WaitForSeconds(0.1f);
        Vector3 lookPos = transform.position + Camera.main.transform.rotation * Vector3.forward;
        textMesh.transform.LookAt(lookPos, Camera.main.transform.rotation * Vector3.up);
    }

    void LateUpdate() ///Do stuff regarding the hover text in late update
    {
        if (canBB && Input.GetKey(KeyCode.C) && textMesh)
        {
            textMesh.SetActive(true);
            ///StartCoroutine(billboard());
            billboard();
        }
        else if(textMesh && !Input.GetKey(KeyCode.C))
        {
            textMesh.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addMod(string rolledMod)
    {
        mods.Add(rolledMod);
    }

    public List<string> getMods()
    {
        return mods;
    }

    public string getMod(int i)
    {
        return mods[i];
    }

    public string getName()
    {
        return partName;
    }

    public int getMaxProg()
    {
        return maxProgress;
    }
}
