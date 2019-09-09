using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class BluePrintDropDown : MonoBehaviour
{
    private bool toggle;
    public GameObject canvas,player;
    public Transform bps;
    public Dropdown dd;

    // Start is called before the first frame update
    void Start()
    {
        toggle = false;
        canvas.SetActive(toggle);
        dd.AddOptions(childNamesToList(bps));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            toggle = !toggle;
            canvas.SetActive(toggle);
            switchFPControls(!toggle);
            Debug.Log(toggle);
        }
    }

    void switchFPControls(bool v)
    {
        player.GetComponent<FirstPersonController>().enabled = v;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    /// Similar to children to list in Robobay, maybe this can all be one function see Robobay.cs
    List<string> childNamesToList(Transform parent)
    {
        List<string> outList = new List<string>();
        foreach (Transform child in parent)
        {
            outList.Add(child.name);
        }
        return outList;
    }
}
