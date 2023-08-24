using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerClickHandler
{
    public Action<PointerEventData> OnBeginDragHandler = null;
    public Action<PointerEventData> OnDragHandler = null;
    public Action<PointerEventData> OnClickHandler = null;

    // 드래그를 시작하기 전
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 이벤트를 뿌려준다.
        if (OnBeginDragHandler != null)
            OnBeginDragHandler.Invoke(eventData);
    }

    // 드래그 중
    public void OnDrag(PointerEventData eventData)
    {
        // 이벤트를 뿌려준다.
        if (OnDragHandler != null)
            OnDragHandler.Invoke(eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 이벤트를 뿌려준다.
        if (OnClickHandler != null)
            OnClickHandler.Invoke(eventData);
    }
}