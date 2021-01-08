using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public enum ScreenType
{
    A = 0,
    B = 1,
    C = 2
}

public class SampleScreenManager : VisualElement
{
    public new class UxmlFactory : UxmlFactory<SampleScreenManager, UxmlTraits>
    {
    }

    private readonly List<ScreenBase> _screenList = new List<ScreenBase>();

    private ScreenType _activeScreenType = ScreenType.A;
    private ScreenBase _currentScreen;

    public SampleScreenManager()
    {
        RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    private void OnGeometryChange(GeometryChangedEvent evt)
    {
        _screenList.Clear();
        var screenTypes = Enum.GetValues(typeof(ScreenType));
        foreach (ScreenType type in screenTypes)
        {
            var key = $"Screen{type.ToString()}";
            var s = this.Q<ScreenBase>(key);
            _screenList.Add(s);
        }

        var footerMenu = this.Q<VisualElement>("menu");
        footerMenu.Q<VisualElement>("A").RegisterCallback<ClickEvent>(e => GotoScreen(ScreenType.A));
        footerMenu.Q<VisualElement>("B").RegisterCallback<ClickEvent>(e => GotoScreen(ScreenType.B));
        footerMenu.Q<VisualElement>("C").RegisterCallback<ClickEvent>(e => GotoScreen(ScreenType.C));

        for (int i = 0; i < _screenList.Count; i++)
        {
            var screen = _screenList[i];
            screen.SetActive(false);
        }

        GotoScreen(ScreenType.A);
        UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    async Task GotoScreen(ScreenType type)
    {
        try
        {
            var nextScreen = GetScreen(type);
            nextScreen.Ready();

            if (_currentScreen != null)
            {
                await Task.WhenAll(
                    _currentScreen?.Hide(),
                    nextScreen.Show()
                );
            }
            else
            {
                await nextScreen.Show();
            }

            _currentScreen = nextScreen;
        }
        catch (Exception e)
        {
            Debug.LogError(e.StackTrace);
        }
    }

    ScreenBase GetScreen(ScreenType type)
    {
        return _screenList.Find(x => x.ScreenType == type);
    }
}