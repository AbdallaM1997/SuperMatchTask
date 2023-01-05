using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using DG.Tweening;
public enum ObjectType
{
    obj1, obj2, obj3, obj4, obj5,
}

public class ObjectController : MonoBehaviour
{
    public float FadeDuration = 1f;
    [SerializeField] public ObjectType thisObjecetType;
    [SerializeField] public int Score = 100;
    [SerializeField] private float range = 0.1f;
    [SerializeField] private Color objColorEffect;

    private Color startColor = Color.white;
    private Animator anim;
    private bool inRange;
    private Material objMat;
    private float lastColorChangeTime;
    private bool isFade;
    private void Start()
    {
        anim = GetComponent<Animator>();
        objMat = transform.GetChild(0).GetComponent<MeshRenderer>().material;
    }
    // Update is called once per frame
    void Update()
    {
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, range);

        foreach (Collider colider in colliderArray)
        {
            if (colider.gameObject != this.gameObject)
            {
                if (colider.TryGetComponent<ObjectController>(out ObjectController @object))
                {


                    if (@object.thisObjecetType == thisObjecetType)
                    {
                        inRange = true;
                        isFade = true;

                        //ColorChangeEffect();

                        if (isFade)
                        {
                            Color objectColor = objMat.color;
                            float fadeAmount = objectColor.a - (5 * Time.deltaTime);
                            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                            objMat.color = objectColor;
                            if (objectColor.a <= 0)
                            {
                                isFade = false;
                            }
                        }
     
                        print(@object);
                    }
                }
                else
                {
                    objMat.color = Color.white;
                    inRange = false;
                }

            }
        }

        if (inRange)
        {
            anim.Play("Boomb");
        }
        else
        {
            anim.Play("Idle");
        }
    }

    private void ColorChangeEffect()
    {
        var ratio = (Time.time - lastColorChangeTime) / FadeDuration * 1.5f;
        ratio = Mathf.Clamp01(ratio);
        objMat.color = Color.Lerp(startColor, objColorEffect, ratio * ratio);

        if (ratio == 1f)
        {
            lastColorChangeTime = Time.time;

            var temp = startColor;
            startColor = objColorEffect;
            objColorEffect = temp;
        }
    }
}
