using UnityEngine;
using System.Collections;

/// <summary>
/// Cover Flow Table Item.
/// </summary>
public class HeroItem : MonoBehaviour {
	Transform mTrans, mParent;
	Vector3 scale;
	float cellWidth;
	public float downScale = 0.4f;
	HeroTable hTable;
	
	// Init Default Value
	void Start () {
		mTrans = transform;
		scale = mTrans.localScale;
		mParent = mTrans.parent;
		hTable = mParent.GetComponent<HeroTable>();
		cellWidth = hTable.cellWidth;
		downScale = hTable.downScale;
	}
	
	// Adjust Item Size and Postion
	void Update () {
		Vector3 pos = mTrans.localPosition + mParent.localPosition;
		float dist = Mathf.Clamp(Mathf.Abs(pos.x), 0f, cellWidth);
		mTrans.localScale = ((cellWidth - dist*downScale) / cellWidth) * scale;
	}
}
