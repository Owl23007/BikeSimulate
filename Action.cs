using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class Action : MonoBehaviour
{

    public BicycleState Simulate(BicycleState currentState, ControlStrategy control)
    {

        
        float newLeanAngle;
        if (currentState.LeanAngle > 0)
        {
            // �����ǰ�����㣬��ô LeanAngle Ӧ������
            newLeanAngle = currentState.LeanAngle + 0.5f * (1 - currentState.Speed / maxSpeed);
        }
        else if (currentState.LeanAngle < 0)
        {
            // �����ǰ�����㣬��ô LeanAngle Ӧ�ü���
            newLeanAngle = currentState.LeanAngle - 0.5f * (1 - currentState.Speed / maxSpeed);
        }
        else
        {
            float randomFactor = (Random.value - 0.5f) * 2; // ����һ���� -1 �� 1 ֮��������
            // �����ǰ���м�λ�ã���ô LeanAngle Ӧ�ø�����������ӻ����
            newLeanAngle = currentState.LeanAngle + randomFactor * (1 - currentState.Speed / maxSpeed);
        }

        var nextState = new BicycleState
        {
            Position = currentState.Position +
            currentState.Speed * new Vector3
            (Mathf.Cos(currentState.Direction * Mathf.Deg2Rad), 0, -Mathf.Sin(currentState.Direction * Mathf.Deg2Rad)),
            TargetPosition = currentState.TargetPosition,
            Speed = currentState.Speed * speedDecay, // �����ٶȱ��ֲ���
            Direction = currentState.Direction,
            LeanAngle = newLeanAngle// ����������
        };
        return nextState;
    }


    public class BicycleState
    {
        public Vector3 Position { get; set; } // ��ǰλ��
        public Vector3 TargetPosition { get; set; } // Ŀ��λ��
        public float Speed { get; set; } // ���г��ٶ�
        public float Direction { get; set; } // ���з���
        public float LeanAngle { get; set; } // ��ƫ�Ƕ�
    }


    public class ControlStrategy
    {
        public float AdjustDirection { get; set; } // ��������
    }

    private BicycleState currentState;
    private ControlStrategy control;
    private float inputInterval = 0.2f; // ����Ƶ�����ƣ���λΪ��
    private float lastInputTime; // �ϴ������ʱ��
    private float lastPositionX = 0; // ���ڴ洢������һ�ε�X����
    public float score = 0f; // ���ڴ洢�÷�

    public float maxSpeed = 10f;//speedmax
    public float speedDecay = 0.99f; // �ٶ�˥������
    public float turnSpeed = 40f;
    public float leanSpeed = 0.5f;
    public float acceleration = 3f;
    public bool isTrue = false;

    IEnumerator StartAfterDelay()
    {
        // �ȴ�5��
        yield return new WaitForSeconds(5);
        enabled = true;
        // ��������������ڵȴ�5���ִ�еĴ���
    }

    void Start()
    {

        // ���������Գ�ʼ�����г���״̬�Ϳ��Ʋ��ԡ�
        currentState = new BicycleState
        {

            Position = Vector3.zero,
            TargetPosition = new Vector3(270, 0, 0),
            Speed = 1f,
            Direction = 0.0f,
            LeanAngle = 0.0f
        };
        StartCoroutine(StartAfterDelay());
        enabled = false;
        control = new ControlStrategy
        {
            AdjustDirection = 0.1f
        };

    }

    public float GetScore()
    {
        // ���ص÷�
        return score;
    }



    void Update()
    {
        score += Time.deltaTime;
        if (transform.position.x > lastPositionX)
        {
            // ���������X�����������ƶ�����ô���ӵ÷�
            score += transform.position.x - lastPositionX;
        }

        // ������һ�ε�X����
        lastPositionX = transform.position.x;

        // ����Ƿ��Ѿ������㹻��ʱ��
        if (Time.time - lastInputTime < inputInterval)
        {
            return;
        }


        // ��鰴�����������г���״̬����Ʋ���
        if (Input.GetKey(KeyCode.W))
        {
            // W�����٣����һ�����ٶȱ���
            currentState.Speed = Mathf.Min(currentState.Speed + acceleration * Time.deltaTime, maxSpeed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            // A����ת�����һ��ת����ٶȱ���������ת��Ƕ������г����ٶȹ�������
            currentState.Direction = (currentState.Direction - turnSpeed * Time.deltaTime + 360) % 360;
        }
        if (Input.GetKey(KeyCode.D))
        {
            // D����ת�����һ��ת����ٶȱ���������ת��Ƕ������г����ٶȹ�������
            currentState.Direction = (currentState.Direction + turnSpeed * Time.deltaTime + 360) % 360;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            // Q�����㣬���һ����б���ٶȱ�����������б�Ƕ������г���ת���ٶȹ�������
            currentState.LeanAngle += leanSpeed * Time.deltaTime * Mathf.Abs(turnSpeed);
        }
        if (Input.GetKey(KeyCode.E))
        {
            // E�����㣬���һ����б���ٶȱ�����������б�Ƕ������г���ת���ٶȹ�������
            currentState.LeanAngle -= leanSpeed * Time.deltaTime * Mathf.Abs(turnSpeed);
        }



        // ������������Ӵ�����ʵʱ�������غ����г�״̬��
        // ���磬����Ը������г���λ�ú���ת��
        // ����״̬
        currentState = Simulate(currentState, control);

        // �������г���λ�ú���ת
        transform.position = currentState.Position;
        Vector3 eulerAngle = new Vector3(currentState.LeanAngle, currentState.Direction, 0);
        transform.rotation = Quaternion.Euler(eulerAngle);

        // ��������Ѿ����������ǿ���ֹͣ���¡�
        if (IsFinished(currentState))
        {
            isTrue = true;
            enabled = false;
        }
    }

    bool IsFinished(BicycleState state)
    {
        // ���������Ҫ������г��Ƿ��Ѿ�����Ŀ��λ�ã����߲�ƫ�Ƕ��Ƿ��Ѿ�����45�ȡ�
        // ����һ��ʾ��ʵ�֣�����Ҫ����ʵ����������޸ģ�
        return Mathf.Abs(state.LeanAngle) > 45;
    }
   

}