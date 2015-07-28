using UnityEngine;
using System.Collections;

public class PlayerShot : MonoBehaviour
{
	public int id = 0;

	private ShotCharge charge_;
	public ShotCharge charge 
	{
		protected get { return charge_; }
		set { charge_ = value; }
	}

	private bool isDead_ = false;

	protected bool IsOwnEvent(int hwId)
	{
		return Parameters.GetMarkerId(hwId) == id;
	}

	protected bool CanUse(int energy)
	{
		return charge_.CanUse(energy);
	}

	protected bool Use(int energy)
	{
		return charge_.Use(energy);
	}

	protected bool IsDead()
	{
		return isDead_;
	}
	
	void Start()
	{
		SerialHandler.Pressed  += _OnPressed;
		SerialHandler.Pressing += _OnPressing;
		SerialHandler.Released += _OnReleased;
	}

	void OnDestroy()
	{
		charge_.Release(gameObject);
		SerialHandler.Pressed  -= _OnPressed;
		SerialHandler.Pressing -= _OnPressing;
		SerialHandler.Released -= _OnReleased;
	}

	private void _OnPressed(int hwId)
	{
		if (IsOwnEvent(hwId)) {
			charge_.Get(gameObject);
			OnPressed();
		}
	}

	protected virtual void OnPressed()
	{
	}
	
	private void _OnPressing(int hwId)
	{
		if (IsOwnEvent(hwId)) {
			charge_.Get(gameObject);
			OnPressing();
		}
	}

	protected virtual void OnPressing()
	{
	}

	private void _OnReleased(int hwId)
	{
		if (IsOwnEvent(hwId)) {
			OnReleased();
			charge_.Release(gameObject);
		}
	}

	protected virtual void OnReleased()
	{
	}

	void OnDead()
	{
		isDead_ = true;
	}

	void OnRevival()
	{
		isDead_ = false;
	}
}
