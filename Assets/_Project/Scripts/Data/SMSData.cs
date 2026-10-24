using System;
using System.Collections.Generic;
using UnityEngine;

public enum SMSSender
{
    Nem,
    Nam,
    KiEn,
    Police113
}

[Serializable]
public struct SMSMessage
{
    public SMSSender sender;

    [TextArea(2, 4)]
    public string content;

    public float delay;

    public string GetText => content;

}

[CreateAssetMenu(fileName = "SMS_Conversation")]
public class SMSData : ScriptableObject
{
    [SerializeField] private SMSSender contactOwner = SMSSender.Nem;
    public SMSSender ContactOwner => contactOwner;

    [SerializeField] private List<SMSMessage> messages = new List<SMSMessage>();
    public List<SMSMessage> Messages => messages;
}
