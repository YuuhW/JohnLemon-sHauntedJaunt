using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    public Transform player;//检查玩家角色的Transform 而不是GameObjects
    public GameEnding gameEnding;
    bool m_IsPlayerInRange;

    void OnTriggerEnter(Collider other)
    {
        if (other.transform==player)
        {
            m_IsPlayerInRange = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.transform== player)
        {
            m_IsPlayerInRange = false;
        }
    }

    void Update()
    {
        if (m_IsPlayerInRange)//当玩家角色实际在范围内时，检查视线是否有意义
        {
            Vector3 direction = player.position - transform.position + Vector3.up;//向量计算距离。Vector3.up是（0,1,0）的快捷方式 ，指方向
            Ray ray=new Ray(transform.position,direction);//一条射线，沿视线方向
            RaycastHit raycastHit;
            //out参数有一个名为RaycastHit 的类型，Raycast方法将其数据设置为有关Raycast命中的信息(返回被命中者的信息)
            if (Physics.Raycast(ray,out raycastHit))//返回一个bool，当它击中某个东西时它是true，而当它没有击中任何东西时是false.
            {
                if (raycastHit.collider.transform==player)
                {
                    gameEnding.CaughtPlayer();//转到GameEnding重启游戏
                }
            }
        }
    }
}
