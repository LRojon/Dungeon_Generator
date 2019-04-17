using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateEtage : MonoBehaviour
{
    void Update()
    {
        GetComponent<Text>().text = "Etage : " + DataStorage.Floor;
    }
}
