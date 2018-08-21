using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Called when an actor is moved. It contains the actor, the previous location,
// and the new location in that order
[System.Serializable]
public class ActorMovedEvent : UnityEvent<Actor, Vector2Int, Vector2Int> { }

// Called when an actor is added to the map. It contains the actor and its
// location
[System.Serializable]
public class ActorAddedEvent : UnityEvent<Actor, Vector2Int> { }

// The map class manages the logical state and locations of each actor on the 
// gameboard. It is responsible for holding and updating the state.
//
// There should be no other copy of this data elsewhere.
//
// The map class is not responsible for game logic or deciding when a actor's
// state should be updated, only for keeping track of the information. When the
// state of a actor needs to be updated or the actor needs to be moved, the 
// corresponding function must be called on this object. Once the state has been
// altered, the relevent event will be triggered. All game objects that need to 
// be aware of the state of the map must listen to the relevent event, and must
// not maintain a copy of the gameboard's state.
public class ActorManager : MonoBehaviour {

    public ActorMovedEvent actorMoved;
    public ActorAddedEvent actorAdded;

    public int mapWidth, mapHeight;

    List<Actor> actors = new List<Actor>();
    List<List<Actor>> actorLocations = new List<List<Actor>>();

	void Start () {
        InitializeGameboard();
	}

    public void AddActor(Actor actor, Vector2Int loc) {
        ValidateLocation(loc);
        Debug.Assert(actor != null);
        Debug.Assert(actorLocations[loc.x][loc.y] == null);

        actors.Add(actor);
        actorLocations[loc.x][loc.y] = actor;
        actor.location = loc;
        actorAdded.Invoke(actor, loc);
    }

    public void MoveActor(Vector2Int from, Vector2Int to) {
        ValidateLocation(from);
        ValidateLocation(to);

        Actor movingActor = actorLocations[from.x][from.y];
        Debug.Assert(movingActor != null);
        Debug.Assert(actorLocations[to.x][to.y] == null);

        // Update the actor's internal location
        movingActor.location = to;

        // Update the state of the actorLocations
        actorLocations[to.x][to.y] = movingActor;
        actorLocations[from.x][from.y] = null;

        // Fire the event
        actorMoved.Invoke(movingActor, from, to);
    }

    public void MoveActor(Actor actor, Vector2Int to)
    {
        Debug.Assert(actor != null);
        MoveActor(GetActorLocation(actor).Value, to);
    }

    public Vector2Int? GetActorLocation(Actor targetActor) {
        Debug.Assert(targetActor != null);
        foreach(Actor actor in actors) {
            if (actor == targetActor) {
                return actor.location;
            }
        }
        return null;
    }

    void InitializeGameboard() {
        for (int i = 0; i < mapWidth; ++i) {
            actorLocations.Add(new List<Actor>());
            for (int j = 0; j < mapHeight; ++j) {
                actorLocations[i].Add(null);
            }
        }
    }

    void ValidateLocation(Vector2Int loc) {
        Debug.Assert(loc.x >= 0);
        Debug.Assert(loc.y >= 0);
        Debug.Assert(loc.x < mapWidth);
        Debug.Assert(loc.y < mapHeight);
    }
}
