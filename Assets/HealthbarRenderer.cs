using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class HealthbarRenderer : MonoBehaviour {

    public GameObject healthBar;
    public float maxWidth;
    Unit unit;

	void Start() {
        unit = gameObject.GetComponent<Unit>();
        unit.statsChanged.AddListener(UpdateBar);
	}
	
	void UpdateBar() {
        float newWidth = (unit.currentHealth / unit.maxHealth) * maxWidth;
        transform.localScale = new Vector3(newWidth, 1, 1);
        transform.position = new Vector3(-(newWidth / 4), transform.position.y, transform.position.z);
	}
}