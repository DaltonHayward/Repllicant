using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : Collectible
{
    public GameObject dropWhenStoned;//ʯ��ʱ������
    [HideInInspector]public GameObject dropItemStart;//�����ص�����
    private void Start()
    {
        dropItemStart = sureToDrop;
    }
    public void Stoned()
    {
        sureToDrop = dropWhenStoned;
    }
    public void UnStoned()//���ʯ��
    {
        sureToDrop = dropItemStart;
    }

}
