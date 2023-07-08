using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using TMPro;
public class HandAI : MonoBehaviour
{
    [SerializeField] TMP_Text stateText;
    [SerializeField] Cursor cursor;
    StateMachine handAIStateMachine;

    State idleState, selectUnitsState, moveUnitsState, moveToSelectUnitsState;

    List<Enemy> unitsToBeSelected;
    List<Enemy> unitsSelected;
    Vector2 startSelectionPosition;
    Vector2 endSelectionPosition;

    Transform playerTransform;
    void Start()
    {

        idleState = new State("Idle", OnEnterIdle, OnExitIdle, OnUpdateIdle);
        selectUnitsState = new State("Select Units", OnEnterSelectUnits, OnExitSelectUnits, OnUpdateSelectUnits);
        moveUnitsState = new State("Move Units", OnEnterMoveUnits, OnExitMoveUnits, OnUpdateMoveUnits);
        moveToSelectUnitsState = new State("Move To Select Units", OnEnterMoveToSelectUnits, OnExitMoveToSelectUnits, OnUpdateMoveToSelectUnits);

        handAIStateMachine = new StateMachine(idleState);

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        handAIStateMachine.Update();
        stateText.text = handAIStateMachine.currentState.name;
    }

    void RecalculateSelectionData()
    {
        unitsToBeSelected = new List<Enemy>();
        float minX = Mathf.Infinity;
        float maxX = Mathf.NegativeInfinity;
        float minY = Mathf.Infinity;
        float maxY = Mathf.NegativeInfinity;
        foreach (Enemy enemy in GameObject.FindObjectsOfType<Enemy>())
        {
            if (enemy.GetState() == EnemyBehaviour.Idle)
            {
                unitsToBeSelected.Add(enemy);

                Vector3 screenPoint = Camera.main.WorldToScreenPoint(enemy.transform.position);
                if (screenPoint.x < minX)
                {
                    minX = screenPoint.x;
                }
                if (screenPoint.x > maxX)
                {
                    maxX = screenPoint.x;
                }
                if (screenPoint.y < minY)
                {
                    minY = screenPoint.y;
                }
                if (screenPoint.y > maxY)
                {
                    maxY = screenPoint.y;
                }
            }
        }

        startSelectionPosition = new Vector2(minX - 50, maxY + 50);
        endSelectionPosition = new Vector2(maxX + 50, minY - 50);

    }
    void OnEnterIdle()
    {
        Debug.Log("Entering Idle");
    }

    void OnExitIdle()
    {
        Debug.Log("Exiting Idle");
    }

    State OnUpdateIdle()
    {
        RecalculateSelectionData();
        if (unitsToBeSelected.Count > 0)
        {
            return moveToSelectUnitsState;
        }
        return idleState;
    }

    void OnEnterMoveToSelectUnits()
    {
        Debug.Log("Entering Move To Select Units");
        RecalculateSelectionData();
    }

    void OnExitMoveToSelectUnits()
    {
        Debug.Log("Exiting Move To Select Units");
    }

    State OnUpdateMoveToSelectUnits()
    {
        cursor.MoveTowards(startSelectionPosition);
        if (Vector3.Distance(cursor.transform.position, startSelectionPosition) < 0.1f)
        {
            return selectUnitsState;
        }
        return moveToSelectUnitsState;
    }

    void OnEnterSelectUnits()
    {
        Debug.Log("Entering Select Units");
        cursor.Click();
    }

    void OnExitSelectUnits()
    {
        Debug.Log("Exiting Select Units");
        unitsToBeSelected = null;
        cursor.Release();

    }

    State OnUpdateSelectUnits()
    {
        cursor.MoveTowards(endSelectionPosition);

        if (Vector3.Distance(cursor.transform.position, endSelectionPosition) < 0.1f)
        {
            Debug.Log("Selected Units");
            unitsSelected = new List<Enemy>();
            foreach (Enemy enemy in unitsToBeSelected)
            {
                unitsSelected.Add(enemy);
            }
            return moveUnitsState;
        }
        return selectUnitsState;
    }

    void OnEnterMoveUnits()
    {
        Debug.Log("Entering Move Units");
    }

    void OnExitMoveUnits()
    {
        Debug.Log("Exiting Move Units");
        cursor.Release();
    }

    State OnUpdateMoveUnits()
    {
        Vector2 target = Camera.main.WorldToScreenPoint(playerTransform.position);
        cursor.MoveTowards(target);

        if (Vector3.Distance(cursor.transform.position, target) < 0.1f)
        {
            cursor.Click();
            foreach (Enemy enemy in unitsSelected)
            {
                enemy.SetTarget(playerTransform);

            }
            return idleState;
        }
        return moveUnitsState;
    }

    void SetStateText(string text)
    {
        stateText.text = text;
    }

}
