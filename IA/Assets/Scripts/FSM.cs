using UnityEngine;

public class FSM : MonoBehaviour
{
    public enum EnemyState
    {
        Patrol, 
        Persuit,
        Kill,
        Wander
    }


    public EnemyState currentState = EnemyState.Patrol;

   public void UpdateState(bool canSeePlayer, bool isNear)
    {
        switch (currentState) 
        { 
         case EnemyState.Patrol:

                if (canSeePlayer) 
                { 
                currentState = EnemyState.Persuit;
                }else if(!canSeePlayer)
                {
                    currentState = EnemyState.Wander;
                }

            break;
        case EnemyState.Persuit:

                if (!canSeePlayer) 
                {
                    currentState = EnemyState.Wander;
                
                }

                if (canSeePlayer && isNear)
                {
                    currentState = EnemyState.Kill;

                }
               
                break;

            case EnemyState.Kill:

                if (!isNear && canSeePlayer)
                {
                    currentState = EnemyState.Persuit;
                }

                if (!canSeePlayer)
                {
                    currentState = EnemyState.Wander;
                }

                break;

            case EnemyState.Wander:

                if (canSeePlayer)
                {
                    currentState = EnemyState.Persuit;
                }

                break;


        }
    }
   
}
