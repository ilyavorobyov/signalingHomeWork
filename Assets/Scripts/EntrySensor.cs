using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EntrySensor : MonoBehaviour
{
    private UnityEvent _thiefCame = new UnityEvent();
    private UnityEvent _thiefGone = new UnityEvent();

    public event UnityAction ThiefCame
    {
        add => _thiefCame.AddListener(value);
        remove => _thiefCame.RemoveListener(value);
    }

    public event UnityAction ThiefGone
    {
        add => _thiefGone.AddListener(value);
        remove => _thiefGone.RemoveListener(value);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Thief>(out Thief thief))
        {
            _thiefCame.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Thief>(out Thief thief))
        {
            _thiefGone.Invoke();
        }
    }
}