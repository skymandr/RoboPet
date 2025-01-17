﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodBoyLegAnchor : GoodBoyAnchor
{
    static string DEFAULT_LEG_TYPE = "BlockMotor";
    public static string LegTypeSetting(LegPosition legPosition)
    {
        return string.Format("Leg.Type.{0}", legPosition.ToString());
    }

    public static LegController InstantiateLeg(LegPosition legPosition)
    {
        return Instantiate(Resources.Load<LegController>(
            string.Format(
                "Legs/{0}",
                PlayerPrefs.GetString(LegTypeSetting(legPosition), DEFAULT_LEG_TYPE)
            )
        ));
    }

    [SerializeField]
    LegPosition legPosition;

    public override string SettingKey
    {
        get
        {
            return LegTypeSetting(legPosition);
        }
    }
    public override BodyPart BodyPart => BodyPart.Leg;

    float xOffSign
    {
        get
        {
            var left = legPosition == LegPosition.ForwardLeft || legPosition == LegPosition.RearLeft;
            return left ? -1 : 1;
        }
    }

    public LegController SpawnLeg()
    {        
        var leg = InstantiateLeg(legPosition);
        leg.gameObject.SetActive(false);
        leg.transform.rotation = transform.parent.rotation;
        var offset = leg.transform.rotation * new Vector3(leg.anchorOffset.x * xOffSign, leg.anchorOffset.y, leg.anchorOffset.z);
        leg.transform.position = transform.position + offset;
        leg.transform.SetParent(transform.parent.parent, true);
        leg.ConfigureJoint(transform.parent.GetComponent<Rigidbody>(), transform.position);
        leg.gameObject.SetActive(true);
        leg.legPosition = legPosition;
        return leg;
    }
}
