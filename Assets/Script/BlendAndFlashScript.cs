using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendAndFlashScript : MonoBehaviour
{

    public Transform sharpGameObject_;
    public Texture greenTexture_;
    public Texture blueTexture_;
    public Texture redTexture_;
    public Texture whiteTexture_1_;
    public Texture whiteTexture_2_;
    public Texture whiteTexture_3_;
    public Texture grayTexture_;
    private Texture appearedTexture_;
    private SkinnedMeshRenderer SkinnedMeshRenderer_;

    private float distanceOfGreen_;
    private float distanceOfBlue_;
    private float distanceOfRed_;
    private float distance_;
    private float tempDistance_ = -10;
    private float waitTime_ = 1.0f;
    private int indexOfChildObject;

    //Set distance of sharp object when the plane should warn
    private float alertDistance_ = 30;

    private bool first_LessOf50Blend_ = true;
    private bool first_50Blend_ = false;
    private bool first_100Blend_ = false;
    private bool second_100Blend_ = false;
    private bool second_50Blend_ = false;
    private bool third_50Blend_ = false;

    private bool firstEnter_ = true;
    private bool secondEnter_ = false;
    private bool thirdEnter_ = false;


    // Start is called before the first frame update
    void Start()
    {
        SkinnedMeshRenderer_ = GetComponent<SkinnedMeshRenderer>();
        appearedTexture_ = greenTexture_;
        indexOfChildObject = 0;
    }


    // Update is called once per frame
    void Update()
    {
        //This function adjust distance base on diffrent sections 
        AdjustDistance();

        //This function changes color base on distance 
        AdjustColor();

        //This function makes Blend  
        BlendAndChangeColor();
    }


    private void AdjustDistance()
    {

        if (first_LessOf50Blend_ || first_50Blend_)
        {
            distanceOfGreen_ = Vector3.Distance(transform.GetChild(0).position, sharpGameObject_.position);
            distance_ = distanceOfGreen_;
            indexOfChildObject = 0;
        }

        if (first_100Blend_ || second_50Blend_)
        {
            distanceOfBlue_ = Vector3.Distance(transform.GetChild(1).position, sharpGameObject_.position);
            distance_ = distanceOfBlue_;
            indexOfChildObject = 1;
        }

        if (second_100Blend_ || third_50Blend_)
        {
            distanceOfRed_ = Vector3.Distance(transform.GetChild(2).position, sharpGameObject_.position);
            distance_ = distanceOfRed_;
            indexOfChildObject = 2;
        }
    }

    private void AdjustColor()
    {
        //Adjust the flashing sections
        if (first_LessOf50Blend_ || first_100Blend_ || second_100Blend_)
        {
            if (SkinnedMeshRenderer_.material.mainTexture == appearedTexture_)
            {
                waitTime_ -= Time.deltaTime;
                if (waitTime_ < 0)
                {
                    SkinnedMeshRenderer_.material.mainTexture = grayTexture_;
                    waitTime_ = 1.0f;
                }
            }


            else if (SkinnedMeshRenderer_.material.mainTexture == grayTexture_)
            {
                waitTime_ -= Time.deltaTime;
                if (waitTime_ < 0)
                {
                    SkinnedMeshRenderer_.material.mainTexture = appearedTexture_;
                    waitTime_ = 1.0f;
                }
            }
        }

        //Adjust the white sections
        else if (first_50Blend_ || second_50Blend_ || third_50Blend_)
        {
            SkinnedMeshRenderer_.material.mainTexture = appearedTexture_;
        }
    }


    private void BlendAndChangeColor()
    {

        if (Mathf.RoundToInt(distance_) <= alertDistance_)
        {
            if (Mathf.RoundToInt(distance_) < Mathf.RoundToInt(tempDistance_) || firstEnter_ || secondEnter_ || thirdEnter_)
            {

                firstEnter_ = false;
                secondEnter_ = false;
                thirdEnter_ = false;
                tempDistance_ = Mathf.RoundToInt(distance_);

                if (Mathf.RoundToInt(distance_) == Mathf.RoundToInt(alertDistance_ / 2))
                {
                    SkinnedMeshRenderer_.SetBlendShapeWeight(indexOfChildObject, 50);

                    if (first_LessOf50Blend_)
                    {
                        first_50Blend_ = true;
                        first_LessOf50Blend_ = false;
                        appearedTexture_ = whiteTexture_1_;
                    }

                    if (first_100Blend_)
                    {
                        first_100Blend_ = false;
                        second_50Blend_ = true;
                        appearedTexture_ = whiteTexture_2_;
                    }

                    if (second_100Blend_)
                    {
                        second_100Blend_ = false;
                        third_50Blend_ = true;
                        appearedTexture_ = whiteTexture_3_;
                    }
                }


                else if (Mathf.RoundToInt(distance_) <= 1)
                {
                    SkinnedMeshRenderer_.SetBlendShapeWeight(indexOfChildObject, 100);

                    if (first_50Blend_)
                    {
                        first_50Blend_ = false;
                        first_100Blend_ = true;
                        secondEnter_ = true;
                        appearedTexture_ = blueTexture_;
                        SkinnedMeshRenderer_.material.mainTexture = appearedTexture_;
                    }

                    if (second_50Blend_)
                    {
                        second_50Blend_ = false;
                        second_100Blend_ = true;
                        thirdEnter_ = true;
                        appearedTexture_ = redTexture_;
                        SkinnedMeshRenderer_.material.mainTexture = appearedTexture_;
                    }

                    if (third_50Blend_)
                    {
                        third_50Blend_ = false;
                        appearedTexture_ = grayTexture_;
                        SkinnedMeshRenderer_.material.mainTexture = appearedTexture_;
                    }
                }

                else
                {
                    SkinnedMeshRenderer_.SetBlendShapeWeight(indexOfChildObject, 100 - ((100 * Mathf.RoundToInt(distance_)) / alertDistance_));
                }
            }
        }
    }
}