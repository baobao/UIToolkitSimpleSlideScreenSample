using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class ScreenBase : VisualElement
{
    public new class UxmlFactory : UxmlFactory<ScreenBase, UxmlTraits> { }
    public ScreenType ScreenType { get; set; }

    private const float ScreenWidth = 1280f;

    public void SetActive(bool isActive)
    {
        style.display = isActive ? DisplayStyle.Flex : DisplayStyle.None;
    }

    public void Ready()
    {
        var pos = transform.position;
        pos.x = ScreenWidth;
        transform.position = pos;
        SetActive(true);
    }

    public async Task Show()
    {
        bool isComplete = false;
        DOTween.To(() => transform.position, x => transform.position = x, Vector3.zero, 0.6f)
            .SetEase(Ease.InOutQuart).OnComplete(() => isComplete = true);

        while (isComplete == false)
        {
            await Task.Delay(1);
        }
    }

    public async Task Hide()
    {
        bool isComplete = false;
        DOTween.To(()=>transform.position, x=>transform.position = x, new Vector3(-ScreenWidth, 0, 0), 0.6f)
            .SetEase(Ease.InOutQuart).OnComplete(()=> isComplete = true);
        while (isComplete == false)
        {
            await Task.Delay(1);
        }
    }
}

public class ScreenA : ScreenBase
{
    public new class UxmlFactory : UxmlFactory<ScreenA, UxmlTraits> { }
    public ScreenA() { ScreenType = ScreenType.A; }
}

public class ScreenB : ScreenBase
{
    public new class UxmlFactory : UxmlFactory<ScreenB, UxmlTraits> { }
    public ScreenB() { ScreenType = ScreenType.B; }
}

public class ScreenC : ScreenBase
{
    public new class UxmlFactory : UxmlFactory<ScreenC, UxmlTraits> { }
    public ScreenC() { ScreenType = ScreenType.C; }
}