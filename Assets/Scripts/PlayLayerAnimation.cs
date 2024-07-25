using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

//播放图层混合动画
[RequireComponent(typeof(Animator))]
public class PlayLayerAnimation : MonoBehaviour
{
    public AnimationClip handClip;
    public AnimationClip runClip;
    public AvatarMask handAvatarMask;

    void Start()
    {
        PlayableGraph graph= PlayableGraph.Create("ChanPlayableGraph");
        var animationOutputPlayable = AnimationPlayableOutput.Create(graph, "AnimationOutput", GetComponent<Animator>());
        var layerMixerPlayable = AnimationLayerMixerPlayable.Create(graph, 2); //创建图层混合节点
        var runClipPlayable = AnimationClipPlayable.Create(graph, runClip); 
        var handClipPlayable = AnimationClipPlayable.Create(graph, handClip);
        graph.Connect(runClipPlayable, 0, layerMixerPlayable, 0);//第一层Layer
        graph.Connect(handClipPlayable, 0, layerMixerPlayable, 1);//第二层Layer
        animationOutputPlayable.SetSourcePlayable(layerMixerPlayable);
        layerMixerPlayable.SetLayerMaskFromAvatarMask(1, handAvatarMask); //第二层设置人形化身遮罩
        layerMixerPlayable.SetInputWeight(0, 1); 
        layerMixerPlayable.SetInputWeight(1, 1f);
        graph.Play();
    }

    void Update()
    {
        
    }
}
