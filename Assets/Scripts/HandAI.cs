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
    Vector3 startSelectionPosition;
    Vector3 endSelectionPosition;

    Transform playerTransform;

    float targetTolerance = 0.1f;
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
        float minZ = Mathf.Infinity;
        float maxZ = Mathf.NegativeInfinity;
        foreach (Enemy enemy in GameObject.FindObjectsOfType<Enemy>())
        {
            if (enemy.GetState() == EnemyBehaviour.Idle)
            {
                unitsToBeSelected.Add(enemy);

                Vector3 point = enemy.transform.position;
                if (point.x < minX)
                {
                    minX = point.x;
                }
                if (point.x > maxX)
                {
                    maxX = point.x;
                }
                if (point.y < minZ)
                {
                    minZ = point.z;
                }
                if (point.y > maxZ)
                {
                    maxZ = point.z;
                }
            }
        }

        startSelectionPosition = new Vector3(minX - 1, cursor.transform.position.y, maxZ + 1);
        endSelectionPosition = new Vector3(maxX + 1, cursor.transform.position.y, minZ - 1);

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
        RecalculateSelectionData();
        cursor.MoveTowards(startSelectionPosition);
        if (Vector3.Distance(cursor.transform.position, startSelectionPosition) < targetTolerance)
        {
            return selectUnitsState;
        }
        return moveToSelectUnitsState;
    }

    void OnEnterSelectUnits()
    {
        Debug.Log("Entering Select Units");
        cursor.Click();
        cursor.StartSelecting();
    }

    void OnExitSelectUnits()
    {
        Debug.Log("Exiting Select Units");
        unitsToBeSelected = null;
        cursor.Release();
        cursor.StopSelecting();

    }

    State OnUpdateSelectUnits()
    {
        RecalculateSelectionData();
        cursor.MoveTowards(endSelectionPosition);

        if (Vector3.Distance(cursor.transform.position, endSelectionPosition) < targetTolerance)
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
        Vector3 target = new Vector3(playerTransform.position.x, cursor.transform.position.y, playerTransform.position.z);
        cursor.MoveTowards(target);

        if (Vector3.Distance(cursor.transform.position, target) < targetTolerance)
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
