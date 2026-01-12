using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class AbilityMenuPanelController : MonoBehaviour
{
    const string ShowKey = "Show";
    const string HideKey = "Hide";
    const string EntryPoolKey = "AbilityMenuPanel.Entry";
    const int MenuCount = 4;
    [SerializeField] GameObject entryPrefab;
    [SerializeField] TextMeshProUGUI menuTitleLabel;
    [SerializeField] TextMeshProUGUI resultLabel;
    [SerializeField] Panel panel;
    [SerializeField] GameObject menuCanvas;
    [SerializeField] GameObject resultCanvus;
    List<AbilityMenuEntry> menuEntries = new List<AbilityMenuEntry>(MenuCount);
    public int selection { get; private set; }

    private void Awake()
    {
        // 풀에 메뉴 항목 미리 등록
        ObjectPoolController.AddEntry(EntryPoolKey, entryPrefab, MenuCount, int.MaxValue);
    }

    // 풀에서 꺼내기
    private AbilityMenuEntry Dequeue()
    {
        //  풀에서 가져오기
        Poolable p = ObjectPoolController.Dequeue(EntryPoolKey);
        AbilityMenuEntry entry = p.GetComponent<AbilityMenuEntry>();

        //  패널에 적용하기
        entry.transform.SetParent(panel.transform, false);
        entry.transform.localScale = Vector3.one;

        //  활성화
        entry.gameObject.SetActive(true);
        entry.Reset();
        return entry;
    }

    // 항목을 풀에 반환하기
    private void Enqueue(AbilityMenuEntry entry)
    {
        Poolable p = entry.GetComponent<Poolable>();
        ObjectPoolController.Enqueue(p);
    }

    // 모든 항목 반환 및 리스트 비우기
    private void Clear()
    {
        for (int i = menuEntries.Count - 1; i >= 0; --i)
            Enqueue(menuEntries[i]);
        menuEntries.Clear();
    }

    // 초기 설정 
    private void Start()
    {
        //  비활성화, 숨기기
        panel.SetPosition(HideKey, false);
        menuCanvas.SetActive(false);
        resultCanvus.SetActive(false);
    }

    // 패널 위치 전환 애니메이션
    Tweener TogglePos(string pos)
    {
        Tweener t = panel.SetPosition(pos, true);
        t.easingControl.duration = 0.5f;
        t.easingControl.equation = EasingEquations.EaseOutQuad;
        return t;
    }

    // 선택 항목 변경
    private bool SetSelection(int value)
    {
        //  잠긴 항목은 선택 불가능
        if (menuEntries[value].IsLocked)
            return false;

        // 기존 선택 해제
        if (selection >= 0 && selection < menuEntries.Count)
            menuEntries[selection].IsSelected = false;

        selection = value;

        // 새 항목 선택
        if (selection >= 0 && selection < menuEntries.Count)
            menuEntries[selection].IsSelected = true;

        return true;
    }

    // 다음 선택 항목으로 이동
    public void Next()
    {
        for (int i = selection + 1; i < selection + menuEntries.Count; ++i)
        {
            int index = i % menuEntries.Count;
            if (SetSelection(index))
                break;
        }
    }

    // 이전 선택 항목으로 이동
    public void Previous()
    {
        for (int i = selection - 1 + menuEntries.Count; i > selection; --i)
        {
            int index = i % menuEntries.Count;
            if (SetSelection(index))
                break;
        }
    }

    // 메뉴 표시
    public void MenuShow(string title, List<string> options)
    {
        // 메뉴 캔버스 활성화
        menuCanvas.SetActive(true);
        Clear();
        menuTitleLabel.text = title;
        // 항목 생성 및 초기화
        for (int i = 0; i < options.Count; ++i)
        {
            AbilityMenuEntry entry = Dequeue();
            entry.Title = options[i];
            menuEntries.Add(entry);
        }
        SetSelection(0);
        TogglePos(ShowKey);
    }

    public void ResultShow(string name)
    {
        menuCanvas.SetActive(false);
        Clear();
        resultLabel.text = name;
    }
    //  잠금 설정
    public void SetLocked(int index, bool value)
    {
        if (index < 0 || index >= menuEntries.Count)
            return;

        // 선택 중인 항목이 잠기면 다음 항목으로 이동
        menuEntries[index].IsLocked = value;
        if (value && selection == index)
            Next();
    }

    // 메뉴 숨기기
    public void MenuHide()
    {
        Tweener t = TogglePos(HideKey);
        t.easingControl.completedEvent += delegate (object sender, System.EventArgs e)
        {
            //  비활성화
            if (panel.CurrentPosition == panel[HideKey])
            {
                Clear();
                menuCanvas.SetActive(false);
            }
        };
    }
}