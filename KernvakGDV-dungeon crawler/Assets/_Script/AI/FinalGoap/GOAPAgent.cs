using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace VaalsGOAP {
    public abstract class GOAPAgent : MonoBehaviour
    {
        [HideInInspector]
        public State state;

        [HideInInspector]
        public List<Goal> allGoals;
        public List<Goal> allGoalsOriganal;

        [HideInInspector]
        public List<Action> allActions;
        public List<Action> allActionsOriganal;

        private Goal currentGoal;
        public Action currentAction;

        internal NavMeshAgent agent;

        public bool playerInRange = false;
        public bool AIInRange = false;
        public LayerMask AILayer;

        private float checkPlayerRange = 0;
        private float checkAIRange = 0;

        [HideInInspector]
        public Vector3 playerPos;
        public GameObject gun;
        public GameObject bullet;


        // Use this for initialization
        protected virtual void Start()
        {
            agent = GetComponent<NavMeshAgent>();

            if (allActions.Count > 0)
            {
                allActions.Clear();
            }

            foreach (Action item in allActionsOriganal)
            {
                allActions.Add(item.Clone());
            }


            if (allGoals.Count > 0)
            {
                allGoals.Clear();
            }

            foreach (Goal item in allGoalsOriganal)
            {
                allGoals.Add(item.Clone());
            }

        }

        protected void InitializeActionsAndGoals() { 

            foreach (Action ac in allActions)
            {
                //Debug.Log("Action Initialized: " + ac);
                ac.Initialize(this);
            }
            foreach (Goal g in allGoals)
            {
                //Debug.Log("Action Initialized: " + g);
                g.Initialize(this);
            }
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            if (playerInRange)
            {
                checkPlayerRange += Time.deltaTime;
                if (playerInRange && checkPlayerRange > 2f)
                {
                    playerInRange = false;                               
                }
            }

            if (AIInRange)
            {
                checkAIRange += Time.deltaTime;
                if (AIInRange && checkAIRange > 2f)
                {
                    AIInRange = false;
                }
            }
            else{
                CheckAIInRange(transform.position);
            }

            ExecuteGoal();
        }

        public void CheckPlayerInRange(Vector3 pPos)
        {
            checkPlayerRange = 0;
            playerInRange = true;
            playerPos = pPos;

        }

        public void CheckAIInRange(Vector3 pPos)
        {
            Collider[] hitcol = Physics.OverlapSphere(transform.position, 5, AILayer);
            foreach (var item in hitcol)
            {
                if (item.transform != transform && item.GetComponent<GOAPAgent>())
                {
                    checkAIRange = 0;
                    AIInRange = true;
                    break;
                }
            }

        }

        public void ExecuteGoal()
        {
            if (currentGoal == null)
            {
                ReplanGoal();
            }
            else if (currentAction == null)
            {
                GetNextAction();
            } else if (currentAction.IsViable() && currentGoal.IsViable(state))
            {
                currentAction.OnUpdateAction();
            }
            else
            {
                //Current Goal or Action no longer viable! Replan!
                ReplanGoal();
            }
            
        }

        public void OnActionCompleted(Action action)
        {
            //Debug.Log("Action Completed!");
            state.AddEffectsToState(action.effects);
            action.OnExitAction();


            GetNextAction();
        }

        public void OnActionFailed(Action action)
        {
            action.OnExitAction();
            currentAction = null;
        }

        public void GetNextAction()
        {
            if(currentGoal != null && currentGoal.actionStack.Count > 0)
            {
                currentAction = currentGoal.actionStack.Pop();
                currentAction.OnEnterAction();
            }
            else
            {
                //CurrentGoal Completed Succesfully! Replan!
                //Debug.Log("Goal Completed!");
                ReplanGoal();
            }
           

            
        }

        public void ReplanGoal()
        {
            if(currentAction != null)
            {
                currentAction.OnExitAction();
            }
            currentAction = null;
            currentGoal = null;
            Planner.PlanActions(state, allActions, allGoals, out currentGoal);
            
            //Debug
            //if (currentGoal != null)
            //{
            //    //Debug.Log("Goal Found!" + currentGoal);
            //    foreach (Action ac in currentGoal.actionStack)
            //    {
            //        Debug.Log("Actions For Current Goal: " + ac);

            //    }

            //}
            //else
            //{
            //    Debug.Log("Goal not Found!");
            //}

            
            
        }
    }
}

