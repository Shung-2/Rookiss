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

    // �巡�׸� �����ϱ� ��
    public void OnBeginDrag(PointerEventData eventData)
    {
        // �̺�Ʈ�� �ѷ��ش�.
        if (OnBeginDragHandler != null)
            OnBeginDragHandler.Invoke(eventData);
    }

    // �巡�� ��
    public void OnDrag(PointerEventData eventData)
    {
        // �̺�Ʈ�� �ѷ��ش�.
        if (OnDragHandler != null)
            OnDragHandler.Invoke(eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // �̺�Ʈ�� �ѷ��ش�.
        if (OnClickHandler != null)
            OnClickHandler.Invoke(eventData);
    }
}