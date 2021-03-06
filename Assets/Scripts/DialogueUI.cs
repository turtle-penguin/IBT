﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    private static Queue<string> nextTokens = new Queue<string>();
    private static string prevText = "";
    private static float prevTime;
    private static Text textBox;

    public const float TOKEN_DUR = 0.5f;
    private const float END_PAD = 1f;
    
    void Start()
    {
        textBox = GetComponent<Text>();

        prevTime = float.MinValue;
    }
    
    void Update()
    {
        if (nextTokens.Count > 0)
        {
            if (Time.time - prevTime > TOKEN_DUR)
            {
                var currToken = nextTokens.Dequeue();
                textBox.text += " " + currToken;
                prevTime = Time.time;
            }
        }
    }

    public static bool Available()
    {
        return nextTokens.Count == 0 && Time.time - prevTime > END_PAD;
    }

    public static void SetText(string text)
    {
        if (!Available())
            return;

        prevText = text;
        var agentMsgTokens = text.Split(':');
        textBox.text = "<b>" + agentMsgTokens[0] + ":</b> ";
        foreach (var token in agentMsgTokens[1].Trim().Split(' '))
        {
            nextTokens.Enqueue(token);
        }
    }

    public static bool Finished(string text)
    {
        return prevText == text;
    }
}
