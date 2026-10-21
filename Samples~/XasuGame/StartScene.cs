using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Xasu;
using Xasu.HighLevel;

public class StartScene : MonoBehaviour
{
    public CanvasGroup buttons;
    public Button startBtn;

    public async void Start()
    {
        await Task.Yield();
        buttons.interactable = false;

        startBtn.onClick.AddListener(StartGame);
        buttons.interactable = true;
    }

    public async void StartGame()
    {
        startBtn.interactable = false;
        XasuTracker.Instance.Init();
        while(XasuTracker.Instance.Status.State == TrackerState.Uninitialized)
        {
            await Task.Yield();
        }
        SceneManager.LoadScene("ChoosePath");
    }
}
