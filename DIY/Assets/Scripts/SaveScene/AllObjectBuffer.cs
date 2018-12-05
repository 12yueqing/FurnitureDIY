using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/*
** Author      : Runing
** Time        : 12/1/2018 8:37:51 PM
** description : 
*/

public class AllObjectBuffer
{
    /// <summary>
    /// The key is model name
    /// The value is child node
    /// </summary>
    public Dictionary<string, List<OneNodeTransform>> transformBufferDictionary = new Dictionary<string, List<OneNodeTransform>>();

    public struct OneNodeTransform
    {
        public string name;
        public string position;
        public string quaterion;
        public string localScale;
        public bool isShow;
        public string textureName;
    }

    /// <summary>
    /// String is converted to Vector3
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private Vector3 ToVector3(string str)
    {
        String[] strArray = str.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

        if (strArray.Length != 3)
            Debug.LogError("AllObjectBuffer.ToVector3(): strArray count is not 3");

        return new Vector3(Convert.ToSingle(strArray[0]), Convert.ToSingle(strArray[1]), Convert.ToSingle(strArray[2]));
    }

    /// <summary>
    /// Vector3 is converted to string
    /// </summary>
    /// <param name="vec"></param>
    /// <returns></returns>
    private string FromVector3(Vector3 vec)
    {
        return vec[0] + "|" + vec[1] + "|" + vec[2];
    }

    /// <summary>
    /// String is converted to Quaternion
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private Quaternion ToQuaterion(string str)
    {
        String[] strArray = str.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

        if (strArray.Length != 4)
            Debug.LogError("AllObjectBuffer.ToQuaterion(): strArray count is not 4");

        return new Quaternion(Convert.ToSingle(strArray[0]), Convert.ToSingle(strArray[1]), Convert.ToSingle(strArray[2]), Convert.ToSingle(strArray[3]));
    }

    /// <summary>
    /// Quaternion is converted to string
    /// </summary>
    /// <param name="quaterion"></param>
    /// <returns></returns>
    private string FromQuaterion(Quaternion quaterion)
    {
        return quaterion.x + "|" + quaterion.y + "|" + quaterion.z + "|" + quaterion.w;
    }

    /// <summary>
    /// Objects that need to be record
    /// </summary>
    public void Record(List<Transform> objectList)
    {
        List<string> nodeNameList = new List<string>();
        transformBufferDictionary.Clear();

        for (int i = 0; i < objectList.Count; i++)
        {
            Transform model = objectList[i];
            nodeNameList.Clear();

            //Get all children in model
            Transform[] childTran = model.GetComponentsInChildren<Transform>(true);

            if (!transformBufferDictionary.ContainsKey(model.name))
            {
                List<OneNodeTransform> list = new List<OneNodeTransform>();
                transformBufferDictionary.Add(model.name, list);
            }

            //Circulating all chlidren
            for (int j = 0; j < childTran.Length; j++)
            {
                Transform child = childTran[j];

                OneNodeTransform oneNodeTransform = new OneNodeTransform();
                oneNodeTransform.name = child.name;
                oneNodeTransform.position = FromVector3(child.position);
                oneNodeTransform.quaterion = FromQuaterion(child.rotation);
                oneNodeTransform.localScale = FromVector3(child.localScale);
                oneNodeTransform.isShow = child.gameObject.activeSelf;

                //��ȡ�������ֲ����浽�����С�
                MeshRenderer mesh = child.GetComponent<MeshRenderer>();
                string textureName = "";

                if (null != mesh && null != mesh.material && null != mesh.material.mainTexture)
                {
                    textureName = mesh.material.mainTexture.name;
                }

                oneNodeTransform.textureName = textureName;
                transformBufferDictionary[model.name].Add(oneNodeTransform);

                //Check if the node name is repeated.
                if (!nodeNameList.Contains(child.name))
                {
                    nodeNameList.Add(child.name);
                }
                else
                {
                    Debug.LogError(string.Format("AllObjectBuffer.Record(): The node:{0} of the model:{1} is repeated !!!", child.name, model.name));
                }
            }
        }
    }

