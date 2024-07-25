using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

//播放单个动画
[RequireComponent(typeof(Animator))]
public class PlaySingleAnimation : MonoBehaviour
{
    public AnimationClip clip;
    PlayableGraph graph;
    void Start()
    {
        graph = PlayableGraph.Create("SPlayableGraph"); //创建播放图像（用于管理Playable节点和Playable输出）
        graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime); //设置更新模式
        var animationOutputPlayable = AnimationPlayableOutput.Create(graph, "SAnimationOutput", GetComponent<Animator>()); //创建动画播放输出
        var sClipplayable = AnimationClipPlayable.Create(graph, clip); //创建动画片段节点
        animationOutputPlayable.SetSourcePlayable(sClipplayable); //将动画片段节点添加到播放输出
        graph.Play(); //播放图像
    }

    void OnDisable()
    {
        graph.Destroy();
    }
}
