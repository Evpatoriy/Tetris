using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

enum Direction {
    left,
    right,
    up,
    down
}

public class Tetramino : MonoBehaviour
{
    float fall = 0;
    public float fallSpeed = 1;
    public bool allowRotation = true;
    public bool limitRotation = false;
    private bool buttonPressed = false;
    private Vector2 startTouchPosition;
    private Vector2 currentPosition;
    private Vector2 endTouchPosition;
    private bool stopTouch = false;
    private float swipeRange = 100;
    public float tapRange;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        CheckUserInput();
        Swipe();
    }

    void changePosition (Direction direction) {

        if (direction == Direction.right && this.buttonPressed) {

            transform.position += new Vector3(1, 0, 0);

            if (CheckIsValidPosition()) {

                FindObjectOfType<Game>().UpdateGrid(this);

            } else {

                transform.position += new Vector3(-1, 0, 0);
            }
            this.buttonPressed = false;
        }

        if (direction == Direction.left && this.buttonPressed) {

            transform.position += new Vector3(-1, 0, 0);

            if (CheckIsValidPosition()) {

                FindObjectOfType<Game>().UpdateGrid(this);

            } else {

                transform.position += new Vector3(1, 0, 0);
            }
            this.buttonPressed = false;
        }

        if (direction == Direction.up && this.buttonPressed) {

            if (allowRotation) {

                if (limitRotation) {

                    if (transform.rotation.eulerAngles.z >= 90) {

                        transform.Rotate(0, 0, -90);
                    } else {

                        transform.Rotate(0, 0, 90);
                    }

                } else {
                    transform.Rotate(0, 0, 90);
                }

                if (CheckIsValidPosition()) {

                    FindObjectOfType<Game>().UpdateGrid(this);

                } else {

                    if (limitRotation) {

                        if (transform.rotation.eulerAngles.z >= 90) {
                            transform.Rotate(0, 0, -90);
                        } else {
                            transform.Rotate(0, 0, 90);
                            }
                    } else {
                        transform.Rotate(0, 0, -90);
                    }
                }
            }
            this.buttonPressed = false;
        }

        if (direction == Direction.down && this.buttonPressed) {
            ImmediateDown();
            this.buttonPressed = false;
        }
    }

    void CheckUserInput() {
        if (Input.GetKeyDown(KeyCode.RightArrow)) {

            this.buttonPressed = true;
            this.changePosition(Direction.right);

        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {

            this.buttonPressed = true;
            this.changePosition(Direction.left);

        }
        if (Input.GetKeyDown(KeyCode.UpArrow)) {

            this.buttonPressed = true;
            this.changePosition(Direction.up);

        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            this.buttonPressed = true;
            this.changePosition(Direction.down);
        } if (Time.time - fall >= fallSpeed) {
            transform.position += new Vector3(0, -1, 0);
            
            if (CheckIsValidPosition()) {
                FindObjectOfType<Game>().UpdateGrid(this);
            } else {
                transform.position += new Vector3(0, 1, 0);
                FindObjectOfType<Game>().DeleteRow();
                if (FindObjectOfType<Game>().CheckIsAboveGrid (this)) {
                    FindObjectOfType<Game>().GameOver();
                }
                enabled = false;
                FindObjectOfType<Game>().SpawnNextTetramino();
            }
            fall = Time.time;
        }
    }

    void ImmediateDown () {
        while(CheckIsValidPosition()) {
            FindObjectOfType<Game>().UpdateGrid(this);
            transform.position += new Vector3(0, -1, 0);
        }
        transform.position += new Vector3(0, 1, 0);
    }
    

    public void Swipe() {

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
            startTouchPosition = Input.GetTouch(0).position;
        }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) {
            currentPosition = Input.GetTouch(0).position;
            Vector2 Distance = currentPosition - startTouchPosition;

            if (!stopTouch) {
                this.buttonPressed = true;
                if (Distance.x < -swipeRange) {
                    this.changePosition(Direction.left);
                    stopTouch = true;
                }
                if (Distance.x > swipeRange) {
                    this.changePosition(Direction.right);
                    stopTouch = true;
                }
                if (Distance.y > swipeRange) {
                    this.changePosition(Direction.up);
                    stopTouch = true;
                }
                if (Distance.y < -swipeRange) {
                    this.changePosition(Direction.down);
                    stopTouch = true;
                }
            }
        }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) {
            stopTouch = false;
            endTouchPosition = Input.GetTouch(0).position;
            Vector2 Distance = endTouchPosition - startTouchPosition;

            if (Mathf.Abs(Distance.x) < tapRange && Mathf.Abs(Distance.y) < tapRange) {
                print("tap");
            }
        }
    }

    bool CheckIsValidPosition() {

        foreach (Transform mino in transform) {

            Vector2 pos = FindObjectOfType<Game>().Round (mino.position);

            if (FindObjectOfType<Game>().CheckIsInsideGrid (pos) == false) {

                return false;
            }

            if (FindObjectOfType<Game>().GetTransformAtGridPosition(pos) != null && FindObjectOfType<Game>().GetTransformAtGridPosition(pos).parent != transform) {

                return false;
            }
        }
        return true;
    }
}
