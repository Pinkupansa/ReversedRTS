using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using TMPro;
public class HandAI : MonoBehaviour
{
    [SerializeField] TMP_Text stateText;
    [SerializeField] Cursor cursor;

    [SerializeField] float attackModeProbability;
    [SerializeField] Transform deleteTarget;
    StateMachine handAIStateMachine;

    State idleState, selectUnitsState, moveUnitsState, moveToSelectUnitsState, moveToClickPlayerState, clickPlayerState, angryState, deleteState;

    List<Enemy> unitsToBeSelected;
    List<Enemy> unitsSelected;
    Vector3 startSelectionPosition;
    Vector3 endSelectionPosition;

    Transform playerTransform;

    float targetTolerance = 0.1f;

    float timeIdleMin = 1f;
    float idleTimer = 0f;
    void Start()
    {

        idleState = new State("Idle", OnEnterIdle, OnExitIdle, OnUpdateIdle);
        selectUnitsState = new State("Select Units", OnEnterSelectUnits, OnExitSelectUnits, OnUpdateSelectUnits);
        moveUnitsState = new State("Move Units", OnEnterMoveUnits, OnExitMoveUnits, OnUpdateMoveUnits);
        moveToSelectUnitsState = new State("Move To Select Units", OnEnterMoveToSelectUnits, OnExitMoveToSelectUnits, OnUpdateMoveToSelectUnits);
        moveToClickPlayerState = new State("Move To Click Player", OnEnterMoveToClickPlayer, OnExitMoveToClickPlayer, OnUpdateMoveToClickPlayer);
        handAIStateMachine = new StateMachine(idleState);
        clickPlayerState = new State("Click Player", OnEnterClickPlayer, OnExitClickPlayer, OnUpdateClickPlayer);
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        angryState = new State("Angry", OnEnterAngry, OnExitAngry, OnUpdateAngry);
        deleteState = new State("Delete", OnEnterDelete, OnExitDelete, OnUpdateDelete);


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

        startSelectionPosition = new Vector3(minX - 2, cursor.transform.position.y, maxZ + 2);
        endSelectionPosition = new Vector3(maxX + 2, cursor.transform.position.y, minZ - 2);


    }
    void OnEnterIdle()
    {
        // Debug.Log("Entering Idle");
    }

    void OnExitIdle()
    {
        //Debug.Log("Exiting Idle");
    }

    State OnUpdateIdle()
    {
        idleTimer += Time.deltaTime;
        RecalculateSelectionData();
        if (idleTimer > timeIdleMin)
        {
            idleTimer = 0f;
            if (unitsToBeSelected.Count > 0)
            {
                float random = Random.Range(0f, 1f);
                if (random < attackModeProbability && Vector3.Distance(playerTransform.position, cursor.transform.position) > 2f)
                {
                    return moveToClickPlayerState;
                }
                else
                {
                    return moveToSelectUnitsState;
                }
            }
            else
            {
                return moveToClickPlayerState;
            }
        }
        return idleState;

    }

    void OnEnterMoveToSelectUnits()
    {
        //Debug.Log("Entering Move To Select Units");
        RecalculateSelectionData();
    }

    void OnExitMoveToSelectUnits()
    {
        //Debug.Log("Exiting Move To Select Units");
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
        //Debug.Log("Entering Select Units");
        cursor.Click();
        cursor.StartSelecting();
    }

    void OnExitSelectUnits()
    {
        //.Log("Exiting Select Units");
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
            //Debug.Log("Selected Units");
            unitsSelected = new List<Enemy>();
            foreach (Enemy enemy in unitsToBeSelected)
            {
                enemy.selectionCircle.GetComponent<SelectionCircle>().Activate();
                unitsSelected.Add(enemy);
            }
            return moveUnitsState;
        }
        return selectUnitsState;
    }

    void OnEnterMoveUnits()
    {
        //Debug.Log("Entering Move Units");
    }

    void OnExitMoveUnits()
    {
        //Debug.Log("Exiting Move Units");
        cursor.Click();
    }

    State OnUpdateMoveUnits()
    {
        Vector3 target = new Vector3(playerTransform.position.x, cursor.transform.position.y, playerTransform.position.z);
        cursor.MoveTowards(target);

        if (Vector3.Distance(cursor.transform.position, target) < targetTolerance)
        {

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

    void OnEnterMoveToClickPlayer()
    {
        //Debug.Log("Entering Move To Click Player");
    }

    void OnExitMoveToClickPlayer()
    {
        //Debug.Log("Exiting Move To Click Player");



    }

    State OnUpdateMoveToClickPlayer()
    {
        Vector3 target = new Vector3(playerTransform.position.x, cursor.transform.position.y, playerTransform.position.z);
        cursor.MoveTowards(target);

        if (Vector3.Distance(cursor.transform.position, target) < targetTolerance)
        {
            return angryState;
        }
        return moveToClickPlayerState;
    }

    void OnEnterAngry()
    {
        cursor.Angry();
    }

    void OnExitAngry()
    {

    }

    State OnUpdateAngry()
    {
        if (cursor.IsAnimOn("angry"))
        {
            return angryState;
        }
        return clickPlayerState;
    }

    void OnEnterClickPlayer()
    {
        // Debug.Log("Entering Click Player");
        cursor.Click();
    }

    void OnExitClickPlayer()
    {
        // Debug.Log("Exiting Click Player");
    }

    State OnUpdateClickPlayer()
    {
        Vector3 playerPos = new Vector3(playerTransform.position.x, cursor.transform.position.y, playerTransform.position.z);
        if (Vector3.Distance(cursor.transform.position, playerPos) < 2f)
        {
            return deleteState;
        }
        return idleState;
    }

    void OnEnterDelete()
    {
        cursor.ShowDeleteMenu();
    }

    void OnExitDelete()
    {
        cursor.Click();
        cursor.HideDeleteMenu();
        Player.instance.TakeDamage(1, true);
    }

    State OnUpdateDelete()
    {
        Vector3 target = new Vector3(deleteTarget.position.x, cursor.transform.position.y, deleteTarget.position.z);
        cursor.MoveTowards(target);
        if (Vector3.Distance(cursor.transform.position, target) > targetTolerance)
        {
            return deleteState;
        }
        return idleState;

    }

}
