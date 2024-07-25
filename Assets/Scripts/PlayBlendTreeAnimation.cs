using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

//播放混合树动画
[RequireComponent(typeof(Animator))]
public class PlayBlendTreeAnimation : MonoBehaviour
{
    public AnimationClip walkClip;
    public AnimationClip runClip;
    [Range(0, 1)]
    public float speed;

    PlayableGraph graph;
    AnimationMixerPlayable mixerPlayable;

    void Start()
    {
        graph = PlayableGraph.Create("PlayBlendTree"); //创建播放图像（用于管理Playable节点和Playable输出）
        var outputPlayable = AnimationPlayableOutput.Create(graph, "PlayBlendTreeOutput", GetComponent<Animator>()); //创建动画播放输出

        mixerPlayable = AnimationMixerPlayable.Create(graph, 2); //创建动画混合节点
        var walkClipPlayable = AnimationClipPlayable.Create(graph, walkClip); //创建行走动画片段节点
        var runClipPlayable = AnimationClipPlayable.Create(graph, runClip); //创建跑步动画片段节点

        //如果混合动画播放时间不一致，需要通过设置速度使其播放时间一致
        walkClipPlayable.SetSpeed(1f); //设置行走动画片段速度
        runClipPlayable.SetSpeed(0.5f); //设置跑步动画片段速度

        //将行走和跑步动画片段连接到混合动画节点
        graph.Connect(walkClipPlayable, 0, mixerPlayable, 0);
        graph.Connect(runClipPlayable, 0, mixerPlayable, 1);

        outputPlayable.SetSourcePlayable(mixerPlayable); //将动画片段节点添加到播放输出

        graph.Play(); //播放图像
    }

    void Update()
    {
        //设置混合动画权重
        mixerPlayable.SetInputWeight(0, 1-speed);
        mixerPlayable.SetInputWeight(1, speed);

        //设置混合动画总播放速度
        float totalSpeed = (1f - speed)*1f + speed * 0.5f;
        mixerPlayable.SetSpeed(1/totalSpeed);

        
    }

    void OnDisable()
    {
        graph.Destroy();
    }
}
