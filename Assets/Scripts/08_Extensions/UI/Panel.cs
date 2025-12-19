using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// UI 오브젝트를 위치시키고, 애니메이션으로 이동시키는 기능 제공
// LayoutAnchor 컴포넌트를 반드시 요구
[RequireComponent(typeof(LayoutAnchor))]
public class Panel : MonoBehaviour
{
    [SerializeField] List<Position> positionList;
    Dictionary<string, Position> positionMap;
    LayoutAnchor anchor;

    // 현재 위치
    public Position CurrentPosition { get; private set; }
    // 현재 실행 중인 애니메이션
    public Tweener Transition { get; private set; }
    // 애니메이션 진행 여부
    public bool InTransition { get { return Transition != null; } }

    // 위치 이름으로 해당 Position을 가져오는 인덱서
    public Position this[string name]
    {
        get
        {
            if (positionMap.ContainsKey(name))
                return positionMap[name];
            return null;
        }
    }


    [Serializable]
    public class Position
    {
        public string name;
        public TextAnchor myAnchor;
        public TextAnchor parentAnchor;
        public Vector2 offset;

        // 이름 설정
        public Position(string name)
        {
            this.name = name;
        }

        // 이름과 앵커 설정
        public Position(string name, TextAnchor myAnchor, TextAnchor parentAnchor) : this(name)
        {
            this.myAnchor = myAnchor;
            this.parentAnchor = parentAnchor;
        }

        // 이름, 앵커, 오프셋까지 설정
        public Position(string name, TextAnchor myAnchor, TextAnchor parentAnchor, Vector2 offset) : this(name, myAnchor, parentAnchor)
        {
            this.offset = offset;
        }
    }

    private void Awake() => Init();
    private void Init()
    {
        anchor = GetComponent<LayoutAnchor>();
        positionMap = new Dictionary<string, Position>(positionList.Count);
        // 리스트에 있는 위치 정보를 딕셔너리에 등록
        for (int i = positionList.Count - 1; i >= 0; --i)
            AddPosition(positionList[i]);
    }


    // 새 위치 정보 딕셔너리 추가
    public void AddPosition(Position p)
    {
        positionMap[p.name] = p;
    }

    // 위치 정보 딕셔너리 제거
    public void RemovePosition(Position p)
    {
        if (positionMap.ContainsKey(p.name))
            positionMap.Remove(p.name);
    }

    // 위치 이름기반 설정
    public Tweener SetPosition(string positionName, bool animated)
    {
        return SetPosition(this[positionName], animated);
    }
    // Position지점 위치 설정
    public Tweener SetPosition(Position p, bool animated)
    {
        // 현재 위치
        CurrentPosition = p;
        // 위치가 동일한가?  
        if (CurrentPosition == null)
            return null;

        // 기존 애니메이션 중지
        if (InTransition)
            Transition.Stop();

        //  애니메이션 여부에 따라 애니메이션 적용 여부 선택
        if (animated)
        {
            Transition = anchor.MoveToAnchorPosition(p.myAnchor, p.parentAnchor, p.offset);
            return Transition;
        }
        else
        {
            anchor.SnapToAnchorPosition(p.myAnchor, p.parentAnchor, p.offset);
            return null;
        }
    }
    private void Start()
    {
        // 위치 지정이 없으면 첫 위치로 설정
        if (CurrentPosition == null && positionList.Count > 0)
            SetPosition(positionList[0], false);
    }
}