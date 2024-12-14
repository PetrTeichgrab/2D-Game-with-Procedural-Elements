using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CastSpell : MonoBehaviour
{
    public Transform castPoint;

    public float spellSpeed = 2.5f;

    public Rigidbody2D rb;

    Vector2 mousePostion;

    public Camera cam;

    public GameObject hitEffect;

    public GameObject spellObject;

    void Update()
    {
        mousePostion = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDirection = mousePostion - (Vector2)rb.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
        castPoint.transform.rotation = Quaternion.Euler(0, 0, angle);
        if (Input.GetButtonDown("Fire1"))
        {
            Cast();
        }
    }

    public void Cast()
    {
        var spellObject = Instantiate(this.spellObject, castPoint.position, castPoint.rotation);
        Rigidbody2D spellRb = spellObject.GetComponent<Rigidbody2D>();
        spellRb.AddForce(castPoint.up * spellSpeed, ForceMode2D.Impulse);
        Destroy(spellObject, 0.7f);
    }

}
