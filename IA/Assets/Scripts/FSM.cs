using UnityEngine;

public class FSM : MonoBehaviour
{
    public enum EnemyState
    {
        Patrol, 
        Persuit,
        Kill
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
                
                }

            break;
        case EnemyState.Persuit:

                if (!canSeePlayer) 
                {
                    currentState = EnemyState.Patrol;
                
                }

                if (canSeePlayer && isNear)
                {
                    currentState = EnemyState.Kill;

                }
                break;

        
        }
    }
   
}
