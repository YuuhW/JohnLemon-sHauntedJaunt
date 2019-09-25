using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//用于重启游戏

public class GameEnding : MonoBehaviour
{
    public float fadeDuration=1f;//淡出时间
    public float displayImageDuration=1f;//退出持续时间变量
    public GameObject player;
    public CanvasGroup exitBackgroundImageCanvasGroup;

    //Observer类中结束关卡的方法，在Update中用else if实现，在EndLevel方法中引入参数，判断是哪种退出方式
    public CanvasGroup caughtBackgroundImageCanvasGroup;//为新图像创建另一个CanvasGroup，如果JohnLemon被捕获，将显示该图像

    //逃与被抓时声音
    public AudioSource exitAudio;
    public AudioSource caughtAudio;

    bool m_IsPlayerCaught;//检查JohnLemon是否已被捕获

    bool m_IsPlayerAtExit;
    float m_Timer;//计时器，以确保游戏在淡入淡出完成之前不会结束
    bool m_HasAudioPlayed;//检测声音是否已经播放，确保只播放一次

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)//如果有Collider的GameObject(进入触发器的那个)等于对JohnLemon的GameObject的引用，则执行代码块中的操作。
        {
            m_IsPlayerAtExit = true;
        }
    }

    public void CaughtPlayer()
    {
        m_IsPlayerCaught = true;
    }

    void Update()
    {
        if (m_IsPlayerAtExit)
        {
            EndLevel(exitBackgroundImageCanvasGroup,false,exitAudio);
        }
        else if (m_IsPlayerCaught)
        {
            EndLevel(caughtBackgroundImageCanvasGroup,true,caughtAudio);
        }
    }

    void EndLevel(CanvasGroup imageCanvasGroup,bool doRestart,AudioSource audioSource)
    {
        if (!m_HasAudioPlayed)
        {
            audioSource.Play();
            m_HasAudioPlayed = true;
        }
        m_Timer += Time.deltaTime;//将计时器设置为等于自身加上deltaTime（deltaTime：获取自上一帧以来经过的时间）
        imageCanvasGroup.alpha = m_Timer / fadeDuration;//设置画布组的Alpha,当计时器为0时，Alpha值应为0.将计时器除以持续时间
        if (m_Timer>fadeDuration+displayImageDuration)
        {
            if (doRestart)
            {
                SceneManager.LoadScene(0);//只有一个场景
            }
            else
            {
                Application.Quit(); //退出应用
            }
        }
    }
}
