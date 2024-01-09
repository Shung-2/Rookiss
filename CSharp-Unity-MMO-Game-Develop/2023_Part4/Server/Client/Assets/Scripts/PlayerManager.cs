using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager
{
    MyPlayer _myplayer;
    Dictionary<int, Player> _players = new Dictionary<int, Player>();

    public static PlayerManager Instance { get; } = new PlayerManager();

    // 내가 맨 처음에 게임에 접속을 했을 때
    public void Add(S_PlayerList packet)
    {
        Object obj = Resources.Load("Player");

        foreach (S_PlayerList.Player p in packet.players)
        {
            GameObject go = Object.Instantiate(obj) as GameObject;

            if (p.isSelf)
            {
                MyPlayer myPlayer = go.AddComponent<MyPlayer>();
                myPlayer.PlayerId = p.playerId;
                myPlayer.transform.position = new Vector3(p.posX, p.posY, p.posZ);
                _myplayer = myPlayer;
            }
            else
            {
                Player player = go.AddComponent<Player>();
                player.PlayerId = p.playerId;
                player.transform.position = new Vector3(p.posX, p.posY, p.posZ);
                _players.Add(p.playerId, player);
            }
        }
    }

    public void Move(S_BroadcastMove packet)
    {
        // 이동 동기화는 어려운 부분 중 하나. 이는 두 가지 방법으로 나뉜다.
        // 1. 서버쪽에서 허락 패킷이 왔을 때 이동하는 방법
        // 2. 클라이언트쪽에서 이동을 하고 서버에게 응답이 올 경우 보정하는 방법

        // 현재 구현 방식은 1번 방식을 따른다.
        if (_myplayer.PlayerId == packet.playerId)
        {
            _myplayer.transform.position = new Vector3(packet.posX, packet.posY, packet.posZ);
        }
        else
        {
            Player player = null;
            if (_players.TryGetValue(packet.playerId, out player))
            {
                player.transform.position = new Vector3(packet.posX, packet.posY, packet.posZ);
            }
        }
    }

    // 내가 이미 접속한 상태에서 새로 접속을 할 경우
    public void EnterGame(S_BroadcastEnterGame packet)
    {
        if (_myplayer.PlayerId == packet.playerId)
        {
            return;
        }

        Object obj = Resources.Load("Player");
        GameObject go = Object.Instantiate(obj) as GameObject;

        Player player = go.AddComponent<Player>();
        player.transform.position = new Vector3(packet.posX, packet.posY, packet.posZ);
        _players.Add(packet.playerId, player);
    }

    public void LeaveGame(S_BroadcastLeaveGame packet)
    {
        if (_myplayer.PlayerId == packet.playerId)
        {
            GameObject.Destroy(_myplayer.gameObject);
            _myplayer = null;
        }
        else
        {
            Player player = null;

            // playerId가 진짜 있는걸까요?
            if (_players.TryGetValue(packet.playerId, out player))
            {
                // 찾았으면 삭제를 해줄게요.
                Object.Destroy(player.gameObject);
                _players.Remove(packet.playerId);
            }
        }
    }
}