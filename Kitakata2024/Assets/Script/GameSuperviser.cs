using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SakeShooterSystems;

public class GameSuperviser : MonoBehaviour
{
    public void OnMasuExit(MasuExitStatus status)
    {
        switch (status)
        {
            case MasuExitStatus.Success:
                Debug.Log("Masu Exit Success");
                break;
            case MasuExitStatus.Failure:
                Debug.Log("Masu Exit Failure");
                break;
        }
    }
}
