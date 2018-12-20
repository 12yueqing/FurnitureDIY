using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;
//using zSpace.Core;

/*
** Author      : Runing
** Time        : 11/26/2018 10:21:16 PM
** description : 
*/

public class RayEvent
{
    //------------------------��� Begin-----------------------------------------
    /// <summary>
    /// ��갴�»ص��¼�(���)
    /// </summary>
    public event Action<Vector3, RaycastHit> mouseLeftDownEvnet;

    /// <summary>
    /// ��껬���ص��¼�(���)
    /// </summary>
    public event Action<Vector3, RaycastHit[]> mouseLeftDragEvnet;

    /// <summary>
    /// ���̧��ص��¼�(���)
    /// </summary>
    public event Action mouseLeftUpEvnet;

    /// <summary>
    /// ��һ��������������
    /// </summary>
    public Transform previousClickObjectOfLeftButton;

    /// <summary>
    /// ���������(���)���������̧��Ҳ��һֱ��¼����������һ�ε����(�������µ���һ�ε��£�Ϊһ����������)
    /// </summary>
    public Transform clickObjectOfLeftButton;

    /// <summary>
    /// ������(���)����¼����������壬���ſ�����գ����ᱣ����(�������µ��ſ���Ϊһ����������)
    /// </summary>
    public Transform rawObjectOfLeftButton;

    /// <summary>
    /// �������Ƿ���(�������ק�����е�����)
    /// </summary>
    public bool isMouseLeftPress = false;

    //------------------------��� End-----------------------------------------

    //------------------------�Ҽ� Begin-----------------------------------------

    /// <summary>
    /// ��갴�»ص��¼�(�Ҽ�)
    /// </summary>
    public event Action<Vector3, RaycastHit> mouseRightDownEvnet;

    /// <summary>
    /// ��껬���ص��¼�(�Ҽ�)
    /// </summary>
    public event Action<Vector3, RaycastHit[]> mouseRightDragEvnet;

    /// <summary>
    /// ���̧��ص��¼�(�Ҽ�)
    /// </summary>
    public event Action mouseRightUpEvnet;

    /// <summary>
    /// ��һ���Ҽ����������
    /// </summary>
    public Transform previousClickObjectOfRightButton;

    /// <summary>
    /// ���������(�Ҽ�)���������̧��Ҳ��һֱ��¼����������һ�ε����
    /// </summary>
    public Transform clickObjectOfRightButton;

    /// <summary>
    /// ������(�Ҽ�)����¼����������壬���ſ�����գ����ᱣ����(�������µ��ſ���Ϊһ����������)
    /// </summary>
    public Transform rawObjectOfRightButton;

    /// <summary>
    /// ����Ҽ��Ƿ���(�������ק�����е�����)
    /// </summary>
    public bool isMouseRightPress = false;

    //------------------------�Ҽ� End-----------------------------------------

    private static RayEvent _instance;

    public static RayEvent Instance
    {
        get
        {
            return _instance ?? (_instance = new RayEvent());
        }
    }


    public bool isReadyInstanceObj = false;

    public GameObject tempInstanceObj;
    /// <summary>
    /// ��ʼ��
    /// </summary>
    public void Init()
    {
        Timer.Add(-1, Update);
    }

    /// <summary>
    /// ÿ֡����һ��
    /// </summary>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    private void Update(int arg1, object[] arg2)
    {
        //�����UI�ڵ����������ߡ�
        if (EventSystem.current.IsPointerOverGameObject()) return;

        MouseLeftButton();
        MouseRightButton();
    }

    /// <summary>
    /// ������
    /// </summary>
    private void MouseLeftButton()
    {
        //ZCore.Pose pose = _core.GetTargetPose(ZCore.TargetType.Primary, ZCore.CoordinateSpace.World);
        //bool isButtonPressed = _core.IsTargetButtonPressed(ZCore.TargetType.Primary, 0);
        //�������
        if (Input.GetMouseButtonDown(0))
        {
            isMouseLeftPress = true;
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            //��һ�ε��������
            previousClickObjectOfLeftButton = clickObjectOfLeftButton;

            if (Physics.Raycast(ray, out hit))
            {
                //����ê���λ�õ���
                //ModelCategory modelCategory = hit.transform.GetComponentInParent<ModelCategory>();
                //if (modelCategory)
                //{
                //    clickObjectOfLeftButton = modelCategory.transform;
                //    rawObjectOfLeftButton = modelCategory.transform;
                //}
                clickObjectOfLeftButton = hit.transform;
                rawObjectOfLeftButton = hit.transform;
            }
            else
            {
                clickObjectOfLeftButton = null;
                rawObjectOfLeftButton = null;
            }

            if (null != mouseLeftDownEvnet)
            {
                mouseLeftDownEvnet(mousePosition, hit);
            }
        }

        //���̧��
        if (Input.GetMouseButtonUp(0))
        {
            if (null != mouseLeftUpEvnet) mouseLeftUpEvnet();

            isMouseLeftPress = false;
            rawObjectOfLeftButton = null;
        }

        //�������
        if (isMouseLeftPress)
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit[] hitInfoArray = Physics.RaycastAll(ray);

            if (null != mouseLeftDragEvnet)
            {
                mouseLeftDragEvnet(mousePosition, hitInfoArray);
            }
        }
    }

    private void MouseRightButton()
    {
        //�Ҽ�����
        if (Input.GetMouseButtonDown(1))
        {
            isMouseRightPress = true;
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            //��һ�ε��������
            previousClickObjectOfRightButton = clickObjectOfRightButton;

            if (Physics.Raycast(ray, out hit))
            {
                clickObjectOfRightButton = hit.transform;
                rawObjectOfRightButton = hit.transform;
            }
            else
            {
                clickObjectOfRightButton = null;
                rawObjectOfRightButton = null;
            }

            if (null != mouseRightDownEvnet)
            {
                mouseRightDownEvnet(mousePosition, hit);
            }
        }

        //�Ҽ�̧��
        if (Input.GetMouseButtonUp(1))
        {
            isMouseRightPress = false;
            rawObjectOfRightButton = null;

            if (null != mouseRightUpEvnet) mouseRightUpEvnet();
        }

        //�Ҽ�����
        if (isMouseRightPress)
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit[] hitInfoArray = Physics.RaycastAll(ray);

            if (null != mouseRightDragEvnet)
            {
                mouseRightDragEvnet(mousePosition, hitInfoArray);
            }
        }
    }
}