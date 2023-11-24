using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cell : MonoBehaviour
{
    //tuple of floats
    private int[] position = new int[2];
    private Entity content;

    void onMouseDown()
    {
        var disNuts = this;
        GameManager.instance.OnCellClicked(disNuts);
    }

    void FocusCell()
    {
        // Highlight the cell or change sprite in content
    }

}
