using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    GameManager manager;

    SharedKnowledge sharedKnowledge;

    NPCUtils utils;
    NPCRandom random;
    NPCFollow follow;
    NPCBuild build;
    NPCSleep sleep;

    Grid grid;

    string targetTag = "Player";
    string bulletTag = "Bullet";
    public float goalTriggerRange = 8f, npcTiggerRange = 8f;
    public double reportTimeValidation = 5000;
    public int buildingSeconds = 3, sleepingSeconds = 3;
    public float followSpeed = 3, randomSpeed = 2;
    public GameObject wallPrefab;

    private StateMachine sm;
    State randomState, followState, buildState, sleepState;

    void Awake()
    {
        manager = FindObjectOfType<GameManager>();

        randomSpeed += (FindObjectOfType<GlobalVariables>().Level * 4) / 10;
        followSpeed += (FindObjectOfType<GlobalVariables>().Level * 4) / 10;

        sharedKnowledge = SharedKnowledge.SharedInstance;
        grid = FindObjectOfType<Grid>();

        utils = new NPCUtils(this, sharedKnowledge, transform, grid, targetTag, goalTriggerRange, npcTiggerRange, reportTimeValidation, buildingSeconds, sleepingSeconds, followSpeed, randomSpeed, wallPrefab);
        random = new NPCRandom(sharedKnowledge, transform, utils);
        follow = new NPCFollow(sharedKnowledge, transform, utils);
        build = new NPCBuild(sharedKnowledge, transform, utils);
        sleep = new NPCSleep(sharedKnowledge, transform, utils);
    }

    void Start()
    {
        // STATES
        randomState = new State();
        randomState.stayActions.Add(random.RandomPath);

        followState = new State();
        followState.stayActions.Add(follow.FollowPlayer);

        buildState = new State();
        buildState.enterActions.Add(build.BuildWall);

        sleepState = new State();
        sleepState.enterActions.Add(sleep.Sleep);

        // TRANSITION
        Transition tRandomCatch1 = new Transition(utils.SeePlayer);
        Transition tRandomCatch2 = new Transition(random.SawPlayer);

        Transition tFollowRandom = new Transition(follow.NotSawPlayer);

        Transition tBuildSleep = new Transition(build.NotBuilding);

        Transition tSleepRandom = new Transition(sleep.NotSleeping);

        // TRIGGERS
        randomState.AddTransition(tRandomCatch1, followState);
        randomState.AddTransition(tRandomCatch2, followState);
        followState.AddTransition(tFollowRandom, randomState);
        buildState.AddTransition(tBuildSleep, sleepState);
        sleepState.AddTransition(tSleepRandom, randomState);

        sm = new StateMachine(randomState);

        manager.ImHere();
    }

    void Update()
    {
        if (!manager.isEnded())
            sm.Update();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == bulletTag)
            sm.DoTransition(new Transition(), buildState);
    }
}