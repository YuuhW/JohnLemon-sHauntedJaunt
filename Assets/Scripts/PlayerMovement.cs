using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed=20f;//设置为3，则转身需要一秒钟，设置为6，转身需要半秒钟
    Animator m_Animator;//用于访问Animator组件
    Rigidbody m_Rigidbody;//将移动和旋转应用于角色，方式为控制刚体
    AudioSource m_AudioSource;

    Vector3 m_Movement;//运动矢量声明；非公共成员变量，命名方式：m_开头
    Quaternion m_Rotation=Quaternion.identity;//定义四元数用来存储旋转

    // Start is called before the first frame update
    void Start()
    {
        //GetComponent是MonoBehaviour的一部分
        m_Animator = GetComponent<Animator>();//先调用Start方法，因此在这设置对Animator组件的引用
        m_Rigidbody = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()//在物理系统解决已发生的任何冲突和其他交互之前调用FixedUpdate。默认情况下，它每秒调用50次。
    {
        float horizontal = Input.GetAxis("Horizontal");//创建一个新的浮点变量并将其调用为水平; 将该变量设置为等于此方法调用的结果。
        float vertical = Input.GetAxis("Vertical");//垂直

        m_Movement.Set(horizontal,0f,vertical);
        //运动矢量由两个数字组成，最大值为1.如果它们的值均为1，则矢量的长度（称为其幅度）将大于1.这是两侧之间的关系毕达哥拉斯定理描述的三角形。
        //归一化向量意味着保持向量的方向相同，但将其大小更改为1。代码如下：
        m_Movement.Normalize();

        //此方法称为近似，来自Mathf类。它需要两个float参数并返回一个bool。如果两个浮点数大致相等则为true，否则为false。
        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;

        //第一个参数是您要设置值的Animator参数的名称，第二个参数是您要将其设置为的值。
        m_Animator.SetBool("IsWalking",isWalking);
        if (isWalking)//用来控制声音
        {
            if (!m_AudioSource.isPlaying)//不会每帧都调用Play
            {
                m_AudioSource.Play();
            }
        }
        else
        {
            m_AudioSource.Stop();
        }

        //RotateTowards：转向
        //以transform.forward 开头，以m_Movement 变量为目标。
        //后面两个参数是起始矢量和目标矢量之间的变化量：首先是角度的变化（以弧度表示），然后是幅度的变化。
        //Time.deltaTime是自上一帧以来的时间（也可将其视为帧之间的时间）。它保证无论游戏帧数是60还是30，相同时间内转向角度都相同
        Vector3 desiredForward=Vector3.RotateTowards(transform.forward,m_Movement,turnSpeed*Time.deltaTime,0f);

        m_Rotation = Quaternion.LookRotation(desiredForward);//简单地调用LookRotation 方法并创建一个指向给定参数方向的旋转
    }

    void OnAnimatorMove()
    {
        //m_Rigidbody.position：新位置为当前位置。deltaPosition是由于应用于此帧的根运动而导致的位置变化
        m_Rigidbody.MovePosition(m_Rigidbody.position+m_Movement*m_Animator.deltaPosition.magnitude);
        m_Rigidbody.MoveRotation(m_Rotation);
    }
}
