using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;//允许访问NavMeshAgent(导航网格)类

public class WaypointPatrol : MonoBehaviour
{
    //脚本作用:应该在首次加载场景和Ghost到达目的地时设置导航网格媒体代理的目的地。需要检查每一帧

    public NavMeshAgent navMeshAgent;
    public Transform[] waypoints;//Transform数组
    int m_CurrentWayPointIndex;//存储航点阵列的当前索引,用于跟踪下一个航点

    void Start()
    {
        navMeshAgent.SetDestination(waypoints[0].position);//设置Nav Mesh代理的初始目标
    }
    void Update()
    {
        //检查是否已到达目的地，简单方法是查看到目的地的剩余距离是否小于在"检查器"窗口中设置的停止距离
        if (navMeshAgent.remainingDistance<navMeshAgent.stoppingDistance)
        {
            //更新当前索引，使用它来设置NavMeshAgent的目标
            //当前索引添加一，如果该增量使索引等于路点数组中的元素数，则将其设置为零
            m_CurrentWayPointIndex = (m_CurrentWayPointIndex + 1)%waypoints.Length;
            navMeshAgent.SetDestination(waypoints[m_CurrentWayPointIndex].position);//更新初始目标
        }
    }
}
