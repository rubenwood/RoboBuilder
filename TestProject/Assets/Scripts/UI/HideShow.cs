using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class HideShow : MonoBehaviour
{
    private bool toggle;
    public GameObject canvas,player;
    // Start is called before the first frame update
    void Start()
    {
        toggle = false;
        canvas.SetActive(toggle);
        ///switchFPControls(toggle);
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
}
