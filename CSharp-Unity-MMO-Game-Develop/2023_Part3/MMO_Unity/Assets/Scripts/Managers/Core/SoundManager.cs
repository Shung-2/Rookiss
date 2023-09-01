using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    // Mp3 Player > AudioSource
    // Mp3 음원 > AudioClip
    // 관객(귀) > AudioListener

    AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.MaxCount];
    
    // Effect 사운드를 저장할 캐싱할 딕셔너리
    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    public void Init()
    {
        GameObject root = GameObject.Find("@Sound");
        if (root == null)
        {
            root = new GameObject { name = "@Sound" };
            // 삭제를 보호한다.
            Object.DontDestroyOnLoad(root);

            // 리플렉션을 이용하여 사운드 이름을 추출한다.
            string[] soundNames = System.Enum.GetNames(typeof(Define.Sound));
            
            // MaxCount를 제외하기 위해 Length - 1을 한다.
            for (int i = 0; i < soundNames.Length - 1; i++)
            {
                 GameObject go = new GameObject { name = soundNames[i] };
                // _audioSources에 AudioSource 컴포넌트를 붙이며 넣어준다.
                _audioSources[i] = go.AddComponent<AudioSource>();
                go.transform.parent = root.transform;
            }

            // Bgm은 loop를 true로 설정하여 반복재생한다.
            _audioSources[(int)Define.Sound.Bgm].loop = true;
        }
    }

    public void Clear()
    {
        // 씬이 변경될 때 마다 캐시를 날려주어 메모리를 절약한다.
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }

        _audioClips.Clear();
    }

    public void Play(string path, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        AudioClip audioClip = GetOrAddAudioClip(path, type);
        Play(audioClip, type, pitch);
    }

    public void Play(AudioClip audioClip, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        if (audioClip == null)
            return;

        if (type == Define.Sound.Bgm)
        {
            // audioSource = Bgm
            AudioSource audioSource = _audioSources[(int)Define.Sound.Bgm];

            // 다른 Bgm이 재생중이라면 정지시킨다.
            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else
        {
            // audioSource = Effect
            AudioSource audioSource = _audioSources[(int)Define.Sound.Effect];
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }
    }

    AudioClip GetOrAddAudioClip(string path, Define.Sound type = Define.Sound.Effect)
    {
        // 패스에 값이 없을 경우 직접 넣어준다.
        if (path.Contains("Sounds/") == false)
            path = $"Sounds/{path}";

        AudioClip audioClip = null;

        if (type == Define.Sound.Bgm)
        {
            // 오디오 클립을 path에서 가져온다.
            audioClip = Managers.Resource.Load<AudioClip>(path);
        }
        else
        {
            // audioClip 캐싱하고 있던 히스토리를 뒤져보고, 
            if (_audioClips.TryGetValue(path, out audioClip) == false)
            {
                // 만약 없었다면 새로 로드해서 캐싱한다.
                audioClip = Managers.Resource.Load<AudioClip>(path);
                _audioClips.Add(path, audioClip);
            }
        }

        if (audioClip == null)
        {
            Debug.Log($"AudioClip Missing! {path}");
        }

        // 기존 패스가 있었으면 이를 사용하고
        return audioClip;
    }
}