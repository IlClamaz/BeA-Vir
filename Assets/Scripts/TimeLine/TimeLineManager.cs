using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLineManager : MonoBehaviour
{
    [SerializeField] private GameObject doorB1;
    [SerializeField] private GameObject doorB2;
    [SerializeField] private GameObject doorB3;
    [SerializeField] private GameObject doorC;
    [SerializeField] private GameObject doorD;
    [SerializeField] private GameObject doorE;
    [SerializeField] private GameObject doorF;
    [SerializeField] private GameObject mapDoor;

    [Header("GameObject Area G")]
    [SerializeField] private GameObject areaG_Object_1;
    [SerializeField] private GameObject areaG_Object_2;
    [SerializeField] private GameObject areaG_Object_3;
    [SerializeField] private GameObject areaG_Object_4;
    [SerializeField] private GameObject areaG_Object_5;
    [SerializeField] private GameObject areaG_Object_6;
    [SerializeField] private GameObject areaG_Object_7;



    public void triggerEntered(string name)
    {
        if (!name.Contains('_'))
        {
            switch (name)
            {
                case "JomonTrigger":

                    doorB1.SetActive(false);
                    doorB2.SetActive(false);
                    doorB3.SetActive(false);
                    doorC.SetActive(false);
                    doorD.SetActive(false);
                    doorE.SetActive(false);
                    doorF.SetActive(false);
                    mapDoor.SetActive(false);
                    break;
                case "YayoiTrigger":
                    doorB1.SetActive(false);
                    doorB2.SetActive(false);
                    doorB3.SetActive(false);
                    doorC.SetActive(true);
                    doorD.SetActive(false);
                    doorE.SetActive(false);
                    doorF.SetActive(false);
                    mapDoor.SetActive(false);
                    break;
                case "KofunYayoiTrigger":
                    doorB1.SetActive(false);
                    doorB2.SetActive(false);
                    doorB3.SetActive(false);
                    doorC.SetActive(false);
                    doorD.SetActive(false);
                    doorE.SetActive(true);
                    doorF.SetActive(false);
                    mapDoor.SetActive(false);
                    break;
                case "KofunTrigger":
                    doorB1.SetActive(true);
                    doorB2.SetActive(false);
                    doorB3.SetActive(true);
                    doorC.SetActive(false);
                    doorD.SetActive(true);
                    doorE.SetActive(false);
                    doorF.SetActive(true);
                    mapDoor.SetActive(false);
                    break;
                case "EdoTrigger":
                    doorB1.SetActive(false);
                    doorB2.SetActive(true);
                    doorB3.SetActive(false);
                    doorC.SetActive(false);
                    doorD.SetActive(false);
                    doorE.SetActive(false);
                    doorF.SetActive(false);
                    mapDoor.SetActive(false);
                    break;
                case "ResetTimeLineBtn":
                    doorB1.SetActive(true);
                    doorB2.SetActive(true);
                    doorB3.SetActive(true);
                    doorC.SetActive(true);
                    doorD.SetActive(true);
                    doorE.SetActive(true);
                    doorF.SetActive(true);
                    mapDoor.SetActive(true);
                    break;
            }
        }
        else
        {
            switch (name)
            {
                case "EarlyKofunTrigger_":
                    areaG_Object_1.SetActive(true);
                    areaG_Object_2.SetActive(true);
                    areaG_Object_3.SetActive(false);
                    areaG_Object_4.SetActive(false);
                    areaG_Object_5.SetActive(false);
                    areaG_Object_6.SetActive(false);
                    break;
                case "MiddleKofunTrigger_":
                    areaG_Object_1.SetActive(false);
                    areaG_Object_2.SetActive(false);
                    areaG_Object_3.SetActive(true);
                    areaG_Object_4.SetActive(true);
                    areaG_Object_5.SetActive(false);
                    areaG_Object_6.SetActive(false);
                    break;
                case "LateKofunTrigger_":
                    areaG_Object_1.SetActive(false);
                    areaG_Object_2.SetActive(false);
                    areaG_Object_3.SetActive(false);
                    areaG_Object_4.SetActive(false);
                    areaG_Object_5.SetActive(true);
                    areaG_Object_6.SetActive(true);
                    break;
                case "ResetTimeLineBtn_":
                    areaG_Object_1.SetActive(true);
                    areaG_Object_2.SetActive(true);
                    areaG_Object_3.SetActive(true);
                    areaG_Object_4.SetActive(true);
                    areaG_Object_5.SetActive(true);
                    areaG_Object_6.SetActive(true);
                    break;
            }

        }

    }

    void OnEnable()
    {
        if (doorB1 != null)
        {
            doorB1.SetActive(true);
            doorB2.SetActive(true);
            doorB3.SetActive(true);
            doorC.SetActive(true);
            doorD.SetActive(true);
            doorE.SetActive(true);
            doorF.SetActive(true);
            mapDoor.SetActive(true);
        }
        else
        {
            areaG_Object_1.SetActive(true);
            areaG_Object_2.SetActive(true);
            areaG_Object_3.SetActive(true);
            areaG_Object_4.SetActive(true);
            areaG_Object_5.SetActive(true);
            areaG_Object_6.SetActive(true);
        }
    }

    private void Reset()
    {
        if (doorB1 == null)
        {
            areaG_Object_1.SetActive(true);
            areaG_Object_2.SetActive(true);
            areaG_Object_3.SetActive(true);
            areaG_Object_4.SetActive(true);
            areaG_Object_5.SetActive(true);
            areaG_Object_6.SetActive(true);
            areaG_Object_7.SetActive(true);
        }
    }

    /*     private void Activate(GameObject[] toBeActivated)
        {
            foreach (var go in toBeActivated)
            {
                go.SetActive(true);
            }
        }

        private void DeActivate(GameObject[] toBeDeActivated)
        {
            foreach (var go in toBeDeActivated)
            {
                go.SetActive(false);
            }
        } */
}
