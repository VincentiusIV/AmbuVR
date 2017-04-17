using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypadButton : MonoBehaviour {

    public TBSA_Controller tbsa;
    public int change;
    public bool finish;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("VR_Controller"))
        {
            tbsa.UpdateInputField(change);

            if (finish)
                tbsa.FinishAttempt();
        }
    }

}
