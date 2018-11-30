﻿using UnityEngine;
using System.Collections;

/* Author:       Running
** Time:         18.11.16
** Describtion:  
*/

public class ModelCategory : MonoBehaviour
{
    public enum ECategory
    {
        none = 1 << 0,
        floor = 1 << 1,      //地板
        wall = 1 << 2,       //墙壁
        hang = 1 << 3,       //挂饰 (例如壁画之类的, 能挂在墙上的)
        furniture = 1 << 4,  //家具 (例如椅子，桌子,电视，床,水杯，台灯)
        ceiling = 1 << 5,    //天花板
        pendant = 1 << 6     //下锤物体
    }

    //[Title("我是哪种类型")]
    public ECategory selfCategory;

    //[Title("能被哪种类型  贴在自身上")]
    public ECategory recognitionCategory;

    /// <summary>
    /// It is own child tool node
    /// </summary>
    public Transform toolNode;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {

    }

    public static ModelCategory AttachToModel(Transform model)
    {
        ModelCategory modelCategory = null;

        //通过名字前缀来判断当前的模型是什么类型的。
        if (model.name.Contains("floor_") && !model.GetComponent<FloorModel>())
        {
            modelCategory = model.gameObject.AddComponent<FloorModel>();
        }
        else if (model.name.Contains("wall_") && !model.GetComponent<WallModel>())
        {
            modelCategory = model.gameObject.AddComponent<WallModel>();
        }
        else if (model.name.Contains("hang_") && !model.GetComponent<HangModel>())
        {
            modelCategory = model.gameObject.AddComponent<HangModel>();
        }
        else if (model.name.Contains("furniture_") && !model.GetComponent<FurnitureModel>())
        {
            modelCategory = model.gameObject.AddComponent<FurnitureModel>();
        }
        else if (model.name.Contains("ceiling_") && !model.GetComponent<CeilingModel>())
        {
            modelCategory = model.gameObject.AddComponent<CeilingModel>();
        }
        else if (model.name.Contains("chandelier_") && !model.GetComponent<PendantModel>())
        {
            modelCategory = model.gameObject.AddComponent<PendantModel>();
        }
        else
        {
            Debug.LogError("ModelCategory.AttachToModel(): no modelCategory ! ! !");
        }
        
        return modelCategory;
    }

    /// <summary>
    /// 在场景中，是否能被移除。
    /// </summary>
    /// <returns></returns>
    public virtual bool CanDelete()
    {
        return false;
    }

    public virtual void AfterRay(RaycastHit hitInfo)
    {
        transform.position = hitInfo.point;
        transform.rotation = Quaternion.LookRotation(hitInfo.normal);
    }
}