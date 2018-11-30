using UnityEngine;
using System.Collections;

/*
** Author      : Runing
** Time        : 11/30/2018 10:40:52 PM
** description : 
*/

public class FurnitureModel : ModelCategory
{
    protected override void Start()
    {
        base.Start();
        selfCategory = ECategory.furniture;
        recognitionCategory = ECategory.furniture;
    }

    public override void AfterRay(RaycastHit hitInfo)
    {
        //��ǰѡ�е����壬ֻ�ܷ�����һ������Ĵ�ֱλ���ϡ�Ҳ����˵�����ܷ������Ĳ��档
        //���磬�����ܷ����������棬���ǲ��ܷ���������Χ���ĸ����ϡ�
        if (hitInfo.normal.x > -0.1f && hitInfo.normal.x < 0.1f &&
           hitInfo.normal.y > 0.9f && hitInfo.normal.y < 1.1f &&
           hitInfo.normal.z > -0.1f && hitInfo.normal.z < 0.1f)
        {
            transform.position = hitInfo.point;
            transform.rotation = Quaternion.LookRotation(hitInfo.normal);
        }
    }

    public override bool CanDelete()
    {
        return true;
    }
}
