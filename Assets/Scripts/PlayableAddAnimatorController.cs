using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

//播放Playable+动画状态机动画
[RequireComponent(typeof(Animator))]
public class PlayableAddAnimatorController : MonoBehaviour
{
    public AnimationClip walkClip;
    public RuntimeAnimatorController runAnimatorController;
    [Range(0, 1)]
    public float speed;
    PlayableGraph graph;
    AnimationMixerPlayable mixerPlayable;
    
    void Start()
    {
        graph = PlayableGraph.Create("PlayBlendTree");
        var outputPlayable = AnimationPlayableOutput.Create(graph, "PlayBlendTreeOutput", GetComponent<Animator>());

        mixerPlayable = AnimationMixerPlayable.Create(graph, 2); //创建动画混合节点
        var walkClipPlayable = AnimationClipPlayable.Create(graph, walkClip); //创建行走动画片段节点
        var runAnimatorControllerPlayable = AnimatorControllerPlayable.Create(graph, runAnimatorController); //创建跑步的动画状态机节点

        walkClipPlayable.SetSpeed(1f);
        runAnimatorControllerPlayable.SetSpeed(0.5f);

        //将行走动画片段节点和跑步动画状态机节点连接到混合动画节点
        graph.Connect(walkClipPlayable, 0, mixerPlayable, 0);
        graph.Connect(runAnimatorControllerPlayable, 0, mixerPlayable, 1);

        outputPlayable.SetSourcePlayable(mixerPlayable); //将动画片段节点添加到播放输出
        graph.Play();
    }

    void Update()
    {
        mixerPlayable.SetInputWeight(0, 1-speed);
        mixerPlayable.SetInputWeight(1, speed);

        float totalSpeed = (1f - speed)*1f + speed * 0.5f;
        mixerPlayable.SetSpeed(1/totalSpeed);
    }

    void OnDisable()
    {
        graph.Destroy();
    }
}
