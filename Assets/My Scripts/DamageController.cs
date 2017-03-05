using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DamageController : MonoBehaviour {

	// Use this for initialization
	public TextMesh txtFront;
	public TextMesh txtBack;
	public float lifeTime;

	void Start () {
	
	}

	public void CreateDamage(string value)
	{
		DrawText (value);
		JumpRandom ();
		Destroy ();
	}

	public void CreateBonusColor(string value,Color color)
	{
		DrawTextOfColor (value,color);
		JumpRandom ();
		Destroy ();
	}

	void DrawTextOfColor(string value,Color color)
	{
		txtFront.text = value.ToString ();
		txtBack.text = value.ToString ();
		txtFront.color = color;
	}

	void DrawText(string value)
	{
		txtFront.text = value.ToString ();
		txtBack.text = value.ToString ();
	}

	void JumpRandom()
	{
		int randomForce = Random.Range (150, 300);
		Vector3 dir = Quaternion.AngleAxis(85, Vector3.forward) * Vector3.right;
		gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (gameObject.GetComponent<Rigidbody2D> ().velocity.x, 0);
		gameObject.GetComponent<Rigidbody2D> ().AddForce (dir*randomForce);
	}
	void Destroy()
	{
		Destroy (gameObject, lifeTime);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
