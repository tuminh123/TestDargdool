using System;
using System.Collections;
using System.Collections.Generic;
//using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class Piston : MonoBehaviour
{
	[Header("Setting")]
	[SerializeField] private float _pushSpeed = 20;
	[SerializeField] private float _returnSpeed = 7.5f;
	[SerializeField] private float _delayNextPush = .5f;
	
	[Header("Reference")]
	[SerializeField] private Rigidbody2D _push;
	[SerializeField] private Transform _startPush;
	[SerializeField] private Transform _targetPush;

	private bool _isPushing;
	private bool _isBusy = false;
	private float _currentSpeed;
	private float _delayCounter = 10f;
	private Transform _currentTarget;
	
	private void Start()
	{
		_push.transform.position = _startPush.position;
	}
	private void OnTriggerEnter2D(Collider2D other)
	{
		Push();
		HandelPhysic(other);
    }

	private void OnTriggerStay2D(Collider2D other)
    {
        Push();
        HandelPhysic(other);
    }

    private void HandelPhysic(Collider2D other)
    {
        Vector2 direction = (-other.transform.position + transform.position).normalized;
        if (other.TryGetComponent<CharacterParent>(out var par))
        {
			Rigidbody2D rb = par.GetComponent<Rigidbody2D>();
			if (rb == null) return;
			rb.AddForce(direction * _pushSpeed , ForceMode2D.Impulse);
			//RandomFly(rb);
		}
    }

    IEnumerator RandomFly(Rigidbody2D rb)
    {
       Vector2 randomDir = Random.insideUnitCircle.normalized;
            float randomForce = Random.Range(300f, 1000f);
            rb.AddForce(randomDir * randomForce, ForceMode2D.Impulse);

            yield return new WaitForSeconds(Random.Range(1f, 3f));
    }

    //[Button]
    private void Push()
    {
	    if (!_isBusy && CanPush())
	    {
		    _isBusy = true;
		    _isPushing = true;
		    _currentTarget = _targetPush;
		    _currentSpeed = _pushSpeed;
	    }
	}

	private void FixedUpdate()
	{
		if (!CanPush())
		{
			_delayCounter += Time.deltaTime;
		}
		if (_isBusy)
		{
			_push.MovePosition(Vector2.MoveTowards(_push.transform.position, _currentTarget.position, _currentSpeed * Time.deltaTime));
			if (_isPushing)
			{
				if ((Vector2)_push.transform.position == (Vector2)_targetPush.position)
				{
					_currentTarget = _startPush;
					_isPushing = false;
					_currentSpeed = _returnSpeed;
				}
			}
			else
			{
				if ((Vector2)_push.transform.position == (Vector2)_startPush.position)
				{
					_currentTarget = _targetPush;
					_isPushing = false;
					_isBusy = false;
					_currentSpeed = _pushSpeed;
					_delayCounter = 0;
				}
			}
		}
	}

	bool CanPush() => _delayCounter >= _delayNextPush;
}