    /// <summary>
    /// Recover those objects that have been recorded
    /// </summary>
    public List<Transform> Recover(Transform modelRoot)
    {
        List<Transform> modelList = new List<Transform>();

        //��������������ģ��
        foreach (var kv in transformBufferDictionary)
        {
            //��������ģ�͵������У�����ȡ���������ӽڵ㡣
            Transform model = LoadModel(kv.Key, modelRoot);
            modelList.Add(model);
            Dictionary<string, Transform> childrenDictionary = GetModelChildren(model);

            //circulating buffer child.
            //����һ������ģ�͵������ӽڵ�
            List<OneNodeTransform> bufferChildList = kv.Value;
            for (int i = 0; i < bufferChildList.Count; i++)
            {
                //�û���ģ�͵Ľڵ����ø�����ģ�ͽڵ�,�ﵽ�ָ�������Ŀ�ġ�
                OneNodeTransform bufferChild = bufferChildList[i];
                RecoverOneNode(childrenDictionary, bufferChild);
            }
        }

        return modelList;
    }

    /// <summary>
    /// �ӱ��ؼ���ģ��
    /// </summary>
    /// <param name="bufferModelNameEX">EX Ϊ����׺��</param>
    /// <param name="parent"></param>
    /// <returns></returns>
    private Transform LoadModel(string bufferModelNameEX, Transform parent)
    {
        string bufferModelName = bufferModelNameEX;

        //todo ���Ľ�, ��������ʽ
        for (int i = 0; i < 20; i++)
        {
            string ex = "_" + i;
            if (bufferModelName.Contains(ex))
                bufferModelName = bufferModelName.Replace(ex, "");
        }

        //load model
        Transform model = ResourceManager.Instance.LoadModel(parent, bufferModelName);
        model.name = bufferModelNameEX;
        return model;
    }

    /// <summary>
    /// ��ȡһ��ģ�������еĽڵ�
    /// </summary>
    /// <param name="model"></param>
    /// <returns>keyΪ�ڵ����֣�valueΪ�ڵ�Transform</returns>
    private Dictionary<string, Transform> GetModelChildren(Transform model)
    {
        Transform[] childArray = model.gameObject.GetComponentsInChildren<Transform>(true);
        Dictionary<string, Transform> childDictionary = new Dictionary<string, Transform>();

        //Array is converted to Dictionary.
        for (int i = 0; i < childArray.Length; i++)
        {
            Transform child = childArray[i];
            childDictionary.Add(child.name, child);
        }

        return childDictionary;
    }

    /// <summary>
    /// �ָ�һ���ڵ�
    /// </summary>
    /// <param name="childrenDictionary">��ǰһ��ģ�����еĽڵ�</param>
    /// <param name="bufferChild">����ģ���е�һ���ڵ�</param>
    private void RecoverOneNode(Dictionary<string, Transform> childrenDictionary, OneNodeTransform bufferChild)
    {
        if (childrenDictionary.ContainsKey(bufferChild.name))
        {
            Transform child = childrenDictionary[bufferChild.name];
            child.name = bufferChild.name;
            child.position = ToVector3(bufferChild.position);
            child.rotation = ToQuaterion(bufferChild.quaterion);
            child.localScale = ToVector3(bufferChild.localScale);
            child.gameObject.SetActive(bufferChild.isShow);

            //��������
            MeshRenderer mesh = child.GetComponent<MeshRenderer>();
            if (!string.IsNullOrEmpty(bufferChild.textureName) && null != mesh)
            {
                Texture texture = ResourceManager.Instance.LoadTexture(bufferChild.textureName);

                //�����Resources���Ҳ��������Ͳ�Ҫ���ˡ�
                //�ܿ����������û�б��滻���ģ���ô�Ͳ�Ҫȥ������������Ĭ�ϵ�������С�
                if (null != texture)
                {
                    mesh.material.mainTexture = texture;
                }
            }
        }
        else
        {
            //��ǰģ�ͽڵ㲻�ڻ���ģ�ͽڵ��У�˵����ǰģ���ж���û�м��ؽ�����
            //����ģ��
            //�ݹ�
            //Transform model = LoadModel(kv.Key, modelRoot);
            //modelList.Add(model);
            //Dictionary<string, Transform> childrenDictionary = GetModelChildren(model);
        }
    }
}
