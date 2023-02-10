using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveComponent : MonoBehaviour
{
    class TargetPosition
    {
        public Vector3 targetPos;
        public float duration;
        public TargetPosition(Vector3 targetPos, float duration)
        {
            this.targetPos = targetPos;
            this.duration = duration;
        }
        public Vector3 getTargetPos()
        {
            return targetPos;
        }

        public void setTargetPos(Vector3 targetPos)
        {
            this.targetPos = targetPos;
        }

        public float getDuration()
        {
            return duration;
        }

        public void setDuration(float duration)
        {
            this.duration = duration;
        }
    }

    private Vector3 start;
    private Queue<TargetPosition> targetPositions = new Queue<TargetPosition>();
    private float timePassed;

    public MoveComponent moveTo(Vector3 target, float duration)
    {
        targetPositions.Clear();
        start = transform.position;
        timePassed = 0;
        if (target == start)
        {
            return this;
        }
        targetPositions.Enqueue(new TargetPosition(target, duration));
        return this;
    }
    public MoveComponent thenMoveTo(Vector3 target, float duration)
    {
        if (targetPositions.Count > 0)
        {
            targetPositions.Enqueue(new TargetPosition(target, duration));
        }
        else
        {
            moveTo(target, duration);
        }
        return this;
    }
    void Update()
    {
        if (targetPositions.Count > 0)
        {
            timePassed += Time.deltaTime;
            Vector2 newPosition;
            TargetPosition target = targetPositions.Peek();
            float t = timePassed / target.duration;
            if (t > 1)
            {
                t = 1;
            }
            newPosition = start + t * (target.targetPos - start);
            transform.position = newPosition;
            if (t == 1)
            {
                TargetPosition oldTarget = targetPositions.Dequeue();
                start = oldTarget.targetPos;
                timePassed = 0;
                return;
            }
        }
    }

    public bool isMoving()
    {
        return targetPositions.Count > 0;
    }
}
