using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;
using Holoville.HOTween.Plugins;

/// <summary>
/// Cover Flow Table UI.
/// </summary>
public class HeroTable : MonoBehaviour {
	// for item list
	UISprite[] heros;

	Transform mTrans;
	bool mIsDragging = false;
	Vector3 mPosition, mLocalPosition;
	Vector3 mDragStartPosition;
	Vector3 mDragPosition;
	Vector3 mStartPosition;
	
	public float cellWidth = 160f;
	public float cellHeight = 300f;
	public float downScale = 0.4f;
	public int cellTotal = 6;
	public int seq = 3;
	
	// for displaying item title
	public UILabel titleLabel;

	// put items into array;
	void Start () {
		heros = gameObject.GetComponentsInChildren<UISprite>();
		foreach(UISprite sprite in heros)
			sprite.transform.parent.GetComponent<HeroItem>().downScale = downScale;
		SetPosition(false);
		cellTotal = heros.Length;
		BoxCollider collider = gameObject.GetComponent<BoxCollider>();
		collider.size = new Vector3(cellWidth * cellTotal, cellHeight, 1f);
		collider.center = new Vector3(cellWidth * (cellTotal-1) * 0.5f, 0f, 0f);
		GetComponent<UIGrid>().Reposition();
	}

	void Awake(){
		mTrans = transform;
		mPosition = mTrans.position;
		mLocalPosition = mTrans.localPosition;
	}
	
	// Determine scroll direction
	void SetSequence(bool isRight){
		Vector3 dist = mLocalPosition - mTrans.localPosition;
		float distX = Mathf.Round(dist.x/cellWidth);
		seq = (int)distX;
		if (seq >= cellTotal) seq = cellTotal - 1;
		if (seq <= 0) seq = 0;
	}

	// Set current table position;
	void SetPosition(bool isMotion){
		Vector3 pos = mLocalPosition;
		pos -= new Vector3(seq * cellWidth, 0f, 0f);
		if (isMotion) {
			TweenParms parms = new TweenParms();
			parms.Prop("localPosition", pos);
			parms.Ease(EaseType.Linear);
			HOTween.To(mTrans, 0.1f, parms);
			HOTween.Play();
		} else {
			mTrans.localPosition = pos;
		}
		if (titleLabel!=null) titleLabel.text = heros[seq].spriteName;
	}

	// drop event
	void Drop () {
		Vector3 dist = mDragPosition - mDragStartPosition;
		if (dist.x>0f) SetSequence(true);
		else SetSequence(false);
		SetPosition(true);
	}

	// drag event
	void OnDrag (Vector2 delta) {
		Ray ray = UICamera.currentCamera.ScreenPointToRay(UICamera.lastTouchPosition);
		float dist = 0f;
		// determine drag state and current drag position
		Vector3 currentPos = ray.GetPoint(dist);
		if (UICamera.currentTouchID == -1 || UICamera.currentTouchID == 0) {
			if (!mIsDragging) {
				mIsDragging = true;
				mDragPosition = currentPos;
			} else {
				Vector3 pos = mStartPosition - (mDragStartPosition - currentPos);
				Vector3 cpos = new Vector3(pos.x, mTrans.position.y, mTrans.position.z);
				mTrans.position = cpos;
			}
		}
	}

	// press event
	void OnPress (bool isPressed) {
		mIsDragging = false;
		Collider col = collider;
		// determine press start position
		if (col != null) {
			Ray ray = UICamera.currentCamera.ScreenPointToRay(UICamera.lastTouchPosition);
			float dist = 0f;
			mDragStartPosition = ray.GetPoint(dist);
			mStartPosition = mTrans.position;
			col.enabled = !isPressed;
		}
		if (!isPressed) Drop();
	}
}
