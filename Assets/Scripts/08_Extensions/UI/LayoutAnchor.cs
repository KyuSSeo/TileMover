using DG.Tweening;
using UnityEngine;

public class LayoutAnchor : MonoBehaviour
{
    //  기반이 되는 RectTransform
    RectTransform myRT;
    RectTransform parentRT;


    //  초기화
    private void Awake() => Init();
    private void Init()
    {
        myRT = transform as RectTransform;
        parentRT = transform.parent as RectTransform;
        if (parentRT == null)
            Debug.Log("부모 구성 요소 부족", gameObject);
    }

    //  주어진 TextAnchor 를 기준으로 상대 좌표 구하기
    private Vector2 GetPosition(RectTransform rt, TextAnchor anchor)
    {
        Vector2 retValue = Vector2.zero;

        //  X좌표 확인
        switch (anchor)
        {
            case TextAnchor.LowerCenter:
            case TextAnchor.MiddleCenter:
            case TextAnchor.UpperCenter:
                retValue.x += rt.rect.width * 0.5f;
                break;
            case TextAnchor.LowerRight:
            case TextAnchor.MiddleRight:
            case TextAnchor.UpperRight:
                retValue.x += rt.rect.width;
                break;
        }
        //  Y좌표 확인
        switch (anchor)
        {
            case TextAnchor.MiddleLeft:
            case TextAnchor.MiddleCenter:
            case TextAnchor.MiddleRight:
                retValue.y += rt.rect.height * 0.5f;
                break;
            case TextAnchor.UpperLeft:
            case TextAnchor.UpperCenter:
            case TextAnchor.UpperRight:
                retValue.y += rt.rect.height;
                break;
        }

        return retValue;
    }


    public Vector2 AnchorPosition(TextAnchor myAnchor, TextAnchor parentAnchor, Vector2 offset)
    {
        //  기준 위치
        Vector2 myOffset = GetPosition(myRT, myAnchor);

        //  부모 기준 위치
        Vector2 parentOffset = GetPosition(parentRT, parentAnchor);

        //  앵커 중심
        Vector2 anchorCenter = new Vector2(
            Mathf.Lerp(myRT.anchorMin.x, myRT.anchorMax.x, myRT.pivot.x),
            Mathf.Lerp(myRT.anchorMin.y, myRT.anchorMax.y, myRT.pivot.y)
            );

        //  앵커 위치 보정
        Vector2 myAnchorOffset = new Vector2(
            parentRT.rect.width * anchorCenter.x,
            parentRT.rect.height * anchorCenter.y
            );

        //  피벗 보정
        Vector2 myPivotOffset = new Vector2(
            myRT.rect.width * myRT.pivot.x,
            myRT.rect.height * myRT.pivot.y
            );

        //  최종 위치 선정
        Vector2 pos = parentOffset - myAnchorOffset - myOffset + myPivotOffset + offset;
        pos.x = Mathf.RoundToInt(pos.x);
        pos.y = Mathf.RoundToInt(pos.y);

        return pos;
    }
    //  UI 순간 배치, 부모의 앵커 기준 위치로 즉시 이동
    public void SnapToAnchorPosition(TextAnchor myAnchor, TextAnchor parentAnchor, Vector2 offset)
    {
        myRT.anchoredPosition = AnchorPosition(myAnchor, parentAnchor, offset);
    }

    //  UI 애니메이션 배치, 부모의 앵커 기준 위치로 부드럽게 이동
    public Tweener MoveToAnchorPosition(TextAnchor myAnchor, TextAnchor parentAnchor, Vector2 offset)
    {
        return myRT.AnchorTo(AnchorPosition(myAnchor, parentAnchor, offset));
    }
}