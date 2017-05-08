using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigureResultsToDisplay : MonoBehaviour {

    public TextMesh cooledText;
    public TextMesh receiveMedText;
    public TextMesh plasticWrapText;

    public void ConfigureResultDisplay(List<AreaStatus> _bwsList, bool didReceivePainMed, bool didNeedWrap)
    {
        int amountOfCooledWounds = 0;
        int amountOfWrappedWounds = 0;
        foreach (AreaStatus bws in _bwsList)
        {
            if (bws.isCooled)
                amountOfCooledWounds++;
            if (bws.isWrapped)
                amountOfWrappedWounds++;
        }

        cooledText.text = amountOfCooledWounds + "/" + _bwsList.Count + " burns were cooled";
        receiveMedText.text = "Received correct pain medication: " + didReceivePainMed;

        string pwrapString;
        if (didNeedWrap)
            pwrapString = "Pwrap was not necessary";
        else pwrapString = string.Format("Plastic wrap was necessary and {0}/{1} burns received it", amountOfWrappedWounds, _bwsList.Count);
        plasticWrapText.text = pwrapString;

    }
}
