using System;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

#pragma warning disable CS0649


public class UiPanelsManager : MonoBehaviour {
    UiPanel mainPanel;
    UiPanel[] nestedPanels;

    Stack<UiPanel> openedPanels = new Stack<UiPanel>();


    void Awake() {
        mainPanel = GetComponentInChildren<UiPanel>(true);
        mainPanel.Initialise();
        nestedPanels = mainPanel.GetComponentsInChildren<UiPanel>(true);
        LockCursor();
        foreach (var panel in nestedPanels) {
            panel.Initialise();
            panel.WindowOpenedEvent += HandlePanelOpened;
            panel.WindowClosedEvent += HandlePanelClosed;
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
            ToggleMenu();
    }

    void ToggleMenu() {
        if (!mainPanel.IsOpen) {
            FirstPersonController.InputEnabled = false;
            mainPanel.Open();
        }
        else
            CloseCurrentPanel();
    }

    void HandlePanelOpened(UiPanel panel) {
        openedPanels.Push(panel);
        UnlockCursor();
    }

    void HandlePanelClosed(UiPanel panel) {
        if (openedPanels.Contains(panel))
            openedPanels.Pop();
    }

    public void CloseCurrentPanel() {
        if (openedPanels.Count == 0)
            return;
        var openedPanel = openedPanels.Pop();
        openedPanel.Close();
        if (openedPanels.Count == 0) {
            FirstPersonController.InputEnabled = true;
            LockCursor();
        }
    }

    //assigned in inspector to ExitButton
    public void ExitGame() {
        Debug.Log("ExitGame");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    static void UnlockCursor() {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    static void LockCursor() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}

public abstract class UiPanel : MonoBehaviour {
    public event Action<UiPanel> WindowOpenedEvent = delegate { };
    public event Action<UiPanel> WindowClosedEvent = delegate { };

    bool initialised;

    public bool IsOpen { get; private set; }


    public void Initialise() {
        if (initialised)
            return;
        initialised = true;
        OnInitialise();
    }

    public void Open() {
        gameObject.SetActive(true);
        if (!IsOpen) {
            OnOpen();
            WindowOpenedEvent(this);
        }
        IsOpen = true;
    }

    public void Close() {
        gameObject.SetActive(false);
        if (IsOpen) {
            OnClose();
            WindowClosedEvent(this);
        }
        IsOpen = false;
    }

    protected abstract void OnInitialise();
    protected abstract void OnOpen();
    protected abstract void OnClose();
}