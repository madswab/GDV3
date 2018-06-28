using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
namespace VaalsGOAP
{
    public class Planner
    {
        public static List<T> Shuffle<T>(List<T> list)
        {

            for(int i=0; i < list.Count; i++)
            {
                T temp = list[i];
                int num = UnityEngine.Random.Range(i, list.Count);
                list[i] = list[num];
                list[num] = temp;
            }

            return list;
        }

        public static void PlanActions(State playerState, List<Action> availableActions, List<Goal> availableGoals, out Goal outGoal)
        {
            outGoal = null;
            availableGoals = Shuffle<Goal>(availableGoals);
            foreach (Goal goal in availableGoals.OrderByDescending(g => g.priority).ToList())
            {
                if (goal.IsViable(playerState))
                {
                    TryPlanWithAStar(goal, playerState, availableActions, ref outGoal);
                    if (outGoal != null)
                    {
                        return;
                    }
                }
            }

        }

        public static void TryPlanWithAStar(Goal goalToReach, State currentState, List<Action> availableActions, ref Goal outGoal)
        {
            List<Node> closedList = new List<Node>();
            List<Node> openList = new List<Node>();

            State s = new State();
            s.state = currentState.state;
            openList.Add(new Node(null, null, s, 0));
            Dictionary<Effect, int> gScore = new Dictionary<Effect, int>();
            gScore.Add(openList[0].state.state, 0);

            float loopsMade = 0;

            while (openList.Count > 0 && loopsMade < 50)
            {
                loopsMade++;
                Node curNode = openList.OrderByDescending((t) => t.cumulativeCost).ToList().LastOrDefault();

                if (curNode.state.CheckIfEffectsArePresent(goalToReach.effects))
                {
                    outGoal = goalToReach;
                    ReconstructPath(curNode, ref outGoal);
                    return;
                }

                openList.Remove(curNode);
                closedList.Add(curNode);
                
                foreach(Action action in availableActions)
                {
                    if (curNode.state.CheckIfEffectsArePresent(action.conditions))
                    {
                        Node n = curNode.CloneNode();
                        n.parentNode = curNode;
                        n.cumulativeCost += action.cost;
                        n.actionForThisNode = action;
                        n.state.AddEffectsToState(action.effects);
                        if (closedList.Contains(n, new NodeComparer()))
                        {
                            continue;
                        }
                        int tentativeScore = n.cumulativeCost;
                        if (!openList.Contains(n, new NodeComparer()))
                        {
                            openList.Add(n);
                        }
                        else if (tentativeScore >= gScore[n.state.state])
                        {
                            continue;
                        }
                        gScore[n.state.state] = tentativeScore;
                    }

                }

            }
            return;
        }

        public static void ReconstructPath(Node endNode, ref Goal outGoal)
        {
            outGoal.actionStack = new Stack<Action>();
            while(endNode.parentNode != null && endNode.actionForThisNode != null)
            {
                outGoal.actionStack.Push(endNode.actionForThisNode);
                endNode = endNode.parentNode;
            }
        }
    }

    public class Node
    {
        public Node parentNode;
        public Action actionForThisNode;
        public State state;
        public int cumulativeCost;
        public Node() { }
        public Node(Node parentNode, Action action, State state, int cumulativeCost)
        {

            this.state = state;
            actionForThisNode = action;
            this.parentNode = parentNode;
            this.cumulativeCost = cumulativeCost;
        }

        public void AddEffectsToState(List<EffectState> effects)
        {
            state.AddEffectsToState(effects);
        }

        public Node CloneNode()
        {
            Node n = (Node)this.MemberwiseClone();
            n.state = new State();
            n.state.state = this.state.state;
            return n;
            
        }
    }

    public class NodeComparer : IEqualityComparer<Node>
    {
        public bool Equals(Node n1, Node n2)
        {
            return n1.state.CompareState(n2.state);
        }
        public int GetHashCode(Node n)
        {
            return n.GetHashCode();
        }

    }

}