using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager
{
    MyPlayer _myplayer;
    Dictionary<int, Player> _players = new Dictionary<int, Player>();

    public static PlayerManager Instance { get; } = new PlayerManager();

    // ���� �� ó���� ���ӿ� ������ ���� ��
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
        // �̵� ����ȭ�� ����� �κ� �� �ϳ�. �̴� �� ���� ������� ������.
        // 1. �����ʿ��� ��� ��Ŷ�� ���� �� �̵��ϴ� ���
        // 2. Ŭ���̾�Ʈ�ʿ��� �̵��� �ϰ� �������� ������ �� ��� �����ϴ� ���

        // ���� ���� ����� 1�� ����� ������.
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

    // ���� �̹� ������ ���¿��� ���� ������ �� ���
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

            // playerId�� ��¥ �ִ°ɱ��?
            if (_players.TryGetValue(packet.playerId, out player))
            {
                // ã������ ������ ���ٰԿ�.
                Object.Destroy(player.gameObject);
                _players.Remove(packet.playerId);
            }
        }
    }
}