using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowAction : MonoBehaviour
{
        
        public Transform target; // Ŀ������
        public Vector3 offset; // �����Ŀ���ƫ����

    // Start is called before the first frame update
    void Start()
    {
        // �����ʼƫ����
        offset = transform.position - target.position;
        //offset.y -= 3;
        //offset.x += 1;
    }

    // Update is called once per frame
    void Update()
    {
        float originalRotationX = transform.rotation.eulerAngles.x;
        // �����������λ�ã�ʹ��ʼ����Ŀ��ĺ�
        transform.position = target.position + offset;
        // �������������ת��ʹ��ʼ�տ���Ŀ��
        transform.LookAt(target);
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.x = originalRotationX;
        transform.rotation = Quaternion.Euler(rotation);
    }


}
