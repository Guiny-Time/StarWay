using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

    public class TimelineUnit
    {

        public string name;
        public PlayableDirector director;
        public PlayableAsset asset;
        public Dictionary<string, PlayableBinding> bindings;
        public Dictionary<string, Dictionary<string, PlayableAsset>> clips;
        public Dictionary<string, PlayableAsset> playables;

        public void Init(string path, PlayableDirector director)
        {
            this.name = path;               // director标签
            this.director = director;       // 获取当前组件
            playables = new Dictionary<string, PlayableAsset>();
            InitPlayables(path);            // 获得所有的动画
        }

        public void Switch(string assetName)
        {

            if (!playables.TryGetValue(assetName, out this.asset))
            {
                Debug.LogError("No Asset exist!");
                return;
            } 
            director.playableAsset = asset; // 导演Manager

            bindings = new Dictionary<string, PlayableBinding>(); // PlayableAsset下的所有binding
            clips = new Dictionary<string, Dictionary<string, PlayableAsset>>(); // binding里的所有clip
            foreach (var o in asset.outputs) // 每一个binding，其实就是trackasset和需要动画的模型之间的链接关系
            {
                var trackname = o.streamName;
                bindings.Add(trackname, o);  // 每一个binding的名字和binding绑定

                var track = o.sourceObject as TrackAsset; // 每个binding下的对象为track
                if (track == null) continue;
                var clipList = track.GetClips(); // 获得每一个track下的所有动画片段
                foreach (var c in clipList) // 存入clips
                {
                    if (!clips.ContainsKey(trackname))
                    {
                        clips[trackname] = new Dictionary<string, PlayableAsset>();
                    }
                    var name2clips = clips[trackname];
                    if (!name2clips.ContainsKey(c.displayName))
                    {
                        name2clips.Add(c.displayName, c.asset as PlayableAsset);
                    }
                }
            }
        }

            // 清空数据
            public void Remove()
            {
                bindings.Clear();
                clips.Clear();
            }

            // 动画和模型进行绑定
            public void SetBinding(string trackName, Object o)
            {
                director.SetGenericBinding(bindings[trackName].sourceObject, o);
            }

            // 获得动画轨道
            public T GetTrack<T>(string trackName) where T : TrackAsset
            {
                return bindings[trackName].sourceObject as T;
            }

            // 获得动画片段
            public T GetClip<T>(string trackName, string clipName) where T : PlayableAsset
            {
                if (clips.ContainsKey(trackName))
                {
                    var track = clips[trackName];
                    if (track.ContainsKey(clipName))
                    {
                        return track[clipName] as T;
                    }
                    else
                    {
                        Debug.LogError("GetClip Error, Track does not contain clip, trackName: " + trackName + ", clipName: " + clipName);
                    }
                }
                else
                {
                    Debug.LogError("GetClip Error, Track does not contain clip, trackName: " + trackName + ", clipName: " + clipName);
                }
                return null;
            }

            // 读入所有动画资源
            private void InitPlayables(string path)
            {
                PlayableAsset[] o = Resources.LoadAll<PlayableAsset>(path + "/");
                foreach (var asset in o)
                {
                    playables.Add(asset.ToString().Replace(" (UnityEngine.Timeline.TimelineAsset)", ""), asset);
                    Debug.Log(asset.ToString());
                }
            }

            // 播放动画
            public void Play()
            {
                director.Play();
            }
    }
