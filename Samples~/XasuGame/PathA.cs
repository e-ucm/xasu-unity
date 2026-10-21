using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Xasu;
using Xasu.HighLevel;
public class PathA : MonoBehaviour
{
    public CanvasGroup buttons;
    public Button interacted, goToEnd;

    public async void Start()
    {
        await Task.Yield();
        buttons.interactable = false;

        Debug.Log("[PATHA] Sending Progressed trace");
        CompletableTracker.Instance.Progressed("MyGame", CompletableTracker.CompletableType.Game, 0.5f);
        Debug.Log("[PATHA] Done!");
        
        Debug.Log("[PATHA] Sending Initialized pathA level");
        CompletableTracker.Instance.Initialized("pathA", CompletableTracker.CompletableType.Level);
        Debug.Log("[PATHA] Done!");

        interacted.onClick.AddListener(Interacted);
        goToEnd.onClick.AddListener(GoToEnd);

        buttons.interactable = true;
    }

    public void Interacted()
    {
        interacted.interactable = false;
        Debug.Log("[PATHA] Sending Interacted trace");
        GameObjectTracker.Instance.Interacted("pathA-interacted-button");
        Debug.Log("[PATHA] Done!");
        interacted.interactable = true;
    }

    public void GoToEnd()
    {
        goToEnd.interactable = false;
        Debug.Log("[PATHA] Sending Completed pathA level");
        CompletableTracker.Instance.Completed("pathA", CompletableTracker.CompletableType.Level).WithSuccess(true);
        Debug.Log("[PATHA] Done!");
        Debug.Log("[PATHA] Sending Accessed trace");
        AccessibleTracker.Instance.Accessed("endScene");
        Debug.Log("[PATHA] Done!");
        goToEnd.interactable = true;
        SceneManager.LoadScene("End");
    }
}
