using System;
using Unity.Behavior;

[BlackboardEnum]
public enum EnemyState
{
	Idle   ,	// 행동 대기
	Acting ,	// 행동중
	Waiting,	// 상대 턴
}
